using PolygonEditor.Visitors.CorrectionStatusVisitors;
using PolygonEditor.Visitors.VoidVisitors;

namespace PolygonEditor.GeometryModel.Edges
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
