using Lab1.GeometryModel;
using Lab1.GeometryModel.Edges;
using Lab1.LineDrawers;
using System.Net;

namespace Lab1.Visitors.VoidVisitors
{
    public class EdgeDrawingVisitor : IEdgeVoidVisitor
    {
        //Bitmap Bitmap { get; }
        //Graphics G { get; }
        ////public EdgeDrawingVisitor(Bitmap bitmap)
        ////{
        ////    Bitmap = bitmap;
        ////}
        //public EdgeDrawingVisitor(Graphics g) => G = g;
        private Graphics G { get; }
        public ILineDrawer LineDrawer { get; set; }
        public EdgeDrawingVisitor(ILineDrawer lineDrawer, Graphics g)
        {
            LineDrawer = lineDrawer;
            G = g;
        }

        //private void DrawStraightLine(Point start, Point end)
        //{
        //    G.DrawLine(Pens.Black, start, end);
        //}
        public void Visit(Edge edge)
        {
            //Pen pen = new Pen(Color.Black);
            //using Graphics g = Graphics.FromImage(Bitmap);

            //g.DrawLine(Pens.Black, startPoint, e.Location);
            //DrawStraightLine(edge.Start.Position, edge.End.Position);
            //drawingPictureBox.Image = Bitmap;
            Point start = new Point((int)edge.Start.X, (int)edge.Start.Y);
            Point end = new Point((int)edge.End.X, (int)edge.End.Y);
            LineDrawer.DrawStraightLine(start, end);
        }

        public void Visit(HorizontalEdge edge)
        {
            // draw horizontal edge
            //DrawStraightLine(edge.Start.Position, edge.End.Position);
            Point start = new Point((int)edge.Start.X, (int)edge.Start.Y);
            Point end = new Point((int)edge.End.X, (int)edge.End.Y);
            LineDrawer.DrawHorizontalLine(start, end);
            //edge.RemoveConstraintButton.BringToFront();

            Vertex startPoint = edge.Start;
            Vertex endPoint = edge.End;

            PointF midPoint = new PointF((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
            Font font = new Font("Arial", 16, FontStyle.Bold);
            SizeF textSize = G.MeasureString("H", font);
            G.DrawString("H", font, Brushes.LightBlue, midPoint.X - textSize.Width / 2, midPoint.Y - textSize.Height - 5);

        }

        public void Visit(VerticalEdge edge)
        {
            // draw vertical edge
            //DrawStraightLine(edge.Start.Position, edge.End.Position);
            Point start = new Point((int)edge.Start.X, (int)edge.Start.Y);
            Point end = new Point((int)edge.End.X, (int)edge.End.Y);
            LineDrawer.DrawVerticalLine(start, end);

            Vertex startPoint = edge.Start;
            Vertex endPoint = edge.End;

            PointF midPoint = new PointF((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
            Font font = new Font("Arial", 16, FontStyle.Bold);
            SizeF textSize = G.MeasureString("V", font);
            G.DrawString("V", font, Brushes.LightBlue, midPoint.X + 5, midPoint.Y - textSize.Height / 2);
        }

        public void Visit(FixedEdge edge)
        {
            // draw fixed edge
            //DrawStraightLine(edge.Start.Position, edge.End.Position);
            Point start = new Point((int)edge.Start.X, (int)edge.Start.Y);
            Point end = new Point((int)edge.End.X, (int)edge.End.Y);
            LineDrawer.DrawStraightLine(start, end);


            Vertex startPoint = edge.Start;
            Vertex endPoint = edge.End;

            double length = edge.Length;
            double angle = Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X) * 180 / Math.PI;
            bool isTextAbove = true;

            if (angle > 90)
            {
                angle -= 180;
                isTextAbove = false;
            }
            else if (angle < -90)
            {
                angle += 180;
                isTextAbove = false;
            }

            PointF midPoint = new PointF((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);

            float offsetX = (float)(10 * (endPoint.Y - startPoint.Y) / length);
            float offsetY = (float)(10 * (endPoint.X - startPoint.X) / length);
            PointF startOffset = new PointF(startPoint.X + offsetX, startPoint.Y - offsetY);
            PointF endOffset = new PointF(endPoint.X + offsetX, endPoint.Y - offsetY);
            G.DrawLine(Pens.Gray, startOffset, endOffset);

            // Przekształcanie grafiki dla obrotu tekstu
            G.TranslateTransform(midPoint.X + offsetX, midPoint.Y - offsetY);
            G.RotateTransform((float)angle);

            // Rysowanie tekstu z długością
            string text = $"{length:N0}";
            Font font = new Font("Arial", 12);
            SizeF textSize = G.MeasureString(text, font);

            float y = 5;
            if (isTextAbove)
                y = -textSize.Height - y;

            // Rysowanie tekstu na środku odcinka
            G.DrawString(text, font, Brushes.Blue, -textSize.Width / 2, y);

            // Resetowanie transformacji
            G.ResetTransform();

        }

        public void Visit(BezierEdge edge)
        {
            // draw bezier edge
            Point start = new Point((int)edge.Start.X, (int)edge.Start.Y);
            Point end = new Point((int)edge.End.X, (int)edge.End.Y);
            Point v1 = new Point((int)edge.V1.X, (int)edge.V1.Y);
            Point v2 = new Point((int)edge.V2.X, (int)edge.V2.Y);
            LineDrawer.DrawBezierCurve(start, end, v1, v2);

            G.DrawEllipse(Pens.Black, v1.X - 5, v1.Y - 5, 10, 10);
            G.DrawEllipse(Pens.Black, v2.X - 5, v2.Y - 5, 10, 10);


            if (edge.Start.Continuity != Vertex.ContuinityType.G0)
            {
                drawContinuity(edge.Start);
            }
            if (edge.End.Continuity != Vertex.ContuinityType.G0)
            {
                drawContinuity(edge.End);
            }


            void drawContinuity(Vertex vertex)
            {
                string text = vertex.Continuity.ToString();

                // Ustalanie czcionki
                Font font = new Font("Arial", 12);
                SizeF textSize = G.MeasureString(text, font);

                // Pozycjonowanie tekstu w okolicy wierzchołka (np. nad nim, przesunięte w prawo)
                PointF textPosition = new PointF(vertex.X + 5, vertex.Y - textSize.Height - 5);

                // Rysowanie literki klasy ciągłości
                G.DrawString(text, font, Brushes.LightBlue, textPosition);
            }
        }
    }
}
