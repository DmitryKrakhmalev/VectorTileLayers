using Mapsui.VectorTileLayers.Core.Interfaces;
using Mapsui.VectorTileLayers.Core.Primitives;
using Newtonsoft.Json.Linq;
using System;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Expressions
{
    public class StepExpression : Expression
    {
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
            var input = parser.Parse(array[1], 1);
            var step = input switch
            {
                ZoomExpression zoom => (float)zoom.Evaluate(null),
                _ => 0
            };
            
            var outputType = array[2].HasValues ? array[2].ToString() : "string";
            for (var i = 3; i < length - 1; i += 2)
            {
                var num = int.Parse(array[i].ToString());
                var expression = parser.Parse(array[i+1], 1);
                if (expression == null)
                    return null;
                var result = expression.Evaluate(null);
                if (num > step)
                    return new StepExpression(result, outputType);
            }
            return new StepExpression(null, outputType);
        }

        public StepExpression(object curr, string objectType)
        {
            Value = curr;
            ObjectType = objectType;
        }

        public object Value;
        public string ObjectType;

        public override object Evaluate(EvaluationContext ctx)
        {
            throw new NotImplementedException();
        }

        public override object PossibleOutputs()
        {
            throw new NotImplementedException();
        }
    }
}
