using PolygonEditor.Visitors.CorrectionStatusVisitors;
using PolygonEditor.Visitors.VoidVisitors;

namespace PolygonEditor.GeometryModel.Edges
{
    public class HorizontalEdge : SpecialEdge
    {
        //public override bool IsBasic { get => false; }
        public override bool IsHorizontal { get => true; }

        public HorizontalEdge(Vertex start, Vertex end) : base(start, end)
        {
            End.SetPosition(End.X, Start.Y);
            End.WasMoved = true;
        }

        public override void Accept(IEdgeVoidVisitor visitor) => visitor.Visit(this);
        public override CorrectionStatus Accept(IEdgeCorrectionStatusVisitor visitor) => visitor.Visit(this);
    }
}
