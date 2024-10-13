namespace Lab1.LineDrawers
{
    public class BresenhamLineDrawer : AbstractLineDrawer
    {
        private Bitmap Bitmap { get; }
        public BresenhamLineDrawer(Bitmap bitmap) : base(Graphics.FromImage(bitmap)) => Bitmap = bitmap;

        public override void DrawHorizontalLine(Point start, Point end)
        {
            if(start.Y < 0 || start.Y >= Bitmap.Height)
                return;


            int startX = Math.Min(Math.Max(0, start.X), Bitmap.Width - 1);
            int endX = Math.Min(Math.Max(0, end.X), Bitmap.Width - 1);


            if(startX > endX)
                (startX, endX) = (endX, startX);

            for (int i = startX; i <= endX; i++)
            {
                SetBitmapPixel(i, start.Y);
                //Bitmap.SetPixel(i, start.Y, Color.Black);
            }
        }
        public override void DrawVerticalLine(Point start, Point end)
        {
            if(start.X < 0 || start.X >= Bitmap.Width)
                return;

            int startY = Math.Min(Math.Max(0, start.Y), Bitmap.Height - 1);
            int endY = Math.Min(Math.Max(0, end.Y), Bitmap.Height - 1);

            if (startY > endY)
                (startY, endY) = (endY, startY);

            for (int i = startY; i <= endY; i++)
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
                if(x0 >= 0 && x0 < Bitmap.Width && y0 >= 0 && y0 < Bitmap.Height)
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
