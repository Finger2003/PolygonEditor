namespace Lab1
{
    public class DefaultLineDrawer : AbstractLineDrawer
    {
        public DefaultLineDrawer(Bitmap bitmap) : base(Graphics.FromImage(bitmap)) { }

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
