using Lab1.Exceptions;
using Lab1.Visitors.CorrectionStatusVisitors;
using Lab1.Visitors.VoidVisitors;

namespace Lab1.GeometryModel.Edges
{
    public class VerticalEdge : SpecialEdge
    {
        public override bool IsVertical { get => true; }

        public VerticalEdge(Vertex start, Vertex end) : base(start, end)
        {
            End.SetPosition(Start.X, End.Y);
            End.WasMoved = true;
        }


        public override void Accept(IEdgeVoidVisitor visitor) => visitor.Visit(this);
        public override CorrectionStatus Accept(IEdgeCorrectionStatusVisitor visitor) => visitor.Visit(this);
    }
}
