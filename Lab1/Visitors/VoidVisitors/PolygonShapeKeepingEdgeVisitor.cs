using Lab1.GeometryModel;
using Lab1.GeometryModel.Edges;

namespace Lab1.Visitors.VoidVisitors
{
    public abstract class PolygonShapeKeepingEdgeVisitor
    {
        protected double GetControlAngle(Vertex v1, Vertex v2) => Math.Atan2(v2.Y - v1.Y, v2.X - v1.X);
        protected double GetControlLength(Edge edge) => edge.Length / 3;
        protected double GetBezierControlLength(Vertex v1, Vertex v2) => Vertex.Distance(v1, v2);
    }
}
