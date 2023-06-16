using Mapsui.VectorTileLayers.Core.Interfaces;
using Mapsui.VectorTileLayers.Core.Primitives;
using Mapsui.VectorTileLayers.OpenMapTiles.Extensions;
using Newtonsoft.Json.Linq;
using System;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Expressions
{
    public class ComparisonExpression : Expression
    {
        private readonly string _operation;
        private readonly IExpression _expression;
        private readonly IComparable _value;

        public static IExpression Parse(JToken token, ExpressionParser parser)
        {
            if (token == null || !(token is JArray array))
                throw new ArgumentException("");
            var length = array.Count;
            if (length != 3)
            {
                parser.Error($"3 arguments must be passed, but found {length - 1} instead.");
                return null;
            }
            var operation = array[0].ToString();
            var expression = parser.Parse(array[1], 1);
            var value = array.Last.GetValue(parser.Expected);
            return new ComparisonExpression(operation, expression, value);
        }

        public ComparisonExpression(string operation, IExpression expression, object value)
        {
            _operation = operation;
            _expression = expression;
            _value = value as IComparable;
        }

        public override object Evaluate(EvaluationContext ctx)
        {
            var expressonValue = _expression.Evaluate(ctx) as IComparable;
            var compare = expressonValue.CompareTo(_value);
            switch (_operation)
            {
                case "==":
                    return compare == 0;
                case "!=":
                    return compare != 0;
                case ">=":
                    return compare >= 0;
                case "<=":
                    return compare <= 0;
                case "<":
                    return compare < 0;
                case ">":
                    return compare > 0;
            }
            return false;
        }

        public override object PossibleOutputs()
        {
            return false;
        }
    }
}
