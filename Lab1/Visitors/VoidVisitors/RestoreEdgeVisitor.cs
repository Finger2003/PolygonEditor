using Lab1.GeometryModel;
using Lab1.GeometryModel.Edges;

namespace Lab1.Visitors.VoidVisitors
{
    public class RestoreEdgeVisitor : IEdgeVoidVisitor
    {
        private void RestoreVertex(Vertex vertex)
        {
            vertex.Restore();
        }
        public void Visit(Edge edge) => RestoreVertex(edge.Start);
        public void Visit(HorizontalEdge edge) => RestoreVertex(edge.Start);
        public void Visit(VerticalEdge edge) => RestoreVertex(edge.Start);
        public void Visit(FixedEdge edge) => RestoreVertex(edge.Start);
        public void Visit(BezierEdge edge)
        {
            RestoreVertex(edge.Start);
            RestoreVertex(edge.V1);
            RestoreVertex(edge.V2);
        }
    }
}
