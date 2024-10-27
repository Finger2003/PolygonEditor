using Lab1.GeometryModel.Edges;

namespace Lab1.Visitors
{
    public interface IEdgeVisitor
    {
        void Visit(Edge edge);
        void Visit(HorizontalEdge edge);
        void Visit(VerticalEdge edge);
        void Visit(FixedEdge edge);
        void Visit(BezierEdge edge);
    }
}
