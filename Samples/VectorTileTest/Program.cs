using BruTile;
using Mapsui.Layers;
using Mapsui.VectorTileLayers.Core.Enums;
using Mapsui.VectorTileLayers.OpenMapTiles;
public class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello, World!");

        OMTStyleFileLoader.DirectoryForFiles = ".\\mbtiles";
        var layers = LoadMapboxGL("styles/isogd/isogd.json");

        var findLayer = layers.FirstOrDefault(x => x is OMTVectorTileLayer && x.Name == "SpecialZone");
        if (findLayer == null || findLayer is not OMTVectorTileLayer vectorLayer)
            return;
        var tileIndex = new TileIndex(1309, 1384, 11);
        var tileInfo = new TileInfo() { Index = tileIndex };
        var filePath = Path.Combine(Environment.CurrentDirectory, "cache", $"{vectorLayer.Name}_as_{tileIndex.Col}_{tileIndex.Row}_{tileIndex.Level}.png");
        vectorLayer.SaveTileImage(tileInfo, filePath);

        Console.WriteLine("End!");
    }
    private static List<ILayer> LoadMapboxGL(string stylePath)
    {
        var layers = new List<ILayer>();
        using var stream = new FileStream(stylePath, FileMode.Open);
        var openMapLayers = new OpenMapTilesLayer(stream, GetLocalContent);
        foreach (var layer in openMapLayers)
            layers.Add(layer);
        return layers;
    }

    private static Stream GetLocalContent(LocalContentType type, string name)
    {
        switch (type)
        {
            case LocalContentType.File:
                if (File.Exists(name))
                    return File.OpenRead(name);
                else
                    return null;
            case LocalContentType.Resource:
                throw new NotImplementedException("Не поддерживается");
        }
        return null;
    }

}