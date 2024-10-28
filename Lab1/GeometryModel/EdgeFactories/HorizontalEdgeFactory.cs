using Lab1.GeometryModel.Edges;

namespace Lab1.GeometryModel.EdgeFactories
{
    public class HorizontalEdgeFactory : EdgeFactory
    {
        public override Edge CreateEdge(Vertex start, Vertex end)
        {
            return new HorizontalEdge(start, end);
        }
    }
}
