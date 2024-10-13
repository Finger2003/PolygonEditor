using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Edges
{
    public abstract class SpecialEdge : Edge
    {
        protected SpecialEdge(Vertex start, Vertex end) : base(start, end)
        {
            RemoveConstraintButton.Text = "-";
            RemoveConstraintButton.Size = new System.Drawing.Size(15, 15);
            RemoveConstraintButton.Tag = this;
            //RemoveConstraintButton.BackColor = Color.Green;
            SetButtonPosition();
        }
        public override bool IsBasic { get => false; }
        public Button RemoveConstraintButton { get; } = new Button();
        protected Point ButtonPreviousPosition { get; set; }
        protected void SetButtonPosition()
        {
            // Oblicz środek linii
            int centerX = (Start.Position.X + End.Position.X) / 2;
            int centerY = (Start.Position.Y + End.Position.Y) / 2;

            // Oblicz wektor linii (różnica między Start i End)
            int deltaX = End.Position.X - Start.Position.X;
            int deltaY = End.Position.Y - Start.Position.Y;

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
            RemoveConstraintButton.Location = new Point(centerX + offsetX - RemoveConstraintButton.Width/2, centerY + offsetY - RemoveConstraintButton.Height/2);

            ButtonPreviousPosition = RemoveConstraintButton.Location;
        }
        public override void Restore()
        {
            base.Restore();
            RemoveConstraintButton.Location = ButtonPreviousPosition;
        }
        public override void OnMoved()
        {
            SetButtonPosition();
        }
    }
}
