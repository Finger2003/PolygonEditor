namespace Lab1
{
    public interface ILineDrawer
    {
        public void DrawHorizontalLine(Point start, Point end);
        public void DrawVerticalLine(Point start, Point end);
        public void DrawStraightLine(Point start, Point end);
        public void DrawBezierCurve(Point start, Point end, Point control1, Point control2);
    }
}
