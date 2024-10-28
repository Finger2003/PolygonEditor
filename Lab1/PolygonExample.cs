using Lab1.GeometryModel;
using Lab1.GeometryModel.Edges;

namespace Lab1
{
    public static class PolygonExample
    {
        private static List<Vertex> vertices { get; } =
            [
            new Vertex(237, 334),

            new Vertex(214, 116),

            new Vertex(380, 58),

            new Vertex(461, 43),

            new Vertex(565, 125),

            new Vertex(565, 334),
            ];

        private static List<Edge> edges { get; } =
            [
            new Edge(vertices[0], vertices[1]),
            new Edge(vertices[1], vertices[2]),
            new Edge(vertices[2], vertices[3]),
            new Edge(vertices[3], vertices[4]),
            new Edge(vertices[4], vertices[5]),
            new Edge(vertices[5], vertices[0]),
            ];

        private static Polygon Polygon { get; } = new();

        public static void Init()
        {
            if (Polygon.Edges.Count > 0)
                return;

            Polygon.Edges.AddRange(edges);

            Polygon.TrySetBezierCurve(0, edges[0]);
            Polygon.TrySetFixedEdge(3, edges[3], (int)Math.Round(edges[3].Length));
            Polygon.TrySetVerticalEdge(4, edges[4]);
            Polygon.TrySetHorizontalEdge(5, edges[5]);
            Polygon.TrySetContinuityInVertex(1, vertices[1], Vertex.ContinuityType.G1);
        }

        public static Polygon? GetPolygon()
        {
            return Polygon.Edges.Count > 0 ? Polygon : null;
        }

    }
}
