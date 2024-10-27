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



        public override CorrectionStatus CorrectEndPosition()
        {
            return CorrectSecondVertex(Start, End);
        }

        public override CorrectionStatus CorrectStartPosition()
        {
            return CorrectSecondVertex(End, Start);
        }


        private CorrectionStatus CorrectSecondVertex(Vertex firstVertex, Vertex secondVertex)
        {
            if (firstVertex.Continuity != Vertex.ContuinityType.G0 && firstVertex.ControlAngle != Math.PI / 2 && firstVertex.ControlAngle != -Math.PI / 2 && !firstVertex.ContinuityChanged || secondVertex.WasMoved)
            {
                return CorrectionStatus.CorrectionFailed;
            }

            if (firstVertex.Continuity == Vertex.ContuinityType.G0)
            {
                if (firstVertex.X == secondVertex.X)
                {
                    secondVertex.ControlLength = GetControlLength(Start, End);
                    if (secondVertex.Continuity == Vertex.ContuinityType.C1)
                    {
                        return CorrectionStatus.FurtherCorrectionNeeded;
                    }
                    return CorrectionStatus.FurtherCorrectionNotNeeded;
                }

                secondVertex.SetPosition(firstVertex.X, secondVertex.Y);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return CorrectionStatus.FurtherCorrectionNeeded;
            }
            else if (firstVertex.Continuity == Vertex.ContuinityType.G1)
            {
                if (secondVertex.Y > firstVertex.Y == (firstVertex.ControlAngle == Math.PI / 2) && firstVertex.Y == secondVertex.Y)
                {
                    secondVertex.ControlAngle = GetControlAngle(Start, End);
                    secondVertex.ControlLength = GetControlLength(Start, End);
                    if (secondVertex.Continuity == Vertex.ContuinityType.G0)
                    {
                        return CorrectionStatus.FurtherCorrectionNotNeeded;
                    }
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }

                float newY = firstVertex.Y + Length * (secondVertex.Y > firstVertex.Y ? 1 : -1);

                secondVertex.SetPosition(firstVertex.X, newY);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return CorrectionStatus.FurtherCorrectionNeeded;
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
                return CorrectionStatus.FurtherCorrectionNeeded;
            }
            else
            {
                return CorrectionStatus.FurtherCorrectionNotNeeded;
            }
        }

        //public override void CorrectStartPositionBasically()
        //{
        //    if (Start.X == End.X)
        //    {
        //        Start.WasMoved = false;
        //    }
        //    else
        //    {
        //        Start.SetPosition(End.X, Start.Y);
        //        Start.WasMoved = true;
        //    }

        //    Start.ControlAngle = GetControlAngle(Start, End);
        //    Start.ControlLength = GetControlLength(Start, End);
        //}
        //public override void CorrectEndPositionBasically()
        //{
        //    if (Start.X == End.X)
        //    {
        //        End.WasMoved = false;
        //    }
        //    else
        //    {
        //        End.SetPosition(Start.X, End.Y);
        //    }

        //    End.ControlAngle = GetControlAngle(Start, End);
        //    End.ControlLength = GetControlLength(Start, End);
        //}

        public override void Accept(IEdgeVoidVisitor visitor) => visitor.Visit(this);
        public override CorrectionStatus Accept(IEdgeCorrectionStatusVisitor visitor) => base.Accept(visitor);
    }
}
