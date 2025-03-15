using PolygonEditor.GeometryModel.Edges;

namespace PolygonEditor.Visitors.CorrectionStatusVisitors
{
    public interface IEdgeCorrectionStatusVisitable
    {
        Edge.CorrectionStatus Accept(IEdgeCorrectionStatusVisitor visitor);
    }
}
