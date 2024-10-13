namespace Lab1
{
    public abstract class AbstractLineDrawer : ILineDrawer
    {
        //protected Graphics G { get; }
        //public AbstractLineDrawer(Graphics g) => G = g;
        protected Graphics G;
        protected AbstractLineDrawer(Graphics g) => G = g;
        public abstract void DrawHorizontalLine(Point start, Point end);
        public abstract void DrawVerticalLine(Point start, Point end);
        public abstract void DrawStraightLine(Point start, Point end);
        public void DrawBezierCurve(Point start, Point end, Point control1, Point control2)
        {
            throw new NotImplementedException();
        }
    }
}
