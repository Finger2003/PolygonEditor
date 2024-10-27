using Lab1.Visitors;

namespace Lab1.Edges
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
            //SetButtonPosition();
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

        //protected override void SetButtonPosition()
        //{
        //    // Oblicz środek linii
        //    double centerX = (V1.Position.X + V2.Position.X) / 2;
        //    double centerY = (V1.Position.Y + V2.Position.Y) / 2;

        //    // Oblicz wektor linii (różnica między V1 i V2)
        //    double deltaX = V2.Position.X - V1.Position.X;
        //    double deltaY = V2.Position.Y - V1.Position.Y;

        //    // Oblicz wektor prostopadły do linii (wymiana współrzędnych i zmiana znaku jednej z nich)
        //    double perpendicularX = -deltaY;
        //    double perpendicularY = deltaX;

        //    // Normalizuj wektor prostopadły, aby miał długość 10 pikseli
        //    double length = Math.Sqrt(perpendicularX * perpendicularX + perpendicularY * perpendicularY);
        //    double unitX = perpendicularX / length;
        //    double unitY = perpendicularY / length;
        //    double offsetX = unitX * 10;  // Przesunięcie w poziomie (nad linią)
        //    double offsetY = unitY * 10;  // Przesunięcie w pionie (nad linią)

        //    // Ustawienie pozycji przycisku z przesunięciem o 10 pikseli w górę
        //    RemoveConstraintButton.Location = new Point((int)(centerX + offsetX - RemoveConstraintButton.Width / 2), (int)(centerY + offsetY - RemoveConstraintButton.Height / 2));

        //    ButtonPreviousPosition = RemoveConstraintButton.Location;
        //}

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

            //if (v == V1)
            //    return Math.Atan2(V1.Position.Y - Start.Position.Y, V1.Position.X - Start.Position.X);

            //if (v == V2)
            //    return Math.Atan2(V2.Position.Y - End.Position.Y, V2.Position.X - End.Position.X);

            //throw new ArgumentException("Vertex is not a control point of this edge");
        }

        protected override double GetControlLength(Vertex v1, Vertex v2)
        {
            return Vertex.Distance(v1, v2);
            //if (v == V1)
            //    return Vertex.Distance(Start, V1);

            //if (v == V2)
            //    return Vertex.Distance(End, V2);

            //throw new ArgumentException("Vertex is not a control point of this edge");
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
            //V1.ContinuityPropertiesChanged = false;
            //V2.ContinuityPropertiesChanged = false;
        }

        public override void ResetOwnedMovedVerticesPreviousPositions()
        {
            base.ResetOwnedMovedVerticesPreviousPositions();
            //if (V1.WasMoved)
            V1.ResetPreviousPosition();
            //if (V2.WasMoved)
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
