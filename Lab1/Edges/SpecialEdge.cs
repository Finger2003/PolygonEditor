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
            //SetButtonPosition();
        }
        public override bool IsBasic { get => false; }
        public Button RemoveConstraintButton { get; } = new Button();
        protected Point ButtonPreviousPosition { get; set; }
        protected virtual void SetButtonPosition()
        {
            // Oblicz środek linii
            double centerX = (Start.Position.X + End.Position.X) / 2;
            double centerY = (Start.Position.Y + End.Position.Y) / 2;

            // Oblicz wektor linii (różnica między Start i End)
            double deltaX = End.Position.X - Start.Position.X;
            double deltaY = End.Position.Y - Start.Position.Y;

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
            RemoveConstraintButton.Location = new Point((int)(centerX + offsetX - RemoveConstraintButton.Width/2), (int)(centerY + offsetY - RemoveConstraintButton.Height / 2));

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
