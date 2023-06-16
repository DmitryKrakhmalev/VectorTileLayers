using Mapsui.VectorTileLayers.Core.Interfaces;
using Mapsui.VectorTileLayers.Core.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Expressions
{
    /// <summary>
    /// Class holding AdvancedString data
    /// </summary>
    public class AdvancedString : IExpression
    {
        public float Base { get; set; } = 1f;

        public IList<KeyValuePair<float, string>> Stops { get; set; }

        public string SingleVal { get; set; } = string.Empty;

        public IExpression Expression;

        public bool IsEvaluated => Expression != null || (Stops != null && Stops.Count > 0);

       
        public object Evaluate(EvaluationContext context)
        {
            if (Expression != null)
                if (Expression.Evaluate(context) is string text)
                    return text;


            // Are there no stopps, but a single value?
            if (SingleVal != string.Empty)
                return SingleVal;

            // Are there no stopps in array
            if (Stops == null || Stops.Count == 0)
                return string.Empty;

            var zoom = context.Zoom ?? 0f;

            var lastZoom = Stops[0].Key;
            var lastValue = Stops[0].Value;

            if (lastZoom > zoom)
                return lastValue;

            for (int i = 1; i < Stops.Count; i++)
            {
                var nextZoom = Stops[i].Key;
                var nextValue = Stops[i].Value;

                if (zoom == nextZoom)
                    return nextValue;

                if (lastZoom <= zoom && zoom < nextZoom)
                {
                    return lastValue;
                }

                lastZoom = nextZoom;
                lastValue = nextValue;
            }

            return lastValue;
        }

        public object PossibleOutputs()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString() => SingleVal ?? Stops?.FirstOrDefault().Value;
    }
}
