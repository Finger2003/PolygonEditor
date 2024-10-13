namespace Lab1
{
    public class BezierEdge : Edge
    {
        public override bool IsBasic { get => false; }
        public override bool IsBezier { get => true; }

        public BezierEdge(Vertex start, Vertex end) : base(start, end) { }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
