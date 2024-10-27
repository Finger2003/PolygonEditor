using System.Numerics;

namespace Lab1.GeometryModel
{
    public class Vertex
    {
        public enum ContuinityType
        {
            G0,
            G1,
            C1
        }


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
        public Vector2 PreviousPosition { get; set; }
        public Vector2 PositionDifference { get => Position - PreviousPosition; }

        public bool IsControlPoint { get; }
        private double _controlAngle;
        private double _controlLength;
        public double ControlAngle
        {
            get => _controlAngle;
            set
            {
                PreviousControlAngle = ControlAngle;
                _controlAngle = value;
                ContinuityPropertiesChanged = true;
            }
        }
        public double ControlLength
        {
            get => _controlLength;
            set
            {
                PreviousControlLength = ControlLength;
                _controlLength = value;
                ContinuityPropertiesChanged = true;
            }
        }
        public ContuinityType Continuity { get; set; } = ContuinityType.G0;
        public bool ContinuityChanged { get; set; }
        public bool ContinuityPropertiesChanged { get; set; }
        public double PreviousControlAngle { get; set; }
        public double PreviousControlLength { get; set; }

        private Vertex(bool isControlPoint)
        {
            IsControlPoint = isControlPoint;
        }
        public Vertex(int x, int y, bool isControlPoint = false) : this(isControlPoint)
        {
            Position = new Vector2(x, y);
            PreviousPosition = Position;
        }
        public Vertex(float x, float y, bool isControlPoint = false) : this(isControlPoint)
        {
            Position = new Vector2(x, y);
            PreviousPosition = Position;
        }
        public Vertex(Point p, bool isControlPoint = false) : this(p.X, p.Y, isControlPoint) { }

        public void ResetPreviousPosition()
        {
            PreviousPosition = Position;
            PreviousControlAngle = ControlAngle;
            PreviousControlLength = ControlLength;
        }
        public void Restore()
        {
            Position = PreviousPosition;
            ControlAngle = PreviousControlAngle;
            ControlLength = PreviousControlLength;
        }



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
            return Math.Abs(p.X - Position.X) < 5 && Math.Abs(p.Y - Position.Y) < 5;
        }
        public static float Distance(Vertex v1, Vertex v2)
        {
            return Distance(v1.Position, v2.Position);
        }

        public static float Distance(Vector2 v1, Vector2 v2)
        {
            return Vector2.Distance(v1, v2);
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
