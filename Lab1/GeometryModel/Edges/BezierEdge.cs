using Lab1.Visitors;

namespace Lab1.GeometryModel.Edges
{
    public class BezierEdge : SpecialEdge
    {
        //public override bool IsBasic { get => false; }
        public override bool IsBezier { get => true; }
        public Vertex V1 { get; set; }
        public Vertex V2 { get; set; }

        public BezierEdge(Vertex start, Vertex end) : base(start, end)
        {
            V1 = new Vertex((Start.X + End.X) / 2, Start.Y, true);
            V2 = new Vertex((Start.X + End.X) / 2, End.Y, true);
        }

        public override void MoveOwnedVertices(float dx, float dy)
        {
            base.MoveOwnedVertices(dx, dy);
            V1.Move(dx, dy);
            V2.Move(dx, dy);
        }

        public override bool TryGetHitOwnedVertex(Point p, out Vertex? vertex)
        {
            vertex = null;

            if (Start.IsHit(p))
                vertex = Start;
            else if (V1.IsHit(p))
                vertex = V1;
            else if (V2.IsHit(p))
                vertex = V2;

            return vertex is not null;
        }

        public override bool TryGetHitOwnedVertex(float x, float y, out Vertex? vertex)
        {
            vertex = null;

            if (Start.IsHit(x, y))
                vertex = Start;
            else if (V1.IsHit(x, y))
                vertex = V1;
            else if (V2.IsHit(x, y))
                vertex = V2;

            return vertex is not null;
        }

        public override correctingStatus CorrectEndPosition()
        {
            if (Start.Continuity != Vertex.ContuinityType.G0 && !V1.WasMoved && Start.ContinuityPropertiesChanged)
            {
                double length = length = Start.ControlLength;
                if (Start.Continuity == Vertex.ContuinityType.G1)
                {
                    length = Vertex.Distance(Start.PreviousPosition, V1.Position);
                }

                double newX = Start.X + length * Math.Cos(Start.ControlAngle);
                double newY = Start.Y + length * Math.Sin(Start.ControlAngle);
                V1.SetPosition((float)newX, (float)newY);
                V1.WasMoved = true;
            }

            if (V2.WasMoved)
            {
                End.ControlAngle = GetControlAngle(V2, End);
                End.ControlLength = GetControlLength(V2, End);
                return correctingStatus.FurtherCorrectionNeeded;
            }
            return correctingStatus.FurtherCorrectionNotNeeded;
        }

        public override correctingStatus CorrectStartPosition()
        {
            if (End.Continuity != Vertex.ContuinityType.G0 && !V2.WasMoved && End.ContinuityPropertiesChanged)
            {
                double length = length = End.ControlLength;
                if (End.Continuity == Vertex.ContuinityType.G1)
                {
                    length = Vertex.Distance(V2.Position, End.PreviousPosition);
                }

                double newX = End.X - length * Math.Cos(End.ControlAngle);
                double newY = End.Y - length * Math.Sin(End.ControlAngle);
                V2.SetPosition((float)newX, (float)newY);
                V2.WasMoved = true;
            }

            if (V1.WasMoved)
            {
                Start.ControlAngle = GetControlAngle(Start, V1);
                Start.ControlLength = GetControlLength(Start, V1);
                return correctingStatus.FurtherCorrectionNeeded;
            }
            return correctingStatus.FurtherCorrectionNotNeeded;
        }

        protected override double GetControlAngle(Vertex v1, Vertex v2)
        {
            return Math.Atan2(v2.Y - v1.Y, v2.X - v1.X);
        }

        protected override double GetControlLength(Vertex v1, Vertex v2)
        {
            return Vertex.Distance(v1, v2);
        }

        public override bool IsControlVertex(Vertex v)
        {
            return v == V1 || v == V2;
        }

        public override void SetVerticesContinuityRelevantProperties(Vertex v)
        {
            if (v == V1 || v == Start)
            {
                Start.ControlAngle = GetControlAngle(Start, V1);
                Start.ControlLength = GetControlLength(Start, V1);
            }
            else if (v == V2 || v == End)
            {
                End.ControlAngle = GetControlAngle(V2, End);
                End.ControlLength = GetControlLength(V2, End);
            }
            else
            {
                throw new ArgumentException("Vertex is not a control point of this edge");
            }
        }

        public override void ResetOwnedVerticesMovementFlags()
        {
            base.ResetOwnedVerticesMovementFlags();
            V1.WasMoved = false;
            V2.WasMoved = false;
        }

        public override void ResetOwnedMovedVerticesPreviousPositions()
        {
            base.ResetOwnedMovedVerticesPreviousPositions();
            V1.ResetPreviousPosition();
            V2.ResetPreviousPosition();
        }

        public override void Restore()
        {
            base.Restore();
            V1.Restore();
            V2.Restore();
        }

        public override void CorrectStartPositionBasically() { }
        public override void CorrectEndPositionBasically() { }
        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
