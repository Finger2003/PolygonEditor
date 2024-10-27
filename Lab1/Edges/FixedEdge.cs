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
        private int SetLength { get; }

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

        public override void CorrectStartPositionBasically()
        {
            Start.Move(End.PositionDifference);
            Start.WasMoved = true;
            Start.ControlAngle = GetControlAngle(Start, End);
            Start.ControlLength = GetControlLength(Start, End);
        }
        public override void CorrectEndPositionBasically()
        {
            End.Move(Start.PositionDifference);
            End.WasMoved = true;
            End.ControlAngle = GetControlAngle(Start, End);
            End.ControlLength = GetControlLength(Start, End);
        }

       
        public override correctingStatus CorrectEndPosition()
        {
            double angle = Start.ControlAngle;
            return CorrectSecondVertex(Start, End, angle);
        }

        public override correctingStatus CorrectStartPosition()
        {
            double angle = End.ControlAngle + Math.PI;
            return CorrectSecondVertex(End, Start, angle);
        }

        private correctingStatus CorrectSecondVertex(Vertex firstVertex, Vertex secondVertex, double angle)
        {
            if ((firstVertex.Continuity == Vertex.ContuinityType.C1 && !firstVertex.WasMoved && !firstVertex.ContinuityChanged && firstVertex.ContinuityPropertiesChanged) || secondVertex.WasMoved)
            {
                return correctingStatus.CorrectionFailed;
            }

            if (firstVertex.WasMoved)
            {
                secondVertex.Move(firstVertex.PositionDifference);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return correctingStatus.FurtherCorrectionNeeded;
            }
            else if (firstVertex.Continuity == Vertex.ContuinityType.G1)
            {
                double newX = firstVertex.X + SetLength * Math.Cos(angle);
                double newY = firstVertex.Y + SetLength * Math.Sin(angle);

                secondVertex.SetPosition((float)newX, (float)newY);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return correctingStatus.FurtherCorrectionNeeded;
            }
            return correctingStatus.FurtherCorrectionNotNeeded;
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
