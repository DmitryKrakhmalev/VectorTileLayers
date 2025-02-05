﻿using Mapsui.VectorTileLayers.Core.Interfaces;
using Mapsui.VectorTileLayers.Core.Primitives;
using Mapsui.VectorTileLayers.OpenMapTiles.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Expressions
{
    public class StepExpression : Expression
    {
        private IExpression _expression;
        private Dictionary<int, IExpression> _values;
        private object _default;

        public static IExpression Parse(JArray array, ExpressionParser parser)
        {
            if (array == null)
                throw new ArgumentException("");

            var length = array.Count;
            if (length < 5)
            {
                parser.Error($"Expected 2 arguments, but found {length - 1} instead.");

                return null;
            }
            var expression = parser.Parse(array[1], 1);
            var values = new Dictionary<int, IExpression>();
            var defaultValue = array[2].GetValue(parser.Expected);
            for (var i = 3; i < length - 1; i += 2)
            {
                var match = int.Parse(array[i].ToString());
                var match_expression = parser.Parse(array[i + 1], 1);
                values[match] = match_expression;
            }

            return new StepExpression(expression, values, defaultValue);
        }

        public StepExpression(IExpression expression, Dictionary<int, IExpression> values, object defaultValue)
        {
            _expression = expression;
            _values = values;
            _default = defaultValue;
        }

        public override object Evaluate(EvaluationContext ctx)
        {
            var step = int.Parse(_expression.Evaluate(ctx)?.ToString());
            var value = _default;
            foreach(var item in _values)
            {
                if (step >= item.Key)
                {
                    value = item.Value.Evaluate(ctx);
                }
            }
            return value;

        }

        public override object PossibleOutputs()
        {
            return null;
        }
    }
}
