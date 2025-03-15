using PolygonEditor.GeometryModel.Edges;

namespace PolygonEditor.GeometryModel.EdgeFactories
{
    public class VerticalEdgeFactory : EdgeFactory
    {
        public override Edge CreateEdge(Vertex start, Vertex end)
        {
            return new VerticalEdge(start, end);
        }
    }
}
