using Lab1.GeometryModel.Edges;

namespace Lab1.GeometryModel.EdgeFactories
{
    public abstract class EdgeFactory
    {
        public abstract Edge CreateEdge(Vertex start, Vertex end);
    }
}
