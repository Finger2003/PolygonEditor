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
        public ILineDrawer LineDrawer { get; set; }
        public EdgeDrawingVisitor(ILineDrawer lineDrawer) => LineDrawer = lineDrawer;

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
