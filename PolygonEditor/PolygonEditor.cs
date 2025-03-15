using PolygonEditor;
using PolygonEditor.GeometryModel;
using PolygonEditor.GeometryModel.Edges;
using PolygonEditor.LineDrawers;
using PolygonEditor.Visitors.VoidVisitors;
using System.Resources;

namespace PolygonEditor
{
    public partial class PolygonEditor : Form
    {
        private Bitmap Bitmap { get; set; }
        private Vertex? SelectedVertex { get; set; }
        private Edge? SelectedEdge { get; set; }
        private Edge? DrawnEdge { get; set; }
        private bool IsDrawing { get; set; } = false;
        private Point? HoldPoint { get; set; }

        private EdgeDrawingVisitor EdgeDrawingVisitor { get; set; }
        private ILineDrawer[] LineDrawers { get; }
        Graphics G { get; set; }
        private LengthDialogForm LengthDialogForm { get; set; } = new LengthDialogForm();


        private int SelectedVertexIndex { get; set; } = -1;
        private int SelectedEdgeIndex { get; set; } = -1;

        private Vertex.ContinuityType DefaultContinuity { get; set; } = Vertex.ContinuityType.C1;
        private Polygon Polygon { get; }

        private string ConstraintErrorMessage { get; } = "Nowe ograniczenie nie mo¿e zostaæ dodane ze wzglêdu na pozosta³e ograniczenia.";


        public PolygonEditor()
        {
            InitializeComponent();

            PolygonExample.Init();
            Polygon = PolygonExample.GetPolygon()!;

            ResourceManager resourceManager = new("PolygonEditor.Properties.Resources", typeof(PolygonEditor).Assembly);
            Icon = resourceManager.GetObject("Icon") as Icon;

            drawingPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;


            Bitmap = new Bitmap(drawingPictureBox.Width, drawingPictureBox.Height);
            drawingPictureBox.Image = Bitmap;
            G = Graphics.FromImage(Bitmap);

            LineDrawers = [new DefaultLineDrawer(G), new BresenhamLineDrawer(Bitmap, G)];
            EdgeDrawingVisitor = new EdgeDrawingVisitor(LineDrawers[defaultRadioButton.Checked ? 0 : 1], G);

            PaintPictureBox();
        }

        private void drawingPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!IsDrawing && Polygon.Edges.Count == 0)
                {
                    DrawnEdge = new Edge(new Vertex(e.Location), new Vertex(e.Location));
                    IsDrawing = true;
                }
                else if (IsDrawing)
                {
                    Polygon.Edges.Add(DrawnEdge!);
                    if (Polygon.Edges[0].Start.IsHit(e.Location))
                    {
                        DrawnEdge!.End = Polygon.Edges[0].Start;
                        DrawnEdge = null;
                        IsDrawing = false;

                    }
                    else
                    {
                        DrawnEdge = new Edge(Polygon.Edges[^1].End, new Vertex(e.Location));
                    }
                }

            }
            else if (e.Button == MouseButtons.Right && !IsDrawing)
            {
                SelectedVertex = null;
                SelectedVertexIndex = Polygon.Edges.FindIndex(edge => edge.Start.IsHit(e.Location));
                if (SelectedVertexIndex >= 0)
                {
                    int previousIndex = Polygon.GetPreviousIndex(SelectedVertexIndex);
                    SelectedVertex = Polygon.Edges[SelectedVertexIndex].Start;
                    setContinuityInVertexToolStripMenuItem.Enabled = Polygon.Edges[SelectedVertexIndex].IsBezier || Polygon.Edges[previousIndex].IsBezier;

                    g0ToolStripMenuItem.Checked = SelectedVertex.Continuity == Vertex.ContinuityType.G0;
                    g1ToolStripMenuItem.Checked = SelectedVertex.Continuity == Vertex.ContinuityType.G1;
                    c1ToolStripMenuItem.Checked = SelectedVertex.Continuity == Vertex.ContinuityType.C1;

                    verticesContextMenuStrip.Show(drawingPictureBox, e.Location);
                    return;
                }

                SelectedEdge = null;
                SelectedEdgeIndex = Polygon.Edges.FindIndex(edge => edge.IsHit(e.Location));
                if (SelectedEdgeIndex >= 0)
                {
                    SelectedEdge = Polygon.Edges[SelectedEdgeIndex];
                    addConstraintToolStripMenuItem.Enabled = SelectedEdge.IsBasic;
                    removeConstraintToolStripMenuItem.Enabled = !SelectedEdge.IsBasic;
                    edgesContextMenuStrip.Show(drawingPictureBox, e.Location);

                }
            }

        }


        private void drawingPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrawing)
            {
                DrawnEdge!.End.SetPosition(e.Location);
            }
            if (SelectedVertex is not null && e.Button == MouseButtons.Left)
            {
                Polygon.SetVertexPosition(SelectedVertexIndex, SelectedVertex, e.Location.X, e.Location.Y);
            }
            else if (HoldPoint is Point hp && e.Button == MouseButtons.Right)
            {
                int dx = e.Location.X - hp.X;
                int dy = e.Location.Y - hp.Y;
                Polygon.Move(dx, dy);

                HoldPoint = e.Location;
            }
            drawingPictureBox.Invalidate();

        }



        private void drawingPictureBox_Paint(object sender, PaintEventArgs e)
        {
            PaintPictureBox();
        }

        private void PaintPictureBox()
        {
            G.Clear(Color.White);
            foreach (Edge edge in Polygon.Edges)
            {
                edge.Accept(EdgeDrawingVisitor);
                G.FillEllipse(Brushes.Black, edge.Start.Position.X - 5, edge.Start.Position.Y - 5, 10, 10);
            }
            DrawnEdge?.Accept(EdgeDrawingVisitor);
            if (DrawnEdge is not null)
            {
                G.FillEllipse(Brushes.Black, DrawnEdge.Start.Position.X - 5, DrawnEdge.Start.Position.Y - 5, 10, 10);
            }
        }

        private void drawingPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            SelectedVertexIndex = -1;
            SelectedVertex = null;

            (SelectedVertexIndex, SelectedVertex) = Polygon.GetHitVertexWithIndex(e.X, e.Y);

            HoldPoint = e.Location;
        }

        private void drawingPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SelectedEdgeIndex = -1;
                SelectedVertex = null;
            }
            HoldPoint = null;
        }



        private bool IsEdgeVertical(Edge edge) => edge.IsVertical;
        private bool IsEdgeHorizontal(Edge edge) => edge.IsHorizontal;
        private void SetVerticalOrHorizontalEdge(int index, Edge edge, string orientationNameForErrorMessage, Func<Edge, bool> orientationPredicate, Func<int, Edge, bool> settingFunc)
        {
            int leftIndex = Polygon.GetPreviousIndex(index);
            int rightIndex = Polygon.GetNextIndex(index);

            if (orientationPredicate(Polygon.Edges[leftIndex]) || orientationPredicate(Polygon.Edges[rightIndex]))
                MessageBox.Show($"Nie mo¿na ustawiæ ograniczenia {orientationNameForErrorMessage} dla dwóch s¹siednich krawêdzi");
            else
            {
                if (!settingFunc(index, edge))
                {
                    MessageBox.Show(ConstraintErrorMessage);
                }
            }
        }




        private void defaultRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (defaultRadioButton.Checked)
            {
                EdgeDrawingVisitor.LineDrawer = LineDrawers[0];
            }
        }
        private void bresenhamRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (bresenhamRadioButton.Checked)
            {
                EdgeDrawingVisitor.LineDrawer = LineDrawers[1];
            }
        }


        private void deleteVertexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Polygon.DeleteVertex(SelectedVertexIndex, SelectedVertex!);
        }

        private void g0ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SetContinuity(SelectedVertexIndex, SelectedVertex!, Vertex.ContinuityType.G0);
        }

        private void g1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetContinuity(SelectedVertexIndex, SelectedVertex!, Vertex.ContinuityType.G1);
        }

        private void c1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetContinuity(SelectedVertexIndex, SelectedVertex!, Vertex.ContinuityType.C1);
        }

        private void SetContinuity(int index, Vertex vertex, Vertex.ContinuityType contuinity)
        {
            if (!Polygon.TrySetContinuityInVertex(index, vertex, contuinity))
            {
                MessageBox.Show($"Nie mo¿na ustawiæ ograniczenia {contuinity} dla tego wierzcho³ka");
            }
        }
        private void removeConstraintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Polygon.RemoveConstraint(SelectedEdgeIndex, SelectedEdge!);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.Show();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Polygon.Edges.Clear();
            PaintPictureBox();
        }

        private void addVertexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Polygon.AddVertexInEdge(SelectedEdgeIndex, SelectedEdge!);
        }

        private void fixedLengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LengthDialogForm.SetStartingValue((int)Math.Round(SelectedEdge!.Length));

            if (LengthDialogForm.ShowDialog() == DialogResult.OK)
            {
                if (!Polygon.TrySetFixedEdge(SelectedEdgeIndex, SelectedEdge, LengthDialogForm.Length))
                {
                    MessageBox.Show(ConstraintErrorMessage);
                }
            }
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetVerticalOrHorizontalEdge(SelectedEdgeIndex, SelectedEdge!, "pionowego", IsEdgeVertical, Polygon.TrySetVerticalEdge);
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetVerticalOrHorizontalEdge(SelectedEdgeIndex, SelectedEdge!, "poziomego", IsEdgeHorizontal, Polygon.TrySetHorizontalEdge);
        }

        private void bezierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Polygon.TrySetBezierCurve(SelectedEdgeIndex, SelectedEdge!))
            {
                MessageBox.Show(ConstraintErrorMessage);
            }
        }


        private void drawingPictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (drawingPictureBox.Height <= 0 || drawingPictureBox.Width <= 0)
                return;

            Bitmap oldBitmap = Bitmap;
            Graphics oldGraphics = G;

            Bitmap = new Bitmap(drawingPictureBox.Width, drawingPictureBox.Height);
            G = Graphics.FromImage(Bitmap);
            drawingPictureBox.Image = Bitmap;
            G.DrawImage(oldBitmap, 0, 0);


            LineDrawers[0] = new DefaultLineDrawer(G);
            LineDrawers[1] = new BresenhamLineDrawer(Bitmap, G);
            EdgeDrawingVisitor = new EdgeDrawingVisitor(LineDrawers[defaultRadioButton.Checked ? 0 : 1], G);

            oldGraphics.Dispose();
            oldBitmap.Dispose();

            drawingPictureBox.Invalidate();
        }
    }
}
