using PolygonEditor.GeometryModel.Edges;

namespace PolygonEditor.GeometryModel.EdgeFactories
{
    public class HorizontalEdgeFactory : EdgeFactory
    {
        public override Edge CreateEdge(Vertex start, Vertex end)
        {
            return new HorizontalEdge(start, end);
        }
    }
}
