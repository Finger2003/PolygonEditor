using Lab1.Visitors.CorrectionStatusVisitors;
using Lab1.Visitors.VoidVisitors;

namespace Lab1.GeometryModel.Edges
{
    public class FixedEdge : SpecialEdge
    {
        public override bool IsFixed { get => true; }
        public int SetLength { get; }

        public FixedEdge(Vertex start, Vertex end, int length) : base(start, end)
        {
            SetLength = length;
            double deltaX = End.Position.X - Start.Position.X;
            double deltaY = End.Position.Y - Start.Position.Y;
            if (Length != length)
            {
                double lengthRatio = length / Length;
                deltaX *= lengthRatio;
                deltaY *= lengthRatio;
                End.SetPosition((float)(Start.Position.X + deltaX), (float)(Start.Position.Y + deltaY));
                End.WasMoved = true;
            }
        }

        public override void Accept(IEdgeVoidVisitor visitor) => visitor.Visit(this);
        public override CorrectionStatus Accept(IEdgeCorrectionStatusVisitor visitor) => visitor.Visit(this);
    }
}
