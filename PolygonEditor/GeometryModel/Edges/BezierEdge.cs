using PolygonEditor.Visitors.CorrectionStatusVisitors;
using PolygonEditor.Visitors.VoidVisitors;

namespace PolygonEditor.GeometryModel.Edges
{
    public class BezierEdge : SpecialEdge
    {
        public override bool IsBezier { get => true; }
        public Vertex V1 { get; set; }
        public Vertex V2 { get; set; }

        public BezierEdge(Vertex start, Vertex end) : base(start, end)
        {
            V1 = new Vertex((Start.X + End.X) / 2, Start.Y, true);
            V2 = new Vertex((Start.X + End.X) / 2, End.Y, true);
        }


        public override bool TryGetHitOwnedVertex(Point p, out Vertex? vertex)
        {
            vertex = null;

            if (Start.IsHit(p))
                vertex = Start;
            else if (V1.IsHit(p))
                vertex = V1;
            else if (V2.IsHit(p))
                vertex = V2;

            return vertex is not null;
        }

        public override bool TryGetHitOwnedVertex(float x, float y, out Vertex? vertex)
        {
            vertex = null;

            if (Start.IsHit(x, y))
                vertex = Start;
            else if (V1.IsHit(x, y))
                vertex = V1;
            else if (V2.IsHit(x, y))
                vertex = V2;

            return vertex is not null;
        }


        public override bool IsControlVertex(Vertex v)
        {
            return v == V1 || v == V2;
        }

        protected override double DistanceToHitLine(Point p)
        {
            double distance = DistanceBetweenLineAndPoint(Start.X, Start.Y, V1.X, V1.Y, p);
            double distance2 = DistanceBetweenLineAndPoint(V1.X, V1.Y, V2.X, V2.Y, p);
            double distance3 = DistanceBetweenLineAndPoint(V2.X, V2.Y, End.X, End.Y, p);
            return Math.Min(distance, Math.Min(distance2, distance3));
        }

        public override void Accept(IEdgeVoidVisitor visitor) => visitor.Visit(this);
        public override CorrectionStatus Accept(IEdgeCorrectionStatusVisitor visitor) => visitor.Visit(this);
    }
}
