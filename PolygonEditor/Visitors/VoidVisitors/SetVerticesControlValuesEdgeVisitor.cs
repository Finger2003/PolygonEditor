using PolygonEditor.GeometryModel;
using PolygonEditor.GeometryModel.Edges;

namespace PolygonEditor.Visitors.VoidVisitors
{
    public class SetVerticesControlValuesEdgeVisitor : PolygonShapeKeepingEdgeVisitor, IEdgeVoidVisitor
    {
        public Vertex? Vertex { get; set; }
        private void SetStraightEdgeControlValues(Edge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;
            Vertex!.ControlAngle = GetControlAngle(start, end);
            Vertex!.ControlLength = GetControlLength(edge);
        }
        public void Visit(Edge edge) => SetStraightEdgeControlValues(edge);
        public void Visit(HorizontalEdge edge) => SetStraightEdgeControlValues(edge);
        public void Visit(VerticalEdge edge) => SetStraightEdgeControlValues(edge);
        public void Visit(FixedEdge edge) => SetStraightEdgeControlValues(edge);

        public void Visit(BezierEdge edge)
        {
            Vertex v0 = edge.Start;
            Vertex v1 = edge.V1;
            Vertex v2 = edge.V2;
            Vertex v3 = edge.End;

            if (Vertex == v0 || Vertex == v1)
            {
                v0.ControlAngle = GetControlAngle(v0, v1);
                v0.ControlLength = GetBezierControlLength(v0, v1);
                v1.WasMoved = true;
            }
            else if (Vertex == v2 || Vertex == v3)
            {
                v3.ControlAngle = GetControlAngle(v2, v3);
                v3.ControlLength = GetBezierControlLength(v2, v3);
                v2.WasMoved = true;
            }
        }
    }
}
