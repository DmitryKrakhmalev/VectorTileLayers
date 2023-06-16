using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Mapsui.VectorTileLayers.OpenMapTiles.Expressions;
using Mapsui.VectorTileLayers.OpenMapTiles.Json;
using Mapsui.Logging;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Converter
{
    public class AdvancedFloatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(JsonStoppedFloat) || objectType == typeof(int);
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
                        var stoppedFloat = new AdvancedFloat { Stops = new List<KeyValuePair<float, float>>() };
                        stoppedFloat.Base = token.SelectToken("base") != null ? token.SelectToken("base").ToObject<float>() : 1f;


                        foreach (var stop in token.SelectToken("stops"))
                        {
                            var zoom = (float)stop.First.ToObject<float>();
                            var value = stop.Last.ToObject<float>();
                            stoppedFloat.Stops.Add(new KeyValuePair<float, float>(zoom, value));
                        }

                        return stoppedFloat;
                    case JTokenType.Array:
                        //Обработчик выражений, таких как match, case, step и т.п.
                        return new AdvancedFloat() { Expression = ExpressionParser.ParseExpression(token.ToString(), typeof(float)) };
                    default:
                        return new AdvancedFloat() { SingleVal = token.Value<float>() };
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, $"Error convert {token} to StoppedFloat. Path {reader.Path}", ex);
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
