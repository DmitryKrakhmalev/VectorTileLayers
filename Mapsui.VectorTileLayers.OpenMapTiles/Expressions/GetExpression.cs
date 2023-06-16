using Mapsui.VectorTileLayers.Core.Interfaces;
using Mapsui.VectorTileLayers.Core.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Expressions
{
    public class GetExpression: Expression
    {
        public static IExpression Parse(JToken token, ExpressionParser parser)
        {
            if (token == null || !(token is JArray array))
                throw new ArgumentException("");

            var length = array.Count;

            if (length != 2)
            {
                parser.Error("Expected one argument, but found " + length.ToString() + " instead.");
                return null;
            }
            return new GetExpression(array[1].ToString());
        }

        private string _propertyName;
        public GetExpression(string propertyName)
        { 
            _propertyName = propertyName;
        }

        public override object Evaluate(EvaluationContext ctx = null)
        {
            //Get value from semantic
            var elem = ctx?.Tags?.KeyValues.FirstOrDefault(x => x.Key == _propertyName);
            if (elem == null)
                return null;
            return  elem.Value.Value;
        }

        public override object PossibleOutputs()
        {
            throw new NotImplementedException();
        }
    }
}
