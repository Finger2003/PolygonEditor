using Lab1.GeometryModel.Edges;

namespace Lab1.GeometryModel.EdgeFactories
{
    public class VerticalEdgeFactory : EdgeFactory
    {
        public override Edge CreateEdge(Vertex start, Vertex end)
        {
            return new VerticalEdge(start, end);
        }
    }
}
