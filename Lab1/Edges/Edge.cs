using Lab1.Visitors;

namespace Lab1.Edges
{
    public class Edge : IEdgeVisitable
    {
        public virtual Vertex Start { get; set; }
        public virtual Vertex End { get; set; }

        public float Length => Vertex.Distance(Start, End);
        public virtual double GetControlAngle(Vertex v)
        { 
            return Math.Atan2(End.Y - Start.Y, End.X - Start.X);
        }
        public virtual double GetControlLength(Vertex v)
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


        public virtual bool CorrectEndPosition()
        {
            return false;
        }

        public virtual bool CorrectStartPosition()
        {
            return false;
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

        public virtual void OnMoved() { }
        public virtual void MoveOwnedVertices(int dx, int dy)
        {
            Start.Move(dx, dy);
        }
        public virtual bool TryGetHitOwnedVertex(Point p, out Vertex? vertex)
        {
            vertex = null;

            if(Start.IsHit(p))
                vertex = Start;

            return vertex is not null;
        }
    }
}
