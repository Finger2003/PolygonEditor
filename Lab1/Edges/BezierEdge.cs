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
            V1 = new Vertex((Start.Position.X + End.Position.X) / 2, Start.Position.Y);
            V2 = new Vertex((Start.Position.X + End.Position.X) / 2, End.Position.Y);
            SetButtonPosition();
        }

        public override void MoveOwnedVertices(int dx, int dy)
        {
            base.MoveOwnedVertices(dx, dy);
            V1.Position = new Point(V1.Position.X + dx, V1.Position.Y + dy);
            V2.Position = new Point(V2.Position.X + dx, V2.Position.Y + dy);
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
            int centerX = (V1.Position.X + V2.Position.X) / 2;
            int centerY = (V1.Position.Y + V2.Position.Y) / 2;

            // Oblicz wektor linii (różnica między V1 i V2)
            int deltaX = V2.Position.X - V1.Position.X;
            int deltaY = V2.Position.Y - V1.Position.Y;

            // Oblicz wektor prostopadły do linii (wymiana współrzędnych i zmiana znaku jednej z nich)
            int perpendicularX = -deltaY;
            int perpendicularY = deltaX;

            // Normalizuj wektor prostopadły, aby miał długość 10 pikseli
            double length = Math.Sqrt(perpendicularX * perpendicularX + perpendicularY * perpendicularY);
            double unitX = perpendicularX / length;
            double unitY = perpendicularY / length;
            int offsetX = (int)(unitX * 10);  // Przesunięcie w poziomie (nad linią)
            int offsetY = (int)(unitY * 10);  // Przesunięcie w pionie (nad linią)

            // Ustawienie pozycji przycisku z przesunięciem o 10 pikseli w górę
            RemoveConstraintButton.Location = new Point(centerX + offsetX - RemoveConstraintButton.Width / 2, centerY + offsetY - RemoveConstraintButton.Height / 2);

            ButtonPreviousPosition = RemoveConstraintButton.Location;
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
