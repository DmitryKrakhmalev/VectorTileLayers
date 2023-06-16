using Mapsui.VectorTileLayers.Core.Interfaces;
using Mapsui.VectorTileLayers.Core.Primitives;
using Mapsui.VectorTileLayers.OpenMapTiles.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Expressions
{
    public class CaseExpression : Expression
    {
        private Dictionary<IExpression, object> _values;
        private object _default;
        public static IExpression Parse(JToken token, ExpressionParser parser)
        {
            if (token == null || !(token is JArray array))
                throw new ArgumentException("");

            var length = array.Count;
            if (length < 4)
            {
                parser.Error($"Minimum 4 arguments must be passed, but found {length - 1} instead.");
                return null;
            }
            var values = new Dictionary<IExpression, object>();
            for (var i = 1; i < length - 1; i += 2)
            {
                var expression = parser.Parse(array[i], 1);
                var value = array[i + 1].GetValue(parser.Expected);
                values.Add(expression, value);
            }
            var defaultValue = array.Last.GetValue(parser.Expected);
            return new CaseExpression(values, defaultValue);
        }

        public CaseExpression(Dictionary<IExpression, object> values, object defaultValue)
        {
            _values = values;
            _default = defaultValue;
        }

        public override object Evaluate(EvaluationContext ctx)
        {
            foreach (var test in _values)
            {
                if (test.Key.Evaluate(ctx) is true)
                    return test.Value;
            }
            return _default;
        }

        public override object PossibleOutputs()
        {
            return _default;
        }
    }
}
