using Mapsui.VectorTileLayers.Core.Interfaces;
using Mapsui.VectorTileLayers.Core.Primitives;
using Newtonsoft.Json.Linq;

namespace Mapsui.VectorTileLayers.OpenMapTiles.Expressions
{
    //Возвращает текущий уровень масштабирования
    public class ZoomExpression : Expression
    {
        private const int DefaultZoom = 10;
        public static IExpression Parse(JArray array, ExpressionParser parser) => new ZoomExpression(); 

        public ZoomExpression()
        {
        }

        public override object Evaluate(EvaluationContext ctx)
        {
            return ctx?.Zoom ?? DefaultZoom;
        }

        public override object PossibleOutputs()
        {
            return DefaultZoom;
        }
    }
}
