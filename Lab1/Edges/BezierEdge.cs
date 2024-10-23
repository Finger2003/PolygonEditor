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
            V1 = new Vertex((Start.X + End.X) / 2, Start.Y);
            V2 = new Vertex((Start.X + End.X) / 2, End.Y);
            SetButtonPosition();
        }

        public override void MoveOwnedVertices(int dx, int dy)
        {
            base.MoveOwnedVertices(dx, dy);
            V1.Move(dx, dy);
            V2.Move(dx, dy);
        }

        public override bool TryGetHitOwnedVertex(Point p, out Vertex? vertex)
        {
            vertex = null;

            if(Start.IsHit(p))
                vertex = Start;
            else if(V1.IsHit(p))
                vertex = V1;
            else if (V2.IsHit(p))
                vertex = V2;

            return vertex is not null;
        }

        protected override void SetButtonPosition()
        {
            // Oblicz środek linii
            double centerX = (V1.Position.X + V2.Position.X) / 2;
            double centerY = (V1.Position.Y + V2.Position.Y) / 2;

            // Oblicz wektor linii (różnica między V1 i V2)
            double deltaX = V2.Position.X - V1.Position.X;
            double deltaY = V2.Position.Y - V1.Position.Y;

            // Oblicz wektor prostopadły do linii (wymiana współrzędnych i zmiana znaku jednej z nich)
            double perpendicularX = -deltaY;
            double perpendicularY = deltaX;

            // Normalizuj wektor prostopadły, aby miał długość 10 pikseli
            double length = Math.Sqrt(perpendicularX * perpendicularX + perpendicularY * perpendicularY);
            double unitX = perpendicularX / length;
            double unitY = perpendicularY / length;
            double offsetX = unitX * 10;  // Przesunięcie w poziomie (nad linią)
            double offsetY = unitY * 10;  // Przesunięcie w pionie (nad linią)

            // Ustawienie pozycji przycisku z przesunięciem o 10 pikseli w górę
            RemoveConstraintButton.Location = new Point((int) (centerX + offsetX - RemoveConstraintButton.Width / 2), (int) (centerY + offsetY - RemoveConstraintButton.Height / 2));

            ButtonPreviousPosition = RemoveConstraintButton.Location;
        }

        public override bool CorrectEndPosition()
        {
            return false;
        }

        public override bool CorrectStartPosition()
        {
            return false;
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
