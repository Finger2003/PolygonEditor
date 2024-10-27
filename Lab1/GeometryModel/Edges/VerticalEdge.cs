using Lab1.Exceptions;
using Lab1.Visitors;

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



        public override correctingStatus CorrectEndPosition()
        {
            return CorrectSecondVertex(Start, End);
        }

        public override correctingStatus CorrectStartPosition()
        {
            return CorrectSecondVertex(End, Start);
        }


        private correctingStatus CorrectSecondVertex(Vertex firstVertex, Vertex secondVertex)
        {
            if (firstVertex.Continuity != Vertex.ContuinityType.G0 && firstVertex.ControlAngle != Math.PI / 2 && firstVertex.ControlAngle != -Math.PI / 2 && !firstVertex.ContinuityChanged || secondVertex.WasMoved)
            {
                return correctingStatus.CorrectionFailed;
            }

            if (firstVertex.Continuity == Vertex.ContuinityType.G0)
            {
                if (firstVertex.X == secondVertex.X)
                {
                    secondVertex.ControlLength = GetControlLength(Start, End);
                    if (secondVertex.Continuity == Vertex.ContuinityType.C1)
                    {
                        return correctingStatus.FurtherCorrectionNeeded;
                    }
                    return correctingStatus.FurtherCorrectionNotNeeded;
                }

                secondVertex.SetPosition(firstVertex.X, secondVertex.Y);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return correctingStatus.FurtherCorrectionNeeded;
            }
            else if (firstVertex.Continuity == Vertex.ContuinityType.G1)
            {
                if (secondVertex.Y > firstVertex.Y == (firstVertex.ControlAngle == Math.PI / 2) && firstVertex.Y == secondVertex.Y)
                {
                    secondVertex.ControlAngle = GetControlAngle(Start, End);
                    secondVertex.ControlLength = GetControlLength(Start, End);
                    if (secondVertex.Continuity == Vertex.ContuinityType.G0)
                    {
                        return correctingStatus.FurtherCorrectionNotNeeded;
                    }
                    return correctingStatus.FurtherCorrectionNeeded;
                }

                float newY = firstVertex.Y + Length * (secondVertex.Y > firstVertex.Y ? 1 : -1);

                secondVertex.SetPosition(firstVertex.X, newY);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return correctingStatus.FurtherCorrectionNeeded;
            }
            else if (firstVertex.Continuity == Vertex.ContuinityType.C1)
            {
                double angle = firstVertex.ControlAngle;
                double length = firstVertex.ControlLength * 3;

                double newX = firstVertex.X;
                double newY = firstVertex.Y + length * (secondVertex.Y > firstVertex.Y ? 1 : -1);

                secondVertex.SetPosition((float)newX, (float)newY);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return correctingStatus.FurtherCorrectionNeeded;
            }
            else
            {
                return correctingStatus.FurtherCorrectionNotNeeded;
            }
        }

        public override void CorrectStartPositionBasically()
        {
            if (Start.X == End.X)
            {
                Start.WasMoved = false;
            }
            else
            {
                Start.SetPosition(End.X, Start.Y);
                Start.WasMoved = true;
            }

            Start.ControlAngle = GetControlAngle(Start, End);
            Start.ControlLength = GetControlLength(Start, End);
        }
        public override void CorrectEndPositionBasically()
        {
            if (Start.X == End.X)
            {
                End.WasMoved = false;
            }
            else
            {
                End.SetPosition(Start.X, End.Y);
            }

            End.ControlAngle = GetControlAngle(Start, End);
            End.ControlLength = GetControlLength(Start, End);
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
