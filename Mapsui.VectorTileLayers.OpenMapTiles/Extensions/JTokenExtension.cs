using Newtonsoft.Json.Linq;
using System;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Extensions
{
    public static class JTokenExtension
    {
        public static object GetValue(this JToken token, Type resultType)
        {
            var value = token.Type switch
            {
                JTokenType.String => token.Value<string>(),
                JTokenType.Object => token.Value<object>(),
                JTokenType.Float => token.Value<float>(),
                JTokenType.Integer => token.Value<int>(),
                JTokenType.Boolean => token.Value<bool>(),
                JTokenType.Date => token.Value<DateTime>(),
                JTokenType.Array => token.Value<Array>(),
                JTokenType.Guid => token.Value<Guid>(),
                _ => token
            };
            if (resultType == null)
                return value;
            //Дополнительное преобразование для цвета
            if (resultType == typeof(SkiaSharp.SKColor))
                value = value.ToString().FromString();
            if (resultType == typeof(float) && value.GetType() == typeof(int))
                value = Convert.ToSingle(value);
            if (resultType == typeof(int) && value.GetType() == typeof(float))
                value = Convert.ToInt32(value);
            if (resultType == typeof(bool) && value.GetType() == typeof(int))
                value = (int)value == 1;

            if (resultType != value.GetType())
            {
                Console.WriteLine("Mismatch of the resulting type");
                return null;
            }
            return value;
        }
    }
}
