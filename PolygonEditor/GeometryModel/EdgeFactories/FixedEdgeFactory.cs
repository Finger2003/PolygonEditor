using PolygonEditor.GeometryModel.Edges;

namespace PolygonEditor.GeometryModel.EdgeFactories
{
    public class FixedEdgeFactory : EdgeFactory
    {
        public int Length { get; set; }
        public FixedEdgeFactory(int length) => Length = length;
        public override Edge CreateEdge(Vertex start, Vertex end)
        {
            return new FixedEdge(start, end, Length);
        }
    }
}
