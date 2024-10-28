using Lab1.GeometryModel.Edges;

namespace Lab1.Visitors.CorrectionStatusVisitors
{
    public interface IEdgeCorrectionStatusVisitable
    {
        Edge.CorrectionStatus Accept(IEdgeCorrectionStatusVisitor visitor);
    }
}
