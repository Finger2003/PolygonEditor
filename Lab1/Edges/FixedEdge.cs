using Lab1.Exceptions;
using Lab1.Visitors;
using System.Numerics;

namespace Lab1.Edges
{
    public class FixedEdge : SpecialEdge
    {
        //public override bool IsBasic { get => false; }
        public override bool IsFixed { get => true; }
        //public int Length { get; set; }
        public double RealSquaredLength { get; private set; }

        public FixedEdge(Vertex start, Vertex end, int length) : base(start, end)
        {
            //End.Position
            //Length = length;
            double deltaX = End.Position.X - Start.Position.X;
            double deltaY = End.Position.Y - Start.Position.Y;
            double lengthRatio = Length / Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            deltaX = deltaX * lengthRatio;
            deltaY = deltaY * lengthRatio;
            End.SetPosition((float)(Start.Position.X + deltaX), (float)(Start.Position.Y + deltaY));
            //End.ResetPreviousPosition();

            RealSquaredLength = deltaX * deltaX + deltaY * deltaY;

            End.WasMoved = true;
            //End.InvokeStartPositionChanged();
            SetButtonPosition();
        }

        public override void StartChanged()
        {
            //SetButtonPosition();

            if (GetSquaredLength() == RealSquaredLength)
                return;

            if (End.WasMoved && Start.WasMoved)
                throw new VertexCannotBeMoved();

            //Start.WasChecked = true;

            //Vector2 positionDifference;
            //positionDifference = Start.PositionDifference;

            End.Move(Start.PositionDifference);
            //End.Position = new Point(End.Position.X + positionDifference.X, End.Position.Y + positionDifference.Y);
            End.WasMoved = true;
            End.InvokeStartPositionChanged();

            //if (!End.WasMoved/*Start.WasMoved*/)
            //{
            //    positionDifference = Start.PositionDifference;
            //    End.Position = new Point(End.Position.X + positionDifference.X, End.Position.Y + positionDifference.Y);
            //    End.WasMoved = true;
            //    End.InvokePositionChanged();
            //}
            //else if (!Start.WasMoved/*End.WasMoved*/)
            //{
            //    positionDifference = End.PositionDifference;
            //    Start.Position = new Point(Start.Position.X + positionDifference.X, Start.Position.Y + positionDifference.Y);
            //    Start.WasMoved = true;
            //    //Start.NeighbourPositionChanged();
            //}

            //End.Position = new Point(End.Position.X + positionDifference.X, End.Position.Y + positionDifference.Y);
            //End.WasMoved = true;
            //End.NeighbourPositionChanged();
        }

        public override void EndChanged()
        {
            if (GetSquaredLength() == RealSquaredLength)
                return;

            if (End.WasMoved && Start.WasMoved)
                throw new VertexCannotBeMoved();

            //Start.WasChecked = true;

            //Size positionDifference;

            //positionDifference = End.PositionDifference;

            Start.Move(End.PositionDifference);
            //End.Position = new Point(End.Position.X + positionDifference.X, End.Position.Y + positionDifference.Y);
            Start.WasMoved = true;
            Start.InvokeEndPositionChanged();
        }


        public override bool CorrectEndPosition()
        {
            if (End.WasMoved && Start.WasMoved)
                throw new VertexCannotBeMoved();

            End.Move(Start.PositionDifference);
            End.WasMoved = true;
            return true;
        }

        public override bool CorrectStartPosition()
        {
            if (End.WasMoved && Start.WasMoved)
                throw new VertexCannotBeMoved();


            Start.Move(End.PositionDifference);
            Start.WasMoved = true;
            return true;
        }


        private float GetSquaredLength()
        {
            //int deltaX = End.Position.X - Start.Position.X;
            //int deltaY = End.Position.Y - Start.Position.Y;
            //return deltaX * deltaX + deltaY * deltaY;
            return Vector2.DistanceSquared(Start.Position, End.Position);
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
