using Lab1.Exceptions;
using Lab1.Visitors;

namespace Lab1.Edges
{
    public class FixedEdge : SpecialEdge
    {
        //public override bool IsBasic { get => false; }
        public override bool IsFixed { get => true; }
        public int Length { get; set; }

        public FixedEdge(Vertex start, Vertex end, int length) : base(start, end)
        {
            //End.Position
            Length = length;
            int deltaX = End.Position.X - Start.Position.X;
            int deltaY = End.Position.Y - Start.Position.Y;
            double lengthRatio = Length / Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            deltaX = (int)(deltaX * lengthRatio);
            deltaY = (int)(deltaY * lengthRatio);
            End.Position = new Point(Start.Position.X + deltaX, Start.Position.Y + deltaY);
            End.ResetPreviousPosition();

            End.WasMoved = true;
            End.NeighbourPositionChanged();
            SetButtonPosition();
        }

        public override void StartChanged()
        {
            //SetButtonPosition();

            if (End.WasMoved && Start.WasMoved)
                throw new VertexAlreadyMovedException();

            Start.WasChecked = true;
            Point positionDifference;

            if (!End.WasMoved/*Start.WasMoved*/)
            {
                positionDifference = Start.PositionDifference;
                End.Position = new Point(End.Position.X + positionDifference.X, End.Position.Y + positionDifference.Y);
                End.WasMoved = true;
                End.NeighbourPositionChanged();
            }
            else if (!Start.WasMoved/*End.WasMoved*/)
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
