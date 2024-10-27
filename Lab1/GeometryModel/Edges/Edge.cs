using Lab1.Visitors;

namespace Lab1.GeometryModel.Edges
{
    public class Edge : IEdgeVisitable
    {
        public enum correctingStatus
        {
            FurtherCorrectionNeeded,
            FurtherCorrectionNotNeeded,
            CorrectionFailed
        }

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


        public virtual bool IsControlVertex(Vertex v)
        {
            return v == Start || v == End;
        }
        //public virtual void SetVerticesContinuityRelevantProperties(Vertex v)
        //{
        //    v.ControlAngle = GetControlAngle(Start, End);
        //    v.ControlLength = GetControlLength(Start, End);
        //}


        public virtual correctingStatus CorrectEndPosition()
        {
            double angle = Start.ControlAngle;
            return CorrectSecondVertex(Start, End, angle);
        }

        public virtual correctingStatus CorrectStartPosition()
        {
            double angle = End.ControlAngle + Math.PI;
            return CorrectSecondVertex(End, Start, angle);
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

        private correctingStatus CorrectSecondVertex(Vertex firstVertex, Vertex secondVertex, double angle)
        {
            float newX, newY;

            if (firstVertex.Continuity == Vertex.ContuinityType.G0)
            {
                secondVertex.ControlAngle = GetControlAngle(Start, End);
                secondVertex.ControlLength = GetControlLength(Start, End);
                if (secondVertex.Continuity == Vertex.ContuinityType.G0)
                {
                    return correctingStatus.FurtherCorrectionNotNeeded;
                }
                return correctingStatus.FurtherCorrectionNeeded;
            }

            double length = Length;
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
            return correctingStatus.FurtherCorrectionNeeded;
        }

        public Edge(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
        }


        public virtual bool IsBasic { get => true; }
        public virtual bool IsHorizontal { get => false; }
        public virtual bool IsVertical { get => false; }
        public virtual bool IsFixed { get => false; }
        public virtual bool IsBezier { get => false; }


        public virtual bool IsHit(Point p)
        {
            double distance = DistanceToPoint(p);
            return distance < 5;
        }

        private double DistanceToPoint(Point p)
        {
            float x0 = p.X;
            float y0 = p.Y;
            float x1 = Start.X;
            float y1 = Start.Y;
            float x2 = End.X;
            float y2 = End.Y;

            double numerator = Math.Abs((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1);
            double denominator = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));

            return numerator / denominator;
        }

        public virtual void Accept(IEdgeVisitor visitor) => visitor.Visit(this);

        //public virtual void Restore()
        //{
        //    Start.Restore();
        //}

        //public virtual void MoveOwnedVertices(float dx, float dy)
        //{
        //    Start.Move(dx, dy);
        //}
        public virtual bool TryGetHitOwnedVertex(Point p, out Vertex? vertex)
        {
            vertex = null;

            if (Start.IsHit(p))
                vertex = Start;

            return vertex is not null;
        }
        public virtual bool TryGetHitOwnedVertex(float x, float y, out Vertex? vertex)
        {
            vertex = null;

            if (Start.IsHit(x, y))
                vertex = Start;

            return vertex is not null;
        }
        //public virtual void ResetOwnedVerticesMovementFlags()
        //{
        //    Start.WasMoved = false;
        //    Start.ContinuityPropertiesChanged = false;
        //}
        //public virtual void ResetOwnedMovedVerticesPreviousPositions()
        //{
        //    Start.ResetPreviousPosition();
        //}
    }
}
