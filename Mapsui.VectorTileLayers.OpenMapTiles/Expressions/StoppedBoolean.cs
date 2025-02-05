﻿using Mapsui.VectorTileLayers.Core.Interfaces;
using Mapsui.VectorTileLayers.Core.Primitives;
using System.Collections.Generic;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Expressions
{
    /// <summary>
    /// Class holding StoppedBoolean data
    /// </summary>
    public class StoppedBoolean : IExpression
    {
        public IList<KeyValuePair<float, bool>> Stops { get; set; }

        public bool? SingleVal { get; set; } = null;

        /// <summary>
        /// Calculate the correct boolean for a stopped function
        /// No StoppsType needed, because booleans couldn't interpolated :)
        /// </summary>
        /// <param name="contextZoom">Zoom factor for calculation </param>
        /// <returns>Value for this stopp respecting resolution factor and type</returns>
        public bool Evaluate(float? contextZoom)
        {
            // Are there no stopps, but a single value?
            if (SingleVal != null)
                return (bool)SingleVal;

            // Are there no stopps in array
            if (Stops.Count == 0)
                return false;

            float zoom = contextZoom ?? 0f;

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

        public object Evaluate(EvaluationContext ctx)
        {
            return Evaluate(ctx.Zoom);
        }

        public object PossibleOutputs()
        {
            throw new System.NotImplementedException();
        }
    }
}
