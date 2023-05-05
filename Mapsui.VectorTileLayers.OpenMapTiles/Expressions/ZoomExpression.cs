using Mapsui.VectorTileLayers.Core.Interfaces;
using Mapsui.VectorTileLayers.Core.Primitives;
using Newtonsoft.Json.Linq;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Expressions
{
    //Возвращает текущий уровень масштабирования
    public class ZoomExpression : Expression
    {
        public static IExpression Parse(JArray array, ExpressionParser parser) => new ZoomExpression(); 

        public ZoomExpression()
        {
        }

        public override object Evaluate(EvaluationContext ctx)
        {
            return ctx?.Zoom ?? 10;
        }

        public override object PossibleOutputs()
        {
            throw new System.NotImplementedException();
        }
    }
}
