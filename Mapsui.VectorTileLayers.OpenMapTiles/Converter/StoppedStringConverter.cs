using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Mapsui.VectorTileLayers.OpenMapTiles.Expressions;
using Mapsui.VectorTileLayers.OpenMapTiles.Json;
using Mapsui.Logging;
using Mapsui.VectorTileLayers.Core.Interfaces;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Converter
{
    public class StoppedStringConverter : ExpressionConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(JsonStoppedString) || objectType == typeof(string);
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
                        var stoppedString = new StoppedString
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
                        //Пока заглушаю... не знаю как это обработать
                        var exp = ExpressionParser.ParseExpression(token.ToString(), typeof(string));
                        if (exp is StepExpression step)
                        {
                            var value = step.ObjectType switch
                            {
                                "string" => step.Value.ToString(),
                                //Для дополнительной логики с разными типами
                                _ => step.Value.ToString()
                            };
                            return new StoppedString() { SingleVal = value };
                        }
                        else
                            return new StoppedString() { SingleVal = "TEST" };

                    default:
                        return new StoppedString() { SingleVal = token.Value<string>() };
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
