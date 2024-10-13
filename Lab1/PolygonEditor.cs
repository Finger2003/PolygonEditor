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

        private EdgeDrawingVisitor EdgeDrawingVisitor { get; set; }
        //private IEdgeVisitor[] EdgeDrawingVisitors { get; }
        private ILineDrawer[] LineDrawers { get; }
        Graphics G { get; }


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
            if (SelectedVertex is not null && e.Button == MouseButtons.Left)
            {
                try
                {
                    SelectedVertex.Position = e.Location;
                    SelectedVertex.WasMoved = true;
                    SelectedVertex.NeighbourPositionChanged();
                    //ResetVertexMovementFlags();
                    drawingPictureBox.Invalidate();
                }
                catch (VertexAlreadyMovedException)
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
            if (e.Button == MouseButtons.Left)
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

        private void usuñWierczho³ekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index1 = Edges.FindIndex(edge => edge.End == SelectedVertex);
            int index2 = (index1 + 1) % Edges.Count;
            if (index1 != -1)
            {
                Edge edge1 = Edges[index1];
                Edge edge2 = Edges[index2];
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
