namespace Lab1.LineDrawers
{
    public abstract class AbstractLineDrawer : ILineDrawer
    {
        //protected Graphics G { get; }
        //public AbstractLineDrawer(Graphics g) => G = g;
        private Pen DottedPen { get; } = new Pen(Color.Black) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
        protected Graphics G;
        protected AbstractLineDrawer(Graphics g) => G = g;
        public abstract void DrawHorizontalLine(Point start, Point end);
        public abstract void DrawVerticalLine(Point start, Point end);
        public abstract void DrawStraightLine(Point start, Point end);
        public void DrawBezierCurve(Point V0, Point V3, Point V1, Point V2)
        {
            int A0x = V0.X;
            int A0y = V0.Y;
            int A1x = 3 * (V1.X - V0.X);
            int A1y = 3 * (V1.Y - V0.Y);
            int A2x = 3 * (V2.X - 2 * V1.X + V0.X);
            int A2y = 3 * (V2.Y - 2 * V1.Y + V0.Y);
            int A3x = V3.X - 3 * V2.X + 3 * V1.X - V0.X;
            int A3y = V3.Y - 3 * V2.Y + 3 * V1.Y - V0.Y;


            double oldX = V0.X;
            double oldY = V0.Y;
            for (double t = 0; t <= 1; t += 0.01)
            {
                double x = ((A3x * t + A2x) * t + A1x) * t + A0x;
                double y = ((A3y * t + A2y) * t + A1y) * t + A0y;
                G.DrawLine(Pens.Black, (float)oldX, (float)oldY, (float)x, (float)y);
                oldX = x;
                oldY = y;
            }



            G.DrawEllipse(Pens.Black, V1.X - 2, V1.Y - 2, 4, 4);
            G.DrawEllipse(Pens.Black, V2.X - 2, V2.Y - 2, 4, 4);
            G.DrawLine(DottedPen, V0, V1);
            G.DrawLine(DottedPen, V1, V2);
            G.DrawLine(DottedPen, V2, V3);
        }
    }
}
