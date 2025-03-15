using PolygonEditor.GeometryModel.Edges;

namespace PolygonEditor.GeometryModel.EdgeFactories
{
    public abstract class EdgeFactory
    {
        public abstract Edge CreateEdge(Vertex start, Vertex end);
    }
}
