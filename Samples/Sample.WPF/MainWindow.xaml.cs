﻿using Mapsui;
using Mapsui.Extensions;
using Mapsui.Logging;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.Styles;
using Mapsui.Utilities;
using Mapsui.VectorTileLayers.Core.Enums;
using Mapsui.VectorTileLayers.Core.Extensions;
using Mapsui.VectorTileLayers.Core.Renderer;
using Mapsui.VectorTileLayers.Core.Styles;
using Mapsui.VectorTileLayers.OpenMapTiles;
using Mapsui.Widgets.PerformanceWidget;
using Mapsui.Widgets.ScaleBar;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Sample.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Performance _performance = new (10);

        public MainWindow()
        {
            Initialize();
            var map = CreateMap();
            //Для удобства подключу подложку OpenStreetMap
            map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer());

            //Подгружаем источники из MapBox стиля
            LoadMapboxGL("styles/isogd/isogd.json");
            LoadMapboxGL("styles/isogd/gp.json");

            //Заполнить список задействованными стилями
            PrepareListView();

            //Примерно позиционируемся
            mapControl.Navigator.ZoomTo(9.ToResolution());
            mapControl.Navigator.RotateTo(0);
            mapControl.Navigator.CenterOn(5589347.7922078343, 7029295.9187208461);
        }

        private void PrepareListView()
        {
            var items = new ObservableCollection<CheckBoxListViewItem>();
            items.CollectionChanged += ListViewCollection_Changed;
            listViewStyles.ItemsSource = items;
            btnAll.Click += BtnAll_Click;
            btnNone.Click += BtnNone_Click;
            // Add handler for zoom buttons
            btnZoomIn.Click += BtnZoomIn_Click;
            btnZoomOut.Click += BtnZoomOut_Click;
            btnRotateCW.Click += BtnRotateCW_Click;
            btnRotateCCW.Click += BtnRotateCCW_Click;
            foreach (OMTVectorTileLayer vectorTileLayer in mapControl.Map.Layers.OfType<OMTVectorTileLayer>())
            {
                if (vectorTileLayer?.Style is not StyleCollection styleCollection)
                    continue;
                foreach (var vts in styleCollection.OfType<VectorTileStyle>())
                {
                    foreach (var vectorStyle in vts.StyleLayers.OfType<OMTVectorTileStyle>())
                    {
                        var item = new CheckBoxListViewItem(vectorStyle, vectorStyle.Id, vectorStyle.Enabled);
                        item.PropertyChanged += Item_PropertyChanged;
                        items.Add(item);
                    }
                }
            }
        }

        private Map CreateMap()
        {
            var map = new Map { CRS = "EPSG:3857" };
            // Add ScaleBarWidget
            map.Widgets.Add(new ScaleBarWidget(map)
            {
                TextAlignment = Mapsui.Widgets.Alignment.Center,
                HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Center,
                VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Top
            });
            // Add ScaleBarWidget
            map.Widgets.Add(new ScaleBarWidget(map)
            {
                TextAlignment = Mapsui.Widgets.Alignment.Center,
                HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Center,
                VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Top
            });

            // Add PerformanceWidget
            map.Widgets.Add(new PerformanceWidget(_performance));
            mapControl.Performance = _performance;
            mapControl.Renderer.WidgetRenders[typeof(PerformanceWidget)] = new PerformanceWidgetRenderer(10, 10, 12, SKColors.Black, SKColors.White);

            mapControl.Renderer.StyleRenderers[typeof(BackgroundTileStyle)] = new BackgroundTileStyleRenderer();
            mapControl.Renderer.StyleRenderers[typeof(RasterTileStyle)] = new RasterTileStyleRenderer();
            mapControl.Renderer.StyleRenderers[typeof(VectorTileStyle)] = new VectorTileStyleRenderer();

            mapControl.Map = map;
            return map;
        }

        private void Initialize()
        {
            InitializeComponent();
            //Директория для файлов mbtiles
            OMTStyleFileLoader.DirectoryForFiles = ".\\mbtiles";
            Logger.LogDelegate = (level, text, exception) =>
            {
                if (level == LogLevel.Information)
                    return;
                System.Diagnostics.Debug.WriteLine($"{level}: {text}, {exception}");
            };
            Topten.RichTextKit.FontMapper.Default = new Mapsui.VectorTileLayers.OpenMapTiles.Utilities.FontMapper();
            LoadFontResources(Assembly.GetAssembly(GetType()));
        }

        private void BtnRotateCCW_Click(object sender, RoutedEventArgs e)
        {
            mapControl.Navigator.RotateTo(mapControl.Viewport.Rotation - 5);
        }

        private void BtnRotateCW_Click(object sender, RoutedEventArgs e)
        {
            mapControl.Navigator.RotateTo(mapControl.Viewport.Rotation + 5);
        }

        private void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            mapControl.Navigator.ZoomIn(0);
        }

        private void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            mapControl.Navigator.ZoomOut(0);
        }

        private void BtnAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in listViewStyles.ItemsSource)
            {
                ((CheckBoxListViewItem)item).IsChecked = true;
            }
        }

        private void BtnNone_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in listViewStyles.ItemsSource)
            {
                ((CheckBoxListViewItem)item).IsChecked = false;
            }
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CheckBoxListViewItem item = (CheckBoxListViewItem)sender;

            if (item.Style.Enabled != item.IsChecked)
            {
                item.Style.Enabled = item.IsChecked;
                mapControl.ForceUpdate();
            }
        }

        private void ListViewCollection_Changed(object s, NotifyCollectionChangedEventArgs e)
        {
            foreach (CheckBoxListViewItem item in e.NewItems)
            {
                item.Style.Enabled = item.IsChecked;
                mapControl.ForceUpdate();
            }
        }
        public void LoadMapboxGL(string stylePath)
        {
            using var stream = new FileStream(stylePath, FileMode.Open);
            var layers = new OpenMapTilesLayer(stream, GetLocalContent);
            foreach (var layer in layers)
                mapControl.Map.Layers.Add(layer);
        }

        public Stream GetLocalContent(LocalContentType type, string name)
        {
            switch (type)
            {
                case LocalContentType.File:
                    if (File.Exists(name))
                        return File.OpenRead(name);
                    else
                        return null;
                case LocalContentType.Resource:
                    return EmbeddedResourceLoader.Load(name, GetType());
            }

            return null;
        }

        public void LoadFontResources(Assembly assemblyToUse)
        {
            // Try to load this font from resources
            var resourceNames = assemblyToUse?.GetManifestResourceNames();

            foreach (var resourceName in resourceNames.Where(s => s.EndsWith(".ttf", System.StringComparison.CurrentCultureIgnoreCase)))
            {
                var fontName = resourceName.Substring(0, resourceName.Length - 4);
                fontName = fontName.Substring(fontName.LastIndexOf(".") + 1);

                using var stream = assemblyToUse.GetManifestResourceStream(resourceName);
                var typeface = SKFontManager.Default.CreateTypeface(stream);
                if (typeface != null)
                {
                    ((Mapsui.VectorTileLayers.OpenMapTiles.Utilities.FontMapper)Topten.RichTextKit.FontMapper.Default).Add(typeface);
                }
            }
        }
    }
}
