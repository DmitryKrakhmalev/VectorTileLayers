﻿using Mapsui.VectorTileLayers.Core.Interfaces;
using Mapsui.VectorTileLayers.Core.Primitives;
using System;
using System.Collections.Generic;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Expressions
{
    /// <summary>
    /// Class holding AdvancedFloat data in Json format
    /// </summary>
    public class AdvancedFloat : IExpression
    {
        public float Base { get; set; } = 1f;

        public IList<KeyValuePair<float, float>> Stops { get; set; }

        public float SingleVal { get; set; } = float.MinValue;

        public IExpression Expression;
        public bool IsEvaluated => Expression != null || (Stops != null && Stops.Count > 0);

        /// <summary>
        /// Calculate the correct value
        /// No Bezier type up to now
        /// </summary>
        /// <param name="contextZoom">Zoom factor for calculation </param>
        /// <param name="stoppsType">Type of calculation (interpolate, exponential, categorical)</param>
        /// <returns>Value for this stopp respecting zoom factor and type</returns>
        public float Evaluate(EvaluationContext context, StopsType stoppsType = StopsType.Exponential)
        {
            //Если имеется сложное правило расчета цвета - пытаемся получить цвет обработав условие
            if (Expression != null)
                if (Expression.Evaluate(context) is float value)
                    return value;

            // Are there no stopps, but a single value?
            // !=
            if (SingleVal - float.MinValue > float.Epsilon)
                return SingleVal;

            // Are there no stopps in array
            if (Stops.Count == 0)
                return 0;

            float zoom = context.Zoom ?? 0f;

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
                    switch (stoppsType)
                    {
                        case StopsType.Interval:
                            return lastValue;
                        case StopsType.Exponential:
                            var progress = zoom - lastZoom;
                            var difference = nextZoom - lastZoom;
                            if (difference < float.Epsilon)
                                return 0;
                            if (Base - 1.0f < float.Epsilon)
                                return lastValue + (nextValue - lastValue) * progress / difference;
                            else
                            {
                                //var r = FromResolution(resolution);
                                //var lr = FromResolution(lastResolution);
                                //var nr = FromResolution(nextResolution);
                                //var logBase = Math.Log(Base);
                                //return lastValue + (float)((nextValue - lastValue) * (Math.Pow(Base, lr-r) - 1) / (Math.Pow(Base, lr-nr) - 1));
                                //return lastValue + (float)((nextValue - lastValue) * (Math.Exp(progress * logBase) - 1) / (Math.Exp(difference * logBase) - 1)); // (Math.Pow(Base, progress) - 1) / (Math.Pow(Base, difference) - 1));
                                return lastValue + (float)((nextValue - lastValue) * (Math.Pow(Base, progress) - 1) / (Math.Pow(Base, difference) - 1));
                            }
                        case StopsType.Categorical:
                            if (nextZoom - zoom < float.Epsilon)
                                return nextValue;
                            break;
                    }
                }

                lastZoom = nextZoom;
                lastValue = nextValue;
            }

            return lastValue;
        }

        public object Evaluate(EvaluationContext ctx)
        {
            return Evaluate(ctx, StopsType.Exponential);
        }

        public object PossibleOutputs()
        {
            throw new NotImplementedException();
        }
    }
}
