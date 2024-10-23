using Lab1.Edges;
using Lab1.Exceptions;
using Lab1.LineDrawers;
using Lab1.Visitors;
using System.Numerics;

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
        private Point? HoldPoint { get; set; }

        private EdgeDrawingVisitor EdgeDrawingVisitor { get; set; }
        //private IEdgeVisitor[] EdgeDrawingVisitors { get; }
        private ILineDrawer[] LineDrawers { get; }
        Graphics G { get; }
        private LengthDialogForm LengthDialogForm { get; set; } = new LengthDialogForm();


        public PolygonEditor()
        {
            InitializeComponent();

            //ContextMenuStrip = edgesContextMenuStrip;
            Bitmap = new Bitmap(drawingPictureBox.Width, drawingPictureBox.Height);
            drawingPictureBox.Image = Bitmap;
            G = Graphics.FromImage(Bitmap);
            G.Clear(Color.White);
            LineDrawers = [new DefaultLineDrawer(Bitmap), new BresenhamLineDrawer(Bitmap)];
            //ContextMenuStrip.Enabled = false;

            EdgeDrawingVisitor = new EdgeDrawingVisitor(LineDrawers[defaultRadioButton.Checked ? 0 : 1]);
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
                SelectedVertex = Edges.Find(edge => edge.Start.IsHit(e.Location))?.Start;
                if (SelectedVertex is not null)
                {
                    verticesContextMenuStrip.Show(drawingPictureBox, e.Location);
                    return;
                }

                SelectedEdge = Edges.Find(edge => edge.IsHit(e.Location));
                if (SelectedEdge is not null)
                {
                    //ContextMenuStrip!.Show(drawingPictureBox, e.Location);
                    //drawingPictureBox.ContextMenuStrip = edgesContextMenuStrip;
                    edgesContextMenuStrip.Items[1].Enabled = SelectedEdge.IsBasic;
                    edgesContextMenuStrip.Show(drawingPictureBox, e.Location);

                }
            }

        }


        private void drawingPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrawing)
            {
                DrawnEdge!.End.SetPosition(e.Location);
                //drawingPictureBox.Invalidate();
            }
            if (SelectedVertex is not null && e.Button == MouseButtons.Left)
            {
                try
                {
                    SelectedVertex.SetPosition(e.Location);
                    SelectedVertex.WasMoved = true;
                    SelectedVertex.InvokeStartPositionChanged();
                    SelectedVertex.InvokeEndPositionChanged();
                    foreach (Edge edge in Edges)
                        edge.OnMoved();
                    //ResetVertexMovementFlags();
                    //drawingPictureBox.Invalidate();
                }
                catch (VertexAlreadyMovedException)
                {
                    foreach (Edge edge in Edges)
                        edge.Restore();
                    MessageBox.Show("Wierzcho³ek nie mo¿e zostaæ przesuniêty ze wzglêdu na ograniczenia");
                }
                finally
                {
                    foreach (Edge edge in Edges)
                        edge.Start.ResetPreviousPosition();
                    ResetVertexMovementFlags();
                }
            }
            else if (HoldPoint is Point hp && e.Button == MouseButtons.Left)
            {
                int dx = e.Location.X - hp.X;
                int dy = e.Location.Y - hp.Y;
                foreach (Edge edge in Edges)
                {
                    //edge.Start.Position = new Point(edge.Start.Position.X + dx, edge.Start.Position.Y + dy);
                    //edge.End.Position = new Point(edge.End.Position.X + dx, edge.End.Position.Y + dy);
                    edge.MoveOwnedVertices(dx, dy);
                }

                HoldPoint = e.Location;

                foreach (Edge edge in Edges)
                    edge.OnMoved();
            }
            drawingPictureBox.Invalidate();

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
            //drawingPictureBox.Image = Bitmap;

            //foreach(Edge edge in Edges)
            //{
            //    if(edge is SpecialEdge ed)
            //        ed.RemoveConstraintButton.BringToFront();
            //}
        }

        private void drawingPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            SelectedVertex = null;

            foreach (Edge edge in Edges)
            {
                if (edge.TryGetHitOwnedVertex(e.Location, out Vertex? v))
                {
                    SelectedVertex = v;
                    break;
                }
            }
            HoldPoint = e.Location;
        }

        private void drawingPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                SelectedVertex = null;
            HoldPoint = null;
        }

        private void sta³aD³ugoœæToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Point p1 = SelectedEdge!.Start.Position;
            //Point p2 = SelectedEdge!.End.Position;
            //int deltaX = p2.X - p1.X;
            //int deltaY = p2.Y - p1.Y;
            LengthDialogForm.SetStartingValue((int) SelectedEdge!.Length);

            if (SelectedEdge!.IsBasic && LengthDialogForm.ShowDialog() == DialogResult.OK)
            {
                int length = LengthDialogForm.Length;
                SelectedEdge.UnsubscribeVertices();
                FixedEdge fixedEdge = new FixedEdge(SelectedEdge!.Start, SelectedEdge!.End, length);
                fixedEdge.RemoveConstraintButton.Click += removeConstraint!;
                fixedEdge.RemoveConstraintButton.Parent = drawingPictureBox;

                int index = Edges.FindIndex(edge => edge == SelectedEdge);
                Edges[index] = fixedEdge;
                ResetVertexMovementFlags();
            }
            else
            {
                MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe.");
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
        private void removeConstraint(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.Click -= removeConstraint!;
            drawingPictureBox.Controls.Remove(button);
            SpecialEdge edge = (SpecialEdge)button.Tag!;
            //button.Tag = null;
            //edge.RemoveConstraintButton = null;

            edge.UnsubscribeVertices();
            Edge newEdge = new Edge(edge.Start, edge.End);
            int index = Edges.FindIndex(e => e == edge);
            Edges[index] = newEdge;
        }

        private void pionowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = Edges.FindIndex(edge => edge == SelectedEdge);
            int leftIndex = index == 0 ? Edges.Count - 1 : index - 1;
            int rightIndex = index == Edges.Count - 1 ? 0 : index + 1;


            if (!SelectedEdge!.IsBasic)
                MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe.", "Nieprawid³owa operacja");
            else if (Edges[leftIndex].IsVertical || Edges[rightIndex].IsVertical)
                MessageBox.Show("Nie mo¿na ustawiæ ograniczenia pionowego dla dwóch s¹siednich krawêdzi");
            else if (SelectedEdge!.IsBasic)
            {
                SelectedEdge.UnsubscribeVertices();
                VerticalEdge verticalEdge = new VerticalEdge(SelectedEdge!.Start, SelectedEdge!.End);
                verticalEdge.RemoveConstraintButton.Click += removeConstraint!;
                verticalEdge.RemoveConstraintButton.Parent = drawingPictureBox;

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
                MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe.");
            else if (Edges[leftIndex].IsHorizontal || Edges[rightIndex].IsHorizontal)
                MessageBox.Show("Nie mo¿na ustawiæ ograniczenia poziomego dla dwóch s¹siednich krawêdzi");
            else if (SelectedEdge!.IsBasic)
            {
                SelectedEdge.UnsubscribeVertices();
                HorizontalEdge horizontalEdge = new HorizontalEdge(SelectedEdge!.Start, SelectedEdge!.End);
                horizontalEdge.RemoveConstraintButton.Click += removeConstraint!;
                horizontalEdge.RemoveConstraintButton.Parent = drawingPictureBox;
                //int index = Edges.FindIndex(edge => edge == SelectedEdge);
                Edges[index] = horizontalEdge;
                ResetVertexMovementFlags();
            }
            //else
            //{
            //    MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe");
            //}
        }

        private void beToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedEdge!.IsBasic)
            {
                SelectedEdge.UnsubscribeVertices();
                BezierEdge fixedEdge = new BezierEdge(SelectedEdge!.Start, SelectedEdge!.End);
                fixedEdge.RemoveConstraintButton.Click += removeConstraint!;
                fixedEdge.RemoveConstraintButton.Parent = drawingPictureBox;

                int index = Edges.FindIndex(edge => edge == SelectedEdge);
                Edges[index] = fixedEdge;
                ResetVertexMovementFlags();
            }
            else
            {
                MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe.");
            }
        }

        private void dodajWierzcho³ekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedEdge!.UnsubscribeVertices();
            if (SelectedEdge is SpecialEdge se)
            {
                Button button = se.RemoveConstraintButton;
                button.Click -= removeConstraint!;
                drawingPictureBox.Controls.Remove(button);
            }


            Vertex start = SelectedEdge!.Start;
            Vertex end = SelectedEdge!.End;
            Vertex middle = new Vertex((start.Position.X + end.Position.X) / 2, (start.Position.Y + end.Position.Y) / 2);

            Edge edge1 = new Edge(start, middle);
            Edge edge2 = new Edge(middle, end);

            int index = Edges.FindIndex(edge => edge == SelectedEdge);
            Edges.RemoveAt(index);
            Edges.InsertRange(index, [edge1, edge2]);
        }

        private void usuñWierczho³ekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index1 = Edges.FindIndex(edge => edge.End == SelectedVertex);
            int index2 = (index1 + 1) % Edges.Count;
            if (index1 != -1)
            {
                Edge edge1 = Edges[index1];
                Edge edge2 = Edges[index2];
                edge1.UnsubscribeVertices();
                edge2.UnsubscribeVertices();

                if (edge1 is SpecialEdge se1)
                {
                    Button button = se1.RemoveConstraintButton;
                    button.Click -= removeConstraint!;
                    drawingPictureBox.Controls.Remove(button);
                }
                if (edge2 is SpecialEdge se2)
                {
                    Button button = se2.RemoveConstraintButton;
                    button.Click -= removeConstraint!;
                    drawingPictureBox.Controls.Remove(button);
                }


                Edge edge = new Edge(edge1.Start, edge2.End);
                Edges[index1] = edge;
                Edges.RemoveAt(index2);
            }

        }

        private void defaultRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (defaultRadioButton.Checked)
            {
                EdgeDrawingVisitor.LineDrawer = LineDrawers[0];
            }
            else
            {
                EdgeDrawingVisitor.LineDrawer = LineDrawers[1];
            }
        }
    }
}
