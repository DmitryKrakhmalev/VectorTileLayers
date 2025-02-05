﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Mapsui.VectorTileLayers.OpenMapTiles.Expressions;
using Mapsui.VectorTileLayers.OpenMapTiles.Json;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Converter
{
    public class StoppedFloatArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(JsonStoppedFloat) || objectType == typeof(int);
            //return typeof(StoppedDouble).IsAssignableFrom(objectType) || typeof(int).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Object)
            {
                var stoppedFloat = new StoppedFloatArray { Stops = new List<KeyValuePair<float, float[]>>() };

                if (token.SelectToken("base") != null)
                    stoppedFloat.Base = token.SelectToken("base").ToObject<float>();
                else
                    stoppedFloat.Base = 1f;

                foreach (var stop in token.SelectToken("stops"))
                {
                    var zoom = (float)stop.First.ToObject<float>();
                    var value = stop.Last.ToObject<float[]>();
                    stoppedFloat.Stops.Add(new KeyValuePair<float, float[]>(zoom, value));
                }

                return stoppedFloat;
            }

            return new StoppedFloatArray() { SingleVal = token.ToObject<float[]>() };
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
