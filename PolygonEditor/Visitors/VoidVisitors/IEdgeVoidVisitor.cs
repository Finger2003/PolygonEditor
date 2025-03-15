using PolygonEditor.GeometryModel.Edges;

namespace PolygonEditor.Visitors.VoidVisitors
{
    public interface IEdgeVoidVisitor
    {
        void Visit(Edge edge);
        void Visit(HorizontalEdge edge);
        void Visit(VerticalEdge edge);
        void Visit(FixedEdge edge);
        void Visit(BezierEdge edge);
    }
}
