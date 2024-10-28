using Lab1.GeometryModel.Edges;

namespace Lab1.Visitors.CorrectionStatusVisitors
{
    public interface IEdgeCorrectionStatusVisitor
    {
        Edge.CorrectionStatus Visit(Edge edge);
        Edge.CorrectionStatus Visit(HorizontalEdge edge);
        Edge.CorrectionStatus Visit(VerticalEdge edge);
        Edge.CorrectionStatus Visit(FixedEdge edge);
        Edge.CorrectionStatus Visit(BezierEdge edge);
    }
}
