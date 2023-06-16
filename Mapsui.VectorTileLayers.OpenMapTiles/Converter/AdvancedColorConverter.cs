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
    public class AdvancedColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(JsonStoppedColor) || objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            try
            {
                switch (token.Type)
                {
                    case JTokenType.Object:
                        var stoppedColor = new AdvancedColor { Stops = new List<KeyValuePair<float, SKColor>>() };
                        stoppedColor.Base = token.SelectToken("base") != null ? token.SelectToken("base").ToObject<float>() : 1f;

                        foreach (var stop in token.SelectToken("stops"))
                        {
                            var zoom = (float)stop.First.ToObject<float>();
                            var colorString = stop.Last.ToObject<string>();
                            stoppedColor.Stops.Add(new KeyValuePair<float, SKColor>(zoom, colorString.FromString()));
                        }

                        return stoppedColor;
                    case JTokenType.Array:
                        //Обработчик выражений, таких как match, case, step и т.п.
                        return new AdvancedColor() { Expression = ExpressionParser.ParseExpression(token.ToString(), typeof(SKColor)) };
                    default:
                        return new AdvancedColor() { SingleVal = token.Value<string>().FromString() };
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
