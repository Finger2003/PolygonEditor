namespace Lab1.LineDrawers
{
    public class BresenhamLineDrawer : AbstractLineDrawer
    {
        private Bitmap Bitmap { get; }
        public BresenhamLineDrawer(Bitmap bitmap) : base(Graphics.FromImage(bitmap)) => Bitmap = bitmap;

        public override void DrawHorizontalLine(Point start, Point end)
        {
            for (int i = start.X; i <= end.X; i++)
            {
                SetBitmapPixel(i, start.Y);
                //Bitmap.SetPixel(i, start.Y, Color.Black);
            }
        }
        public override void DrawVerticalLine(Point start, Point end)
        {
            for (int i = start.Y; i <= end.Y; i++)
            {
                SetBitmapPixel(start.X, i);
                //Bitmap.SetPixel(start.X, i, Color.Black);
            }
        }

        public override void DrawStraightLine(Point start, Point end)
        {
            int x0 = start.X;
            int y0 = start.Y;
            int x1 = end.X;
            int y1 = end.Y;

            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;

            while (true)
            {
                SetBitmapPixel(x0, y0);
                if (x0 == x1 && y0 == y1)
                    break;
                int e2 = 2 * err;
                if (e2 >= dy)
                {
                    err += dy;
                    x0 += sx;
                }
                if (e2 <= dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        private void SetBitmapPixel(int x, int y)
        {
            Bitmap.SetPixel(x, y, Color.Black);
        }
    }
}
