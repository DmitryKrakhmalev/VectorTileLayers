using Mapsui.VectorTileLayers.Core.Interfaces;
using Mapsui.VectorTileLayers.Core.Primitives;
using Mapsui.VectorTileLayers.OpenMapTiles.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Expressions
{
    public class MatchExpression : Expression
    {
        private IExpression _expression;
        //Тип условия может быть int, string - пока просто привожу к строке
        private Dictionary<string, object> _values;
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
            var values = new Dictionary<string, object>();

            for (var i = 2; i < length - 1; i += 2)
            {
                var match = array[i].ToString();
                var elem = array[i + 1];
                if (match == null)
                    return null;
                values[match] = elem.GetValue(parser.Expected);
            }
            var defaultValue = array.Last.GetValue(parser.Expected);
            return new MatchExpression(expression, values, defaultValue);
        }

        public MatchExpression(IExpression expression, Dictionary<string, object> values, object defaultValue)
        {
            _expression = expression;
            _values = values;
            _default = defaultValue;
        }

        public override object Evaluate(EvaluationContext ctx)
        {
            var value = _expression.Evaluate(ctx);
            if (value == null || !_values.ContainsKey(value.ToString()))
                return _default;
            var matchedValue = _values[value.ToString()];
            return matchedValue;
        }

        public override object PossibleOutputs()
        {
            return _default;
        }
    }
}
