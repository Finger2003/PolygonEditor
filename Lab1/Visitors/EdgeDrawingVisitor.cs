using Lab1.Edges;
using Lab1.LineDrawers;

namespace Lab1.Visitors
{
    public class EdgeDrawingVisitor : IEdgeVisitor
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

        }

        public void Visit(VerticalEdge edge)
        {
            // draw vertical edge
            //DrawStraightLine(edge.Start.Position, edge.End.Position);
            Point start = new Point((int)edge.Start.X, (int)edge.Start.Y);
            Point end = new Point((int)edge.End.X, (int)edge.End.Y);
            LineDrawer.DrawVerticalLine(start, end);
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
            double angle = Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X) * 180 /Math.PI;

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

            // Rysowanie tekstu na środku odcinka
            G.DrawString(text, font, Brushes.Blue, -textSize.Width / 2, -textSize.Height -5);

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
        }
    }
}
