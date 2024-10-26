﻿using Lab1.Exceptions;
using Lab1.Visitors;

namespace Lab1.Edges
{
    public class HorizontalEdge : SpecialEdge
    {
        //public override bool IsBasic { get => false; }
        public override bool IsHorizontal { get => true; }

        public HorizontalEdge(Vertex start, Vertex end) : base(start, end)
        {
            //End.Position = new Point(End.Position.X, Start.Position.Y);
            End.SetPosition(End.X, Start.Y);
            End.WasMoved = true;
            //End.InvokeStartPositionChanged();
            SetButtonPosition();
        }

        public override void StartChanged()
        {
            //SetButtonPosition();
            //Start.WasChecked = true;

            if (Start.Position.Y == End.Position.Y)
            {
                //if (!End.WasChecked)
                //    End.NeighbourPositionChanged();
                return;
            }

            if (End.WasMoved && Start.WasMoved)
                throw new VertexCannotBeMoved();


            //End.Position = new Point(End.Position.X, Start.Position.Y);
            End.SetPosition(End.X, Start.Y);
            End.WasMoved = true;
            End.InvokeStartPositionChanged();

            //if (!End.WasMoved /*Start.WasMoved*/)
            //{
            //    End.Position = new Point(End.Position.X, Start.Position.Y);
            //    End.WasMoved = true;
            //    End.NeighbourPositionChanged();
            //}
            //else if (!Start.WasMoved /*End.WasMoved*/)
            //{
            //    Start.Position = new Point(Start.Position.X, End.Position.Y);
            //    Start.WasMoved = true;
            //    //Start.NeighbourPositionChanged();
            //}

            //End.Position = new Point(End.Position.X, Start.Position.Y);
            //End.WasMoved = true;
            //End.NeighbourPositionChanged();
        }

        public override void EndChanged()
        {
            if (Start.Position.Y == End.Position.Y)
            {
                //if (!End.WasChecked)
                //    End.NeighbourPositionChanged();
                return;
            }

            if (End.WasMoved && Start.WasMoved)
                throw new VertexCannotBeMoved();


            //Start.Position = new Point(Start.Position.X, End.Position.Y);
            Start.SetPosition(Start.X, End.Y);
            Start.WasMoved = true;
            Start.InvokeStartPositionChanged();
        }


        public override bool CorrectEndPosition()
        {
            return correctSecondVertex(Start, End);



            //if ((Start.Contuinity != Vertex.ContuinityType.G0 && Start.ControlAngle != 0 && Start.ControlAngle != Math.PI) || End.WasMoved)
            //{
            //    throw new VertexCannotBeMoved();
            //}

            //if (Start.Contuinity == Vertex.ContuinityType.G0 || Start.Contuinity == Vertex.ContuinityType.G1)
            //{
            //    if (Start.Position.Y == End.Position.Y)
            //    {
            //        return false;
            //    }

            //    End.SetPosition(End.X, Start.Y);
            //    End.WasMoved = true;
            //    return true;
            //}
            //else if (Start.Contuinity == Vertex.ContuinityType.C1)
            //{
            //    double angle = Start.ControlAngle;
            //    double length = Start.ControlLength;

            //    double newX = Start.X + length * Math.Cos(angle);
            //    double newY = Start.Y;

            //    End.SetPosition((float)newX, (float)newY);
            //    End.WasMoved = true;
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            //if (Start.Position.Y == End.Position.Y)
            //{
            //    return false;
            //}

            //if (End.WasMoved && Start.WasMoved)
            //    throw new VertexCannotBeMoved();


            //End.SetPosition(End.X, Start.Y);
            //End.WasMoved = true;
            //return true;
        }

        public override bool CorrectStartPosition()
        {
            return correctSecondVertex(End, Start);


            //if ((End.Contuinity != Vertex.ContuinityType.G0 && End.ControlAngle != 0 && End.ControlAngle != Math.PI) || Start.WasMoved)
            //{
            //    throw new VertexCannotBeMoved();
            //}


            //if (End.Contuinity == Vertex.ContuinityType.G0 || End.Contuinity == Vertex.ContuinityType.G1)
            //{
            //    if (Start.Position.Y == End.Position.Y)
            //    {
            //        return false;
            //    }

            //    End.SetPosition(End.X, Start.Y);
            //    End.WasMoved = true;
            //    return true;
            //}
            //else if(End.Contuinity == Vertex.ContuinityType.C1)
            //{
            //    double angle = End.ControlAngle;
            //    double length = End.ControlLength;

            //    double newX = End.X - length * Math.Cos(angle);
            //    double newY = End.Y;

            //    Start.SetPosition((float)newX, (float)newY);
            //    Start.WasMoved = true;
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}



            //if (Start.Position.Y == End.Position.Y)
            //{
            //    return false;
            //}

            //if (End.WasMoved && Start.WasMoved)
            //    throw new VertexCannotBeMoved();


            //Start.SetPosition(Start.X, End.Y);
            //Start.WasMoved = true;
            //return true;
        }
        private bool correctSecondVertex(Vertex firstVertex, Vertex secondVertex)
        {
            if ((firstVertex.Continuity != Vertex.ContuinityType.G0 && firstVertex.ControlAngle != 0 && firstVertex.ControlAngle != Math.PI && !firstVertex.ContinuityChanged) || secondVertex.WasMoved)
            {
                throw new VertexCannotBeMoved();
            }

            if (firstVertex.Continuity == Vertex.ContuinityType.G0)
            {
                if (firstVertex.Position.Y == secondVertex.Position.Y)
                {
                    secondVertex.ControlLength = GetControlLength(Start, End);
                    return secondVertex.Continuity == Vertex.ContuinityType.C1;
                }

                secondVertex.SetPosition(secondVertex.X, firstVertex.Y);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return true;
            }
            else if (firstVertex.Continuity == Vertex.ContuinityType.G1)
            {
                if(secondVertex.X > firstVertex.X == (firstVertex.ControlAngle == 0) && firstVertex.Y == secondVertex.Y)
                {
                    secondVertex.ControlAngle = GetControlAngle(Start, End);
                    secondVertex.ControlLength = GetControlLength(Start, End);
                    return secondVertex.Continuity != Vertex.ContuinityType.G0;
                }

                float newX = firstVertex.X + (secondVertex.X > firstVertex.X ? 1 : -1) * Length;

                secondVertex.SetPosition(newX, firstVertex.Y);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return true;
            }
            else if (firstVertex.Continuity == Vertex.ContuinityType.C1)
            {
                double angle = firstVertex.ControlAngle;
                double length = firstVertex.ControlLength * 3;

                double newX = firstVertex.X + length * (angle == 0 ? 1 : -1);
                double newY = firstVertex.Y;

                secondVertex.SetPosition((float)newX, (float)newY);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return true;
            }
            else
            {
                return false;
            }            
        }
        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
