namespace Lab1
{
    public class FixedEdge : Edge
    {
        public override bool IsBasic { get => false; }
        public override bool IsFixed { get => true; }
        public int Length { get; set; }

        public FixedEdge(Vertex start, Vertex end) : base(start, end)
        {
            //End.Position
        }

        public override void StartChanged()
        {
            //if (End.WasMoved && Start.WasMoved)
            //    throw new VertexAlreadyMovedException();

            Start.WasChecked = true;
            Point positionDifference;

            if (/*!End.WasMoved*/Start.WasMoved)
            {
                positionDifference = Start.PositionDifference;
                End.Position = new Point(End.Position.X + positionDifference.X, End.Position.Y + positionDifference.Y);
                End.WasMoved = true;
                End.NeighbourPositionChanged();
            }
            else if (/*!Start.WasMoved*/End.WasMoved)
            {
                positionDifference = End.PositionDifference;
                Start.Position = new Point(Start.Position.X + positionDifference.X, Start.Position.Y + positionDifference.Y);
                Start.WasMoved = true;
                //Start.NeighbourPositionChanged();
            }
            //End.Position = new Point(End.Position.X + positionDifference.X, End.Position.Y + positionDifference.Y);
            //End.WasMoved = true;
            //End.NeighbourPositionChanged();
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
