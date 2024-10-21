using Lab1.Visitors;

namespace Lab1.Edges
{
    public class BezierEdge : SpecialEdge
    {
        //public override bool IsBasic { get => false; }
        public override bool IsBezier { get => true; }
        public Vertex V1 { get; set; }
        public Vertex V2 { get; set; }

        public BezierEdge(Vertex start, Vertex end) : base(start, end)
        {
            V1 = new Vertex((Start.Position.X + End.Position.X) / 2, Start.Position.Y);
            V2 = new Vertex((Start.Position.X + End.Position.X) / 2, End.Position.Y);
        }

        public override void MoveOwnedVertices(int dx, int dy)
        {
            base.MoveOwnedVertices(dx, dy);
            V1.Position = new Point(V1.Position.X + dx, V1.Position.Y + dy);
            V2.Position = new Point(V2.Position.X + dx, V2.Position.Y + dy);
        }

        public override bool TryGetHitOwnedVertex(Point p, out Vertex? vertex)
        {
            vertex = null;

            if(Start.IsHit(p))
                vertex = Start;
            else if(V1.IsHit(p))
                vertex = V1;
            else if (V2.IsHit(p))
                vertex = V2;

            return vertex is not null;
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
