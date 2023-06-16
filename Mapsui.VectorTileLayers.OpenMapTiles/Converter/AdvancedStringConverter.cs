using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Mapsui.VectorTileLayers.OpenMapTiles.Expressions;
using Mapsui.VectorTileLayers.OpenMapTiles.Json;
using Mapsui.Logging;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Converter
{
    public class AdvancedStringConverter : ExpressionConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(JsonStoppedString) || objectType == typeof(string);
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
                        var stoppedString = new AdvancedString
                        {
                            Stops = new List<KeyValuePair<float, string>>(),
                            Base = token.SelectToken("base").ToObject<float>()
                        };

                        foreach (var stop in token.SelectToken("stops"))
                        {
                            var zoom = (float)stop.First.ToObject<float>();
                            var text = stop.Last.ToObject<string>();
                            stoppedString.Stops.Add(new KeyValuePair<float, string>(zoom, text));
                        }

                        return stoppedString;
                    case JTokenType.Array:
                        //Обработчик выражений, таких как match, case, step и т.п.
                        return new AdvancedString() { Expression = ExpressionParser.ParseExpression(token.ToString(), typeof(string)) };

                    default:
                        return new AdvancedString() { SingleVal = token.Value<string>() };
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, $"Error convert {token} to StoppedString. Path {reader.Path}", ex);
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
