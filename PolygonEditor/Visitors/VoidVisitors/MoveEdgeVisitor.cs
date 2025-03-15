using PolygonEditor.GeometryModel;
using PolygonEditor.GeometryModel.Edges;

namespace PolygonEditor.Visitors.VoidVisitors
{
    public class MoveEdgeVisitor : IEdgeVoidVisitor
    {
        public float Dx { get; set; }
        public float Dy { get; set; }
        private void MoveVertex(Vertex v)
        {
            v.Move(Dx, Dy);
        }
        public void Visit(Edge edge) => MoveVertex(edge.Start);
        public void Visit(HorizontalEdge edge) => MoveVertex(edge.Start);
        public void Visit(VerticalEdge edge) => MoveVertex(edge.Start);
        public void Visit(FixedEdge edge) => MoveVertex(edge.Start);
        public void Visit(BezierEdge edge)
        {
            MoveVertex(edge.Start);
            MoveVertex(edge.V1);
            MoveVertex(edge.V2);
        }
    }
}
