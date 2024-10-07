namespace Lab1
{
    public partial class PolygonEditor : Form
    {
        private Bitmap Bitmap { get; }
        private List<Edge> Edges { get; } = [];
        private Vertex? SelectedVertex { get; set; }
        private Edge? SelectedEdge { get; set; }
        private bool IsDrawing { get; set; } = false;
        private Point startPoint { get; set; }

        private IEdgeVisitor EdgeDrawingVisitor { get; set; }
        Graphics G { get; }


        public PolygonEditor()
        {
            InitializeComponent();
            Bitmap = new Bitmap(drawingPictureBox.Width, drawingPictureBox.Height);
            drawingPictureBox.Image = Bitmap;
            G = Graphics.FromImage(Bitmap);
            G.Clear(Color.White);

            EdgeDrawingVisitor = new EdgeDrawingVisitor(G);
        }

        private void drawingPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!IsDrawing && Edges.Count == 0)
                {
                    //startPoint = e.Location;
                    SelectedEdge = new Edge(new Vertex(e.Location), new Vertex(e.Location));
                    IsDrawing = true;
                }
                else
                {
                    Edges.Add(SelectedEdge!);
                    if (Edges[0].Start.IsHit(e.Location))
                    {
                        Edges[^1].End = Edges[0].Start;
                        SelectedEdge = null;
                        IsDrawing = false;
                    }
                    else
                    {
                        SelectedEdge = new Edge(Edges[^1].End, new Vertex(e.Location));
                    }
                }
                //Edge edge = new Edge(new Vertex(1,2), new Vertex(3,4));
                //edge.Accept(new EdgeDrawingVisitor());
            }
        }


        private void drawingPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrawing)
            {
                //var g = Graphics.FromImage(Bitmap);
                //g.Clear(Color.White);
                //g.DrawLine(Pens.Black, startPoint, e.Location);
                //drawingPictureBox.Image = Bitmap;

                //SelectedEdge ??= new Edge(new Vertex(startPoint), new Vertex(e.Location));
                SelectedEdge!.End = new Vertex(e.Location);
                drawingPictureBox.Invalidate();

                //using Graphics g = Graphics.FromImage(Bitmap);
                //g.Clear(Color.White);
                //SelectedEdge.Accept(EdgeDrawingVisitor);
                //drawingPictureBox.Image = Bitmap;
            }
        }

        private void drawingPictureBox_Paint(object sender, PaintEventArgs e)
        {
            G.Clear(Color.White);
            foreach (Edge edge in Edges)
            {
                edge.Accept(EdgeDrawingVisitor);
            }
            SelectedEdge?.Accept(EdgeDrawingVisitor);
            drawingPictureBox.Image = Bitmap;
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

        public void Visit(Edge edge)
        {
            //Pen pen = new Pen(Color.Black);
            //using Graphics g = Graphics.FromImage(Bitmap);

            //g.DrawLine(Pens.Black, startPoint, e.Location);
            G.DrawLine(Pens.Black, edge.Start.Position, edge.End.Position);
            //drawingPictureBox.Image = Bitmap;
        }

        public void Visit(HorizontalEdge edge)
        {
            // draw horizontal edge
        }

        public void Visit(VerticalEdge edge)
        {
            // draw vertical edge
        }

        public void Visit(FixedEdge edge)
        {
            // draw fixed edge
        }

        public void Visit(BezierEdge edge)
        {
            // draw bezier edge
        }
    }



    public class Vertex
    {
        public Point Position { get; set; }

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
    }

    public interface IEdgeVisitable
    {
        void Accept(IEdgeVisitor visitor);
    }

    public class Edge: IEdgeVisitable
    {
        public virtual Vertex Start { get; set; }
        public virtual Vertex End { get; set; }


        public Edge(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
        }


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
        public override bool IsHorizontal { get => true; }

        public HorizontalEdge(Vertex start, Vertex end) : base(start, end) { }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }

    public class VerticalEdge : Edge
    {
        public override bool IsVertical { get => true; }

        public VerticalEdge(Vertex start, Vertex end) : base(start, end) { }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }

    public class FixedEdge : Edge
    {
        public override bool IsFixed { get => true; }

        public FixedEdge(Vertex start, Vertex end) : base(start, end) { }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }

    public class BezierEdge : Edge
    {
        public override bool IsBezier { get => true; }

        public BezierEdge(Vertex start, Vertex end) : base(start, end) { }

        public override void Accept(IEdgeVisitor visitor) => visitor.Visit(this);
    }
}
