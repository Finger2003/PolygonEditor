﻿namespace PolygonEditor.LineDrawers
{
    public abstract class AbstractLineDrawer : ILineDrawer
    {
        private Pen DottedPen { get; } = new Pen(Color.Black) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
        protected Graphics G;
        protected AbstractLineDrawer(Graphics g) => G = g;
        public abstract void DrawHorizontalLine(Point start, Point end);
        public abstract void DrawVerticalLine(Point start, Point end);
        public abstract void DrawStraightLine(Point start, Point end);
        public void DrawBezierCurve(Point v0, Point v3, Point v1, Point v2)
        {
            int a1x = 3 * (v1.X - v0.X);
            int a1y = 3 * (v1.Y - v0.Y);
            int a2x = 3 * (v2.X - 2 * v1.X + v0.X);
            int a2y = 3 * (v2.Y - 2 * v1.Y + v0.Y);
            int a3x = v3.X - 3 * v2.X + 3 * v1.X - v0.X;
            int a3y = v3.Y - 3 * v2.Y + 3 * v1.Y - v0.Y;


            int deltaX = v3.X - v0.X;
            int deltaY = v3.Y - v0.Y;

            int loopIterations = (int)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            double dt = 1 / (double)loopIterations;
            double dt2 = dt * dt;
            double dt3 = dt2 * dt;

            double dddX = 6 * a3x * dt3;
            double dddY = 6 * a3y * dt3;
            double ddX = dddX + 2 * a2x * dt2;
            double ddY = dddY + 2 * a2y * dt2;
            double dX = a3x * dt3 + a2x * dt2 + a1x * dt;
            double dY = a3y * dt3 + a2y * dt2 + a1y * dt;


            double oldX, x, oldY, y;
            oldX = x = v0.X;
            oldY = y = v0.Y;

            for (int i = 0; i < loopIterations; i++)
            {
                x += dX;
                y += dY;
                dX += ddX;
                dY += ddY;
                ddX += dddX;
                ddY += dddY;

                G.DrawLine(Pens.Black, (float)oldX, (float)oldY, (float)x, (float)y);
                oldX = x;
                oldY = y;
            }




            G.DrawLine(DottedPen, v0, v1);
            G.DrawLine(DottedPen, v1, v2);
            G.DrawLine(DottedPen, v2, v3);
        }
    }
}
