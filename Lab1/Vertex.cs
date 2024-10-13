namespace Lab1
{
    public class Vertex
    {
        private Point _position;
        public Point Position
        {
            get => _position;
            set
            {
                PreviousPosition = _position;
                _position = value;
            }
        }
        public event Action? PositionChanged;
        public bool WasMoved { get; set; } = false;
        public bool WasChecked { get; set; } = false;
        private Point PreviousPosition { get; set; }
        public Point PositionDifference { get => new Point(Position.X - PreviousPosition.X, Position.Y - PreviousPosition.Y); }

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
            Position = new Point(x, y);
        }
        public Vertex(Point p)
        {
            Position = p;
        }

        public bool IsHit(Point p)
        {
            return (Math.Abs(p.X - Position.X) < 5 && Math.Abs(p.Y - Position.Y) < 5);
        }
        public void NeighbourPositionChanged()
        {
            PositionChanged?.Invoke();
        }
    }
}
