using Lab1.Exceptions;
using Lab1.Visitors;

namespace Lab1.Edges
{
    public class VerticalEdge : SpecialEdge
    {
        //public override bool IsBasic { get => false; }
        public override bool IsVertical { get => true; }

        public VerticalEdge(Vertex start, Vertex end) : base(start, end)
        {
            End.SetPosition(Start.X, End.Y);
            End.WasMoved = true;
            //End.InvokeStartPositionChanged();
            SetButtonPosition();
        }

        public override void StartChanged()
        {
            //SetButtonPosition();
            //Start.WasChecked = true;

            if (Start.Position.X == End.Position.X)
            {
                //if (!End.WasChecked)
                //    End.NeighbourPositionChanged();
                return;
            }
            if (End.WasMoved && Start.WasMoved)
                throw new VertexCannotBeMoved();


            //End.Position = new Point(Start.Position.X, End.Position.Y);
            End.SetPosition(Start.X, End.Y);
            End.WasMoved = true;
            End.InvokeStartPositionChanged();


            //if (!End.WasMoved /*Start.WasMoved*/)
            //{
            //    End.Position = new Point(Start.Position.X, End.Position.Y);
            //    End.WasMoved = true;
            //    End.NeighbourPositionChanged();
            //}
            //else if (!Start.WasMoved/*End.WasMoved*/)
            //{
            //    Start.Position = new Point(End.Position.X, Start.Position.Y);
            //    Start.WasMoved = true;
            //    //Start.NeighbourPositionChanged();
            //}

            //End.Position = new Point(Start.Position.X, End.Position.Y);
            //End.WasMoved = true;
            //End.NeighbourPositionChanged();
        }

        public override void EndChanged()
        {
            if (Start.Position.X == End.Position.X)
            {
                //if (!End.WasChecked)
                //    End.NeighbourPositionChanged();
                return;
            }
            if (End.WasMoved && Start.WasMoved)
                throw new VertexCannotBeMoved();


            //Start.Position = new Point(End.Position.X, Start.Position.Y);
            Start.SetPosition(End.X, Start.Y);
            Start.WasMoved = true;
            Start.InvokeEndPositionChanged();
        }


        public override bool CorrectEndPosition()
        {
            if (Start.Position.X == End.Position.X)
            {
                return false;
            }
            if (End.WasMoved && Start.WasMoved)
                throw new VertexCannotBeMoved();


            End.SetPosition(Start.X, End.Y);
            End.WasMoved = true;
            return true;
        }

        public override bool CorrectStartPosition()
        {
            if (Start.Position.X == End.Position.X)
            {
                return false;
            }
            if (End.WasMoved && Start.WasMoved)
                throw new VertexCannotBeMoved();


            Start.SetPosition(End.X, Start.Y);
            Start.WasMoved = true;
            return true;
        }


        private bool CorrectSecondVertex(Vertex firstVertex, Vertex secondVertex)
        {
            if ((firstVertex.Continuity != Vertex.ContuinityType.G0 && firstVertex.ControlAngle != Math.PI/2 && firstVertex.ControlAngle != 3*Math.PI/2 && !firstVertex.ContinuityChanged) || secondVertex.WasMoved)
            {
                throw new VertexCannotBeMoved();
            }

            if (firstVertex.Continuity == Vertex.ContuinityType.G0)
            {
                if (firstVertex.X == secondVertex.X)
                {
                    secondVertex.ControlLength = GetControlLength(Start, End);
                    return secondVertex.Continuity == Vertex.ContuinityType.C1;
                }

                secondVertex.SetPosition(firstVertex.X, secondVertex.Y);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return true;
            }
            else if(firstVertex.Continuity == Vertex.ContuinityType.G1)
            {
                if (secondVertex.Y > firstVertex.Y == (firstVertex.ControlAngle == Math.PI/2) && firstVertex.Y == secondVertex.Y)
                {
                    secondVertex.ControlAngle = GetControlAngle(Start, End);
                    secondVertex.ControlLength = GetControlLength(Start, End);
                    return secondVertex.Continuity != Vertex.ContuinityType.G0;
                }

                float newY = firstVertex.Y + (secondVertex.Y > firstVertex.Y ? 1 : -1) * Length;

                secondVertex.SetPosition(firstVertex.X, newY);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return true;
            }
            else if (firstVertex.Continuity == Vertex.ContuinityType.C1)
            {
                double angle = firstVertex.ControlAngle;
                double length = firstVertex.ControlLength * 3;

                double newX = firstVertex.X;
                double newY = firstVertex.Y + length * (angle == Math.PI / 2 ? 1 : -1);

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
            //Start.WasMoved = true;
            //return true;
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
