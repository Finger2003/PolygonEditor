namespace PolygonEditor.LineDrawers
{
    public class DefaultLineDrawer : AbstractLineDrawer
    {
        public DefaultLineDrawer(Graphics g) : base(g) { }

        public override void DrawHorizontalLine(Point start, Point end)
        {
            G.DrawLine(Pens.Black, start, end);
        }

        public override void DrawVerticalLine(Point start, Point end)
        {
            G.DrawLine(Pens.Black, start, end);
        }

        public override void DrawStraightLine(Point start, Point end)
        {
            G.DrawLine(Pens.Black, start, end);
        }
    }
}
