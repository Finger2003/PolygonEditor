using System.Numerics;

namespace Lab1
{
    public class Vertex
    {
        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set
            {
                PreviousPosition = _position;
                _position = value;
            }
        }

        public float X { get => Position.X; }
        public float Y { get => Position.Y; }

        public event Action? StartChanged;
        public event Action? EndChanged;
        public bool WasMoved { get; set; } = false;
        public bool WasChecked { get; set; } = false;
        private Vector2 PreviousPosition { get; set; }
        public Vector2 PositionDifference { get => Position - PreviousPosition; }

        public void ResetPreviousPosition()
        {
            PreviousPosition = Position;
        }
        public void Restore()
        {
            Position = PreviousPosition;
        }

        public Vertex(int x, int y)
        {
            Position = new Vector2(x, y);
        }
        public Vertex(float x, float y)
        {
            Position = new Vector2(x, y);
        }
        public Vertex(Point p) : this(p.X, p.Y) { }

        public void Move(Vector2 delta)
        {
            Position += delta;
        }
        public void Move(float dx, float dy)
        {
            Position += new Vector2(dx, dy);
        }

        public void SetPosition(float x, float y)
        {
            Position = new Vector2(x, y);
        }
        public void SetPosition(Point p)
        {
            SetPosition(p.X, p.Y);
        }

        public bool IsHit(Point p)
        {
            return (Math.Abs(p.X - Position.X) < 5 && Math.Abs(p.Y - Position.Y) < 5);
        }
        public void InvokeStartPositionChanged()
        {
            StartChanged?.Invoke();
        }
        public void InvokeEndPositionChanged()
        {
            EndChanged?.Invoke();
        }
    }
}
