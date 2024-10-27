using Lab1.Exceptions;
using Lab1.Visitors.CorrectionStatusVisitors;
using Lab1.Visitors.VoidVisitors;

namespace Lab1.GeometryModel.Edges
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


        public override CorrectionStatus CorrectEndPosition()
        {
            return correctSecondVertex(Start, End);
        }

        public override CorrectionStatus CorrectStartPosition()
        {
            return correctSecondVertex(End, Start);
        }

        private CorrectionStatus correctSecondVertex(Vertex firstVertex, Vertex secondVertex)
        {
            if (firstVertex.Continuity != Vertex.ContuinityType.G0 && firstVertex.ControlAngle != 0 && firstVertex.ControlAngle != Math.PI && !firstVertex.ContinuityChanged || secondVertex.WasMoved)
            {
                return CorrectionStatus.CorrectionFailed;
            }

            if (firstVertex.Continuity == Vertex.ContuinityType.G0)
            {
                if (firstVertex.Position.Y == secondVertex.Position.Y)
                {
                    secondVertex.ControlLength = GetControlLength(Start, End);
                    if (secondVertex.Continuity == Vertex.ContuinityType.C1)
                    {
                        return CorrectionStatus.FurtherCorrectionNeeded;
                    }
                    return CorrectionStatus.FurtherCorrectionNotNeeded;
                }

                secondVertex.SetPosition(secondVertex.X, firstVertex.Y);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return CorrectionStatus.FurtherCorrectionNeeded;
            }
            else if (firstVertex.Continuity == Vertex.ContuinityType.G1)
            {
                if (secondVertex.X > firstVertex.X == (firstVertex.ControlAngle == 0) && firstVertex.Y == secondVertex.Y)
                {
                    secondVertex.ControlAngle = GetControlAngle(Start, End);
                    secondVertex.ControlLength = GetControlLength(Start, End);
                    if (secondVertex.Continuity == Vertex.ContuinityType.G0)
                    {
                        return CorrectionStatus.FurtherCorrectionNotNeeded;
                    }
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }

                float newX = firstVertex.X + Length * (secondVertex.X > firstVertex.X ? 1 : -1);

                secondVertex.SetPosition(newX, firstVertex.Y);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return CorrectionStatus.FurtherCorrectionNeeded;
            }
            else if (firstVertex.Continuity == Vertex.ContuinityType.C1)
            {
                double angle = firstVertex.ControlAngle;
                double length = firstVertex.ControlLength * 3;

                double newX = firstVertex.X + (secondVertex.X > firstVertex.X ? 1 : -1) * length;
                double newY = firstVertex.Y;

                secondVertex.SetPosition((float)newX, (float)newY);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return CorrectionStatus.FurtherCorrectionNeeded;
            }
            else
            {
                return CorrectionStatus.FurtherCorrectionNotNeeded;
            }
        }

        //public override void CorrectStartPositionBasically()
        //{
        //    if (Start.Y == End.Y)
        //    {
        //        Start.WasMoved = false;
        //    }
        //    else
        //    {
        //        Start.SetPosition(Start.X, End.Y);
        //        Start.WasMoved = true;
        //    }

        //    Start.ControlAngle = GetControlAngle(Start, End);
        //    Start.ControlLength = GetControlLength(Start, End);
        //}

        //public override void CorrectEndPositionBasically()
        //{
        //    if (Start.Y == End.Y)
        //    {
        //        End.WasMoved = false;
        //    }
        //    else
        //    {
        //        End.SetPosition(End.X, Start.Y);
        //        End.WasMoved = true;
        //    }

        //    End.ControlAngle = GetControlAngle(Start, End);
        //    End.ControlLength = GetControlLength(Start, End);
        //}
        public override void Accept(IEdgeVoidVisitor visitor) => visitor.Visit(this);
        public override CorrectionStatus Accept(IEdgeCorrectionStatusVisitor visitor) => visitor.Visit(this);
    }
}
