using Lab1.Visitors;

namespace Lab1.Edges
{
    public class Edge : IEdgeVisitable
    {
        public virtual Vertex Start { get; set; }
        public virtual Vertex End { get; set; }

        public float Length => Vertex.Distance(Start, End);
        protected virtual double GetControlAngle(Vertex v1, Vertex v2)
        {
            return Math.Atan2(End.Y - Start.Y, End.X - Start.X);
        }
        protected virtual double GetControlLength(Vertex v1, Vertex v2)
        {
            return Length / 3;
        }

        public virtual void StartChanged()
        {
            //if(Start.WasMoved)
            //    throw new VertexAlreadyMovedException();

            //Start.WasMoved = true;

            //Start.WasChecked = true;

            //if (!End.WasChecked)
            //    End.NeighbourPositionChanged();
        }
        public virtual void EndChanged() { }

        public virtual bool IsControlVertex(Vertex v)
        {
            return v == Start || v == End;
        }
        public virtual void SetVerticesContinuityRelevantProperties(Vertex v)
        {
            v.ControlAngle = GetControlAngle(Start, End);
            v.ControlLength = GetControlLength(Start, End);
        }


        public virtual bool CorrectEndPosition()
        {
            double angle = Start.ControlAngle;
            return CorrectSecondVertex(Start, End, angle);



            //float newX, newY;

            //if (Start.Contuinity == Vertex.ContuinityType.G0)
            //{
            //    End.ControlAngle = GetControlAngle(Start, End);
            //    End.ControlLength = GetControlLength(Start, End);
            //    return End.Contuinity != Vertex.ContuinityType.G0;
            //}

            //double length = Length;
            //double angle = Start.ControlAngle;
            //if(Start.Contuinity == Vertex.ContuinityType.C1)
            //{
            //    length = Start.ControlLength * 3;
            //}

            //newX = (float)(Start.X + length * Math.Cos(angle));
            //newY = (float)(Start.Y + length * Math.Sin(angle));

            //End.SetPosition(newX, newY);
            //End.WasMoved = true;
            //End.ControlAngle = GetControlAngle(Start, End);
            //End.ControlLength = GetControlLength(Start, End);
            //return true;




            //if (Start.Contuinity == Vertex.ContuinityType.G1)
            //{
            //    newX = (float)(Start.X + Length * Math.Cos(Start.ControlAngle));
            //    newY = (float)(Start.Y + Length * Math.Sin(Start.ControlAngle));
            //}
            //else if (Start.Contuinity == Vertex.ContuinityType.C1)
            //{
            //    newX = (float)(Start.X + Start.ControlLength * Math.Cos(Start.ControlAngle));
            //    newY = (float)(Start.Y + Start.ControlLength * Math.Sin(Start.ControlAngle));
            //}
            //else
            //{
            //    return false;
            //}

            //End.SetPosition(newX, newY);
            //End.WasMoved = true;
            //return true;
        }

        public virtual bool CorrectStartPosition()
        {
            double angle = End.ControlAngle + Math.PI;
            return CorrectSecondVertex(End, Start, angle);



            //double newX, newY;

            //if (End.Contuinity == Vertex.ContuinityType.G1)
            //{
            //    newX = End.X - Length * Math.Cos(End.ControlAngle);
            //    newY = End.Y - Length * Math.Sin(End.ControlAngle);
            //}
            //else if (End.Contuinity == Vertex.ContuinityType.C1)
            //{
            //    newX = End.X - End.ControlLength * Math.Cos(End.ControlAngle);
            //    newY = End.Y - End.ControlLength * Math.Sin(End.ControlAngle);
            //}
            //else
            //{
            //    return false;
            //}

            //Start.SetPosition((float)newX, (float)newY);
            //Start.WasMoved = true;
            //return true;
        }

        public virtual void CorrectStartPositionBasically()
        {
            Start.ControlAngle = GetControlAngle(Start, End);
            Start.ControlLength = GetControlLength(Start, End);
        }

        public virtual void CorrectEndPositionBasically()
        {
            End.ControlAngle = GetControlAngle(Start, End);
            End.ControlLength = GetControlLength(Start, End);
        }

        private bool CorrectSecondVertex(Vertex firstVertex, Vertex secondVertex, double angle)
        {
            float newX, newY;

            if (firstVertex.Continuity == Vertex.ContuinityType.G0)
            {
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                return secondVertex.Continuity != Vertex.ContuinityType.G0;
            }

            double length = Length;
            //double angle = firstVertex.ControlAngle;
            if (firstVertex.Continuity == Vertex.ContuinityType.C1)
            {
                length = firstVertex.ControlLength * 3;
            }

            newX = (float)(firstVertex.X + length * Math.Cos(angle));
            newY = (float)(firstVertex.Y + length * Math.Sin(angle));

            secondVertex.SetPosition(newX, newY);
            secondVertex.WasMoved = true;
            secondVertex.ControlAngle = GetControlAngle(Start, End);
            secondVertex.ControlLength = GetControlLength(Start, End);
            return true;
        }

        public Edge(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
            //SubscribeVertices();
        }


        public virtual bool IsBasic { get => true; }
        public virtual bool IsHorizontal { get => false; }
        public virtual bool IsVertical { get => false; }
        public virtual bool IsFixed { get => false; }
        public virtual bool IsBezier { get => false; }


        public bool IsHit(Point p)
        {
            double distance = DistanceToPoint(p);
            return distance < 5;
        }

        protected virtual double DistanceToPoint(Point p)
        {
            var x0 = p.X;
            var y0 = p.Y;
            var x1 = Start.Position.X;
            var y1 = Start.Position.Y;
            var x2 = End.Position.X;
            var y2 = End.Position.Y;

            double numerator = Math.Abs((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1);
            double denominator = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));

            return numerator / denominator;
        }

        public void UnsubscribeVertices()
        {
            Start.StartChanged -= StartChanged;
            End.EndChanged -= EndChanged;
        }
        public void SubscribeVertices()
        {
            Start.StartChanged += StartChanged;
            End.EndChanged += EndChanged;
        }
        public virtual void Accept(IEdgeVisitor visitor) => visitor.Visit(this);

        public virtual void Restore()
        {
            Start.Restore();
        }

        //public virtual void OnMoved() { }
        public virtual void MoveOwnedVertices(float dx, float dy)
        {
            Start.Move(dx, dy);
        }
        public virtual bool TryGetHitOwnedVertex(Point p, out Vertex? vertex)
        {
            vertex = null;

            if (Start.IsHit(p))
                vertex = Start;

            return vertex is not null;
        }
        public virtual void ResetOwnedVerticesMovementFlags()
        {
            Start.WasMoved = false;
            Start.ContinuityPropertiesChanged = false;
        }
        public virtual void ResetOwnedMovedVerticesPreviousPositions()
        {
            //if (Start.WasMoved)
                Start.ResetPreviousPosition();
        }
    }
}
