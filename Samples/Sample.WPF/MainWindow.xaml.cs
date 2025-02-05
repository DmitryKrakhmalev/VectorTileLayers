﻿using BruTile;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Logging;
using Mapsui.Rendering.Skia.SkiaWidgets;
using Mapsui.Utilities;
using Mapsui.VectorTileLayers.Core.Enums;
using Mapsui.VectorTileLayers.Core.Extensions;
using Mapsui.VectorTileLayers.Core.Renderer;
using Mapsui.VectorTileLayers.Core.Styles;
using Mapsui.VectorTileLayers.OpenMapTiles;
using Mapsui.Widgets.PerformanceWidget;
using Mapsui.Widgets.ScaleBar;
using SkiaSharp;
using System;
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
        private int _rotation;

        public MainWindow()
        {
            Initialize();
            var map = CreateMap();
            LoadMapboxGL("switzerland_zurich.mbtiles");
            PrepareListView();
            //Navigate
            map.Home = m => m.CenterOnAndZoomTo(new MPoint(825890.75, 5423194.65), m.Resolutions[7]);
        }

        private void PrepareListView()
        {
            var items = new ObservableCollection<CheckBoxListViewItem>();
            items.CollectionChanged += ListViewCollection_Changed;
            listViewStyles.ItemsSource = items;
            btnAll.Click += BtnAll_Click;
            btnNone.Click += BtnNone_Click;
            btnSaveSnapshot.Click += BtnSaveSnapshot_Click;
            btnCustomSave.Click += BtnCustomSave_Click;
            btnCustomSave2.Click += BtnCustomSave2_Click;
            // Add handler for zoom buttons
            btnZoomIn.Click += BtnZoomIn_Click;
            btnZoomOut.Click += BtnZoomOut_Click;
            btnRotateCW.Click += BtnRotateCW_Click;
            btnRotateCCW.Click += BtnRotateCCW_Click;
            foreach (OMTVectorTileLayer vectorTileLayer in mapControl.Map.Layers.OfType<OMTVectorTileLayer>())
            {
                if (vectorTileLayer?.Style is not VectorTileStyle vectorTileStyle)
                    continue;
                foreach (var vectorStyle in vectorTileStyle.StyleLayers.OfType<OMTVectorTileStyle>())
                {
                    var item = new CheckBoxListViewItem(vectorStyle, vectorStyle.Id, vectorStyle.Enabled);
                    item.PropertyChanged += Item_PropertyChanged;
                    items.Add(item);
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
            OMTStyleFileLoader.DirectoryForFiles = "C:\\Users\\dmitriy\\Source\\Repos\\Dmitry\\VectorTileLayers\\Samples\\Sample.WPF\\mbtiles";
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
            _rotation -= 5;
            mapControl.Map.Navigator.RotateTo(RotationCalculations.NormalizeRotation(_rotation));
        }

        private void BtnRotateCW_Click(object sender, RoutedEventArgs e)
        {
            _rotation += 5;
            mapControl.Map.Navigator.RotateTo(RotationCalculations.NormalizeRotation(_rotation));
        }

        private void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            mapControl.Map.Navigator.ZoomIn(0);
        }

        private void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            mapControl.Map.Navigator.ZoomOut(0);
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
        private void BtnSaveSnapshot_Click(object sender, RoutedEventArgs e)
        {
            using var file = File.Create(Path.Combine(Environment.CurrentDirectory, "cache", "snapshot.png"));
            file.Write(mapControl.GetSnapshot());
        }
        private void BtnCustomSave_Click(object sender, RoutedEventArgs e)
        {
            var viewport = new Viewport()
            {
                CenterX = 5589347.7922078343,
                CenterY = 7029295.9187208461,
                Height = 1024,
                Width = 1980,
                Resolution = ZoomExtensions.ToResolution(9),
                Rotation = 0
            };
            mapControl.Map.RefreshData(new Mapsui.Layers.FetchInfo(viewport.ToSection()));
            mapControl.RefreshGraphics();
            mapControl.ForceUpdate();
            var layers = mapControl.Map.Layers.Where(x => x is OMTVectorTileLayer).ToArray();

            using var stream = mapControl.Renderer.RenderToBitmapStream(viewport, layers);
            if (stream != null)
            {
                stream.Seek(0, SeekOrigin.Begin);
                var outDirectory = Path.Combine(Environment.CurrentDirectory, "cache");
                if (!Directory.Exists(outDirectory))
                    Directory.CreateDirectory(outDirectory);
                using var file = File.Create(Path.Combine(outDirectory, "viewport.png"));
                stream.CopyTo(file);
            }
        }
        private void BtnCustomSave2_Click(object sender, RoutedEventArgs e)
        {
            var vectorLayer = (OMTVectorTileLayer)mapControl.Map.Layers.FirstOrDefault(x => x is OMTVectorTileLayer);
            var tileIndex = new TileIndex(1309, 1384, 11);
            var filePath = Path.Combine(Environment.CurrentDirectory, "cache", $"{vectorLayer.Name}_{tileIndex.Col}_{tileIndex.Row}_{tileIndex.Level}.png");
            var tileInfo = new TileInfo() { Index = tileIndex};
            vectorLayer.SaveTileImage(tileInfo, filePath);
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
            CheckForMBTilesFile(stylePath, OMTStyleFileLoader.DirectoryForFiles);
            var stream = EmbeddedResourceLoader.Load("styles.osm-liberty.json", GetType()) ?? throw new FileNotFoundException($"styles.osm - liberty.json not found");
            var layers = new OpenMapTilesLayer(stream, GetLocalContent);
            foreach (var layer in layers)
                mapControl.Map.Layers.Add(layer);
        }

        private static string CheckForMBTilesFile(string filename, string dataDir)
        {
            filename = Path.Combine(dataDir, filename);
            if (!File.Exists(filename))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceNames = assembly.GetManifestResourceNames();
                var resourceName = resourceNames.FirstOrDefault(s => s.ToLower().EndsWith(filename) == true);
                if (resourceName != null)
                {
                    var stream = assembly.GetManifestResourceStream(resourceName);
                    using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(file);
                    }
                }
            }

            return filename;
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
