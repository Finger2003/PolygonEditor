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
            LineDrawer.DrawStraightLine(edge.Start.Position, edge.End.Position);
        }

        public void Visit(HorizontalEdge edge)
        {
            // draw horizontal edge
            //DrawStraightLine(edge.Start.Position, edge.End.Position);
            LineDrawer.DrawHorizontalLine(edge.Start.Position, edge.End.Position);
            edge.RemoveConstraintButton.BringToFront();

        }

        public void Visit(VerticalEdge edge)
        {
            // draw vertical edge
            //DrawStraightLine(edge.Start.Position, edge.End.Position);
            LineDrawer.DrawVerticalLine(edge.Start.Position, edge.End.Position);
        }

        public void Visit(FixedEdge edge)
        {
            // draw fixed edge
            //DrawStraightLine(edge.Start.Position, edge.End.Position);
            LineDrawer.DrawStraightLine(edge.Start.Position, edge.End.Position);
        }

        public void Visit(BezierEdge edge)
        {
            // draw bezier edge
        }
    }
}
