using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Mapsui.VectorTileLayers.OpenMapTiles.Expressions;
using Mapsui.VectorTileLayers.OpenMapTiles.Extensions;
using Mapsui.VectorTileLayers.OpenMapTiles.Json;
using Mapsui.Logging;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Converter
{
    public class StoppedColorConverter : JsonConverter
    {
        private Random _random = new Random();
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(JsonStoppedString) || objectType == typeof(string);
            //return typeof(StoppedDouble).IsAssignableFrom(objectType) || typeof(int).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            try
            {
                switch (token.Type)
                {
                    case JTokenType.Object:
                        var stoppedColor = new StoppedColor { Stops = new List<KeyValuePair<float, SKColor>>() };

                        if (token.SelectToken("base") != null)
                            stoppedColor.Base = token.SelectToken("base").ToObject<float>();
                        else
                            stoppedColor.Base = 1f;

                        foreach (var stop in token.SelectToken("stops"))
                        {
                            var zoom = (float)stop.First.ToObject<float>();
                            var colorString = stop.Last.ToObject<string>();
                            stoppedColor.Stops.Add(new KeyValuePair<float, SKColor>(zoom, colorString.FromString()));
                        }

                        return stoppedColor;
                    case JTokenType.Array:
                        //Нужен обработчик вырожений, таких как match, case, step и т.п.
                        return new StoppedColor() { SingleVal = $"hsla({_random.Next(1, 251)}, 54%, 68%, 0.3)".FromString() };
                    default:
                        return new StoppedColor() { SingleVal = token.Value<string>().FromString() };
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, $"Error convert {token} to StoppedColor. Path {reader.Path}", ex);
                throw;
            }
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
