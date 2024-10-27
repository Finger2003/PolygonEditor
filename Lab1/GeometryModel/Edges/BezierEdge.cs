using Lab1.Visitors.CorrectionStatusVisitors;
using Lab1.Visitors.VoidVisitors;

namespace Lab1.GeometryModel.Edges
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
                
        public override void Accept(IEdgeVoidVisitor visitor) => visitor.Visit(this);
        public override CorrectionStatus Accept(IEdgeCorrectionStatusVisitor visitor) => visitor.Visit(this);
    }
}
