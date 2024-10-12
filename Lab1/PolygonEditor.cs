using System.Windows.Forms.VisualStyles;

namespace Lab1
{
    public partial class PolygonEditor : Form
    {
        private Bitmap Bitmap { get; }
        private List<Edge> Edges { get; } = [];
        private Vertex? SelectedVertex { get; set; }
        private Edge? SelectedEdge { get; set; }
        private Edge? DrawnEdge { get; set; }
        private bool IsDrawing { get; set; } = false;
        private Point startPoint { get; set; }

        private IEdgeVisitor EdgeDrawingVisitor { get; set; }
        Graphics G { get; }


        public PolygonEditor()
        {
            InitializeComponent();
            //ContextMenuStrip = edgesContextMenuStrip;
            Bitmap = new Bitmap(drawingPictureBox.Width, drawingPictureBox.Height);
            drawingPictureBox.Image = Bitmap;
            G = Graphics.FromImage(Bitmap);
            G.Clear(Color.White);

            //ContextMenuStrip.Enabled = false;

            EdgeDrawingVisitor = new EdgeDrawingVisitor(G);
        }

        private void drawingPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!IsDrawing && Edges.Count == 0)
                {
                    //startPoint = e.Location;
                    DrawnEdge = new Edge(new Vertex(e.Location), new Vertex(e.Location));
                    IsDrawing = true;
                }
                else if (IsDrawing)
                {
                    Edges.Add(DrawnEdge!);
                    if (Edges[0].Start.IsHit(e.Location))
                    {
                        DrawnEdge!.End = Edges[0].Start;
                        DrawnEdge = null;
                        IsDrawing = false;

                    }
                    else
                    {
                        DrawnEdge = new Edge(Edges[^1].End, new Vertex(e.Location));
                    }
                }

                //SelectedVertex = Edges.Find(edge => edge.Start.IsHit(e.Location))?.Start;


                //Edge edge = new Edge(new Vertex(1,2), new Vertex(3,4));
                //edge.Accept(new EdgeDrawingVisitor());
            }
            //Edge? edge = Edges.Find(edge => edge.IsHit(e.Location));
            else if (e.Button == MouseButtons.Right && !IsDrawing)
            {
                SelectedEdge = Edges.Find(edge => edge.IsHit(e.Location));
                if (SelectedEdge is not null)
                {
                    //ContextMenuStrip!.Show(drawingPictureBox, e.Location);
                    //drawingPictureBox.ContextMenuStrip = edgesContextMenuStrip;
                    edgesContextMenuStrip.Show(drawingPictureBox, e.Location);

                }
            }

        }


        private void drawingPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrawing)
            {
                DrawnEdge!.End.Position = e.Location;
                drawingPictureBox.Invalidate();
            }
            if (SelectedVertex is not null)
            {
                try
                {
                    SelectedVertex.Position = e.Location;
                    SelectedVertex.WasMoved = true;
                    SelectedVertex.NeighbourPositionChanged();
                    //ResetVertexMovementFlags();
                    drawingPictureBox.Invalidate();
                }
                catch(VertexAlreadyMovedException)
                {
                    foreach (Edge edge in Edges)
                        edge.Start.Restore();
                    //MessageBox.Show("Wierzcho³ek nie mo¿e zostaæ przesuniêty ze wzglêdu na ograniczenia");
                }
                finally
                {
                    ResetVertexMovementFlags();
                }
            }

        }

        private void drawingPictureBox_Paint(object sender, PaintEventArgs e)
        {
            G.Clear(Color.White);
            foreach (Edge edge in Edges)
            {
                edge.Accept(EdgeDrawingVisitor);
                //G.DrawEllipse(Pens.Black, edge.Start.Position.X - 5, edge.Start.Position.Y - 5, 10, 10);
                G.FillEllipse(Brushes.Black, edge.Start.Position.X - 5, edge.Start.Position.Y - 5, 10, 10);
            }
            DrawnEdge?.Accept(EdgeDrawingVisitor);
            if (DrawnEdge is not null)
            {
                //G.DrawEllipse(Pens.Black, DrawnEdge.Start.Position.X - 5, DrawnEdge.Start.Position.Y - 5, 10, 10);
                G.FillEllipse(Brushes.Black, DrawnEdge.Start.Position.X - 5, DrawnEdge.Start.Position.Y - 5, 10, 10);
            }
            drawingPictureBox.Image = Bitmap;
        }

        private void drawingPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            SelectedVertex = Edges.Find(edge => edge.Start.IsHit(e.Location))?.Start;
        }

        private void drawingPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            SelectedVertex = null;
        }

        private void sta³aD³ugoœæToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedEdge!.IsBasic)
            {
                FixedEdge fixedEdge = new FixedEdge(SelectedEdge!.Start, SelectedEdge!.End);
                int index = Edges.FindIndex(edge => edge == SelectedEdge);
                Edges[index] = fixedEdge;
                ResetVertexMovementFlags();
            }
            else
            {
                Task.Run(() => MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe."));
            }
        }

        private void ResetVertexMovementFlags()
        {
            foreach (Edge edge in Edges)
            {
                edge.Start.WasMoved = false;
                edge.Start.WasChecked = false;
            }
        }

        private void pionowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = Edges.FindIndex(edge => edge == SelectedEdge);
            int leftIndex = index == 0 ? Edges.Count - 1 : index - 1;
            int rightIndex = index == Edges.Count - 1 ? 0 : index + 1;


            if (!SelectedEdge!.IsBasic)
                Task.Run(() => MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe.", "Nieprawid³owa operacja"));
            else if (Edges[leftIndex].IsVertical || Edges[rightIndex].IsVertical)
                Task.Run(() => MessageBox.Show("Nie mo¿na ustawiæ ograniczenia pionowego dla dwóch s¹siednich krawêdzi"));
            else if (SelectedEdge!.IsBasic)
            {
                VerticalEdge verticalEdge = new VerticalEdge(SelectedEdge!.Start, SelectedEdge!.End);

                Edges[index] = verticalEdge;
                ResetVertexMovementFlags();
            }
            //else
            //{
            //    MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe");
            //}
        }

        private void poziomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = Edges.FindIndex(edge => edge == SelectedEdge);
            int leftIndex = index == 0 ? Edges.Count - 1 : index - 1;
            int rightIndex = index == Edges.Count - 1 ? 0 : index + 1;
            if (!SelectedEdge!.IsBasic)
                Task.Run(() => MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe."));
            else if (Edges[leftIndex].IsHorizontal || Edges[rightIndex].IsHorizontal)
                Task.Run(() => MessageBox.Show("Nie mo¿na ustawiæ ograniczenia poziomego dla dwóch s¹siednich krawêdzi"));
            else if (SelectedEdge!.IsBasic)
            {
                HorizontalEdge verticalEdge = new HorizontalEdge(SelectedEdge!.Start, SelectedEdge!.End);
                //int index = Edges.FindIndex(edge => edge == SelectedEdge);
                Edges[index] = verticalEdge;
                ResetVertexMovementFlags();
            }
            //else
            //{
            //    MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe");
            //}
        }

        private void beToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dodajWierzcho³ekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Vertex start = SelectedEdge!.Start;
            Vertex end = SelectedEdge!.End;
            Vertex middle = new Vertex((start.Position.X + end.Position.X) / 2, (start.Position.Y + end.Position.Y) / 2);
            Edge edge1 = new Edge(start, middle);
            Edge edge2 = new Edge(middle, end);
            int index = Edges.FindIndex(edge => edge == SelectedEdge);
            Edges.RemoveAt(index);
            Edges.InsertRange(index, [edge1, edge2]);
        }
    }

    public interface IEdgeVisitor
    {        
        void Visit(Edge edge);
        void Visit(HorizontalEdge edge);
        void Visit(VerticalEdge edge);
        void Visit(FixedEdge edge);
        void Visit(BezierEdge edge);
    }
    public class EdgeDrawingVisitor: IEdgeVisitor
    {
        //Bitmap Bitmap { get; }
        Graphics G { get; }
        //public EdgeDrawingVisitor(Bitmap bitmap)
        //{
        //    Bitmap = bitmap;
        //}
        public EdgeDrawingVisitor(Graphics g) => G = g;

        private void DrawStraightLine(Point start, Point end)
        {
            G.DrawLine(Pens.Black, start, end);
        }
        public void Visit(Edge edge)
        {
            //Pen pen = new Pen(Color.Black);
            //using Graphics g = Graphics.FromImage(Bitmap);

            //g.DrawLine(Pens.Black, startPoint, e.Location);
            DrawStraightLine(edge.Start.Position, edge.End.Position);
            //drawingPictureBox.Image = Bitmap;
        }

        public void Visit(HorizontalEdge edge)
        {
            // draw horizontal edge
            DrawStraightLine(edge.Start.Position, edge.End.Position);

        }

        public void Visit(VerticalEdge edge)
        {
            // draw vertical edge
            DrawStraightLine(edge.Start.Position, edge.End.Position);
        }

        public void Visit(FixedEdge edge)
        {
            // draw fixed edge
            DrawStraightLine(edge.Start.Position, edge.End.Position);
        }

        public void Visit(BezierEdge edge)
        {
            // draw bezier edge
        }
    }



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

    public interface IEdgeVisitable
    {
        void Accept(IEdgeVisitor visitor);
    }

    public class Edge: IEdgeVisitable
    {
        public virtual Vertex Start { get; set; }
        public virtual Vertex End { get; set; }

        public virtual void StartChanged()
        {
            //if(Start.WasMoved)
            //    throw new VertexAlreadyMovedException();

            //Start.WasMoved = true;
            Start.WasChecked = true;

            if (!End.WasChecked)
                End.NeighbourPositionChanged();
        }

        public Edge(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
            Start.PositionChanged += StartChanged;
        }


        public virtual bool IsBasic { get => true; }
        public virtual bool IsHorizontal { get => false; }
        public virtual bool IsVertical { get => false; }
        public virtual bool IsFixed { get => false; }
        public virtual bool IsBezier { get => false; }


        public bool IsHit(Point p)
        {
            double distance = DistanceToPoint(p);
            return distance < 5;
        }

        protected virtual double DistanceToPoint(Point p)
        {
            var x0 = p.X;
            var y0 = p.Y;
            var x1 = Start.Position.X;
            var y1 = Start.Position.Y;
            var x2 = End.Position.X;
            var y2 = End.Position.Y;

            double numerator = Math.Abs((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1);
            double denominator = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));

            return numerator / denominator;
        }

        public virtual void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }

    public class  HorizontalEdge: Edge
    {
        public override bool IsBasic { get => false; }
        public override bool IsHorizontal { get => true; }

        public HorizontalEdge(Vertex start, Vertex end) : base(start, end)
        {
            End.Position = new Point(End.Position.X, Start.Position.Y);
            End.WasMoved = true;
            End.NeighbourPositionChanged();
        }

        public override void StartChanged()
        {

            if(Start.Position.Y == End.Position.Y)
            {
                if (!End.WasChecked)
                    End.NeighbourPositionChanged();
                return;
            }

            //if(End.WasMoved && Start.WasMoved)
            //    throw new VertexAlreadyMovedException();

            Start.WasChecked = true;

            if (/*!End.WasMoved*/ Start.WasMoved)
            {
                End.Position = new Point(End.Position.X, Start.Position.Y);
                End.WasMoved = true;
                End.NeighbourPositionChanged();
            }
            else if (/*!Start.WasMoved*/ End.WasMoved)
            {
                Start.Position = new Point(Start.Position.X, End.Position.Y);
                Start.WasMoved = true;
                //Start.NeighbourPositionChanged();
            }
            //End.Position = new Point(End.Position.X, Start.Position.Y);
            //End.WasMoved = true;
            //End.NeighbourPositionChanged();
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }

    public class VerticalEdge : Edge
    {
        public override bool IsBasic { get => false; }
        public override bool IsVertical { get => true; }

        public VerticalEdge(Vertex start, Vertex end) : base(start, end)
        {
            End.Position = new Point(Start.Position.X, End.Position.Y);
            End.WasMoved = true;
            End.NeighbourPositionChanged();
        }

        public override void StartChanged()
        {

            if (Start.Position.X == End.Position.X)
            {
                if (!End.WasChecked)
                    End.NeighbourPositionChanged();
                return;
            }

            //if (End.WasMoved && Start.WasMoved)
            //    throw new VertexAlreadyMovedException();

            Start.WasChecked = true;

            if(/*!End.WasMoved*/ Start.WasMoved)
            {
                End.Position = new Point(Start.Position.X, End.Position.Y);
                End.WasMoved = true;
                End.NeighbourPositionChanged();
            }
            else if (/*!Start.WasMoved*/End.WasMoved)
            {
                Start.Position = new Point(End.Position.X, Start.Position.Y);
                Start.WasMoved = true;
                //Start.NeighbourPositionChanged();
            }

            //End.Position = new Point(Start.Position.X, End.Position.Y);
            //End.WasMoved = true;
            //End.NeighbourPositionChanged();
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }

    public class FixedEdge : Edge
    {
        public override bool IsBasic { get => false; }
        public override bool IsFixed { get => true; }
        public int Length { get; set; }

        public FixedEdge(Vertex start, Vertex end) : base(start, end)
        {
            //End.Position
        }

        public override void StartChanged()
        {
            //if (End.WasMoved && Start.WasMoved)
            //    throw new VertexAlreadyMovedException();

            Start.WasChecked = true;
            Point positionDifference;

            if(/*!End.WasMoved*/Start.WasMoved)
            {
                positionDifference = Start.PositionDifference;
                End.Position = new Point(End.Position.X + positionDifference.X, End.Position.Y + positionDifference.Y);
                End.WasMoved = true;
                End.NeighbourPositionChanged();
            }
            else if (/*!Start.WasMoved*/End.WasMoved)
            {
                positionDifference = End.PositionDifference;
                Start.Position = new Point(Start.Position.X + positionDifference.X, Start.Position.Y + positionDifference.Y);
                Start.WasMoved = true;
                //Start.NeighbourPositionChanged();
            }
            //End.Position = new Point(End.Position.X + positionDifference.X, End.Position.Y + positionDifference.Y);
            //End.WasMoved = true;
            //End.NeighbourPositionChanged();
        }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }

    public class BezierEdge : Edge
    {
        public override bool IsBasic { get => false; }
        public override bool IsBezier { get => true; }

        public BezierEdge(Vertex start, Vertex end) : base(start, end) { }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }


    public class VertexAlreadyMovedException : Exception
    {
        public VertexAlreadyMovedException() : base() { }
    }
}
