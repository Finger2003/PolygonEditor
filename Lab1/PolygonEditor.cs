using Lab1.Exceptions;
using Lab1.GeometryModel;
using Lab1.GeometryModel.Edges;
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


        private int SelectedVertexIndex { get; set; } = -1;
        private int SelectedEdgeIndex { get; set; } = -1;

        private Vertex.ContuinityType DefaultContuinity { get; set; } = Vertex.ContuinityType.C1;


        public PolygonEditor()
        {
            InitializeComponent();

            Bitmap = new Bitmap(drawingPictureBox.Width, drawingPictureBox.Height);
            drawingPictureBox.Image = Bitmap;
            G = Graphics.FromImage(Bitmap);
            G.Clear(Color.White);
            LineDrawers = [new DefaultLineDrawer(G), new BresenhamLineDrawer(Bitmap, G)];

            EdgeDrawingVisitor = new EdgeDrawingVisitor(LineDrawers[defaultRadioButton.Checked ? 0 : 1], G);
        }

        private void drawingPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!IsDrawing && Edges.Count == 0)
                {
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

            }
            else if (e.Button == MouseButtons.Right && !IsDrawing)
            {
                SelectedVertex = null;
                SelectedVertexIndex = Edges.FindIndex(edge => edge.Start.IsHit(e.Location));
                if (SelectedVertexIndex >= 0)
                {
                    int previousIndex = SelectedVertexIndex == 0 ? Edges.Count - 1 : SelectedVertexIndex - 1;
                    SelectedVertex = Edges[SelectedVertexIndex].Start;
                    setContinuityInVertexToolStripMenuItem.Enabled = Edges[SelectedVertexIndex].IsBezier || Edges[previousIndex].IsBezier;
                    g0ToolStripMenuItem.Checked = SelectedVertex.Continuity == Vertex.ContuinityType.G0;
                    g1ToolStripMenuItem.Checked = SelectedVertex.Continuity == Vertex.ContuinityType.G1;
                    c1ToolStripMenuItem.Checked = SelectedVertex.Continuity == Vertex.ContuinityType.C1;
                    verticesContextMenuStrip.Show(drawingPictureBox, e.Location);
                    return;
                }

                SelectedEdge = null;
                SelectedEdgeIndex = Edges.FindIndex(edge => edge.IsHit(e.Location));
                if (SelectedEdgeIndex >= 0)
                {
                    SelectedEdge = Edges[SelectedEdgeIndex];
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
                ResetVerticesPreviousPositions();
                ResetVertexMovementFlags();
                SelectedVertex.SetPosition(e.Location);
                Vector2 delta = SelectedVertex.PositionDifference;
                try
                {


                    SelectedVertex.WasMoved = true;
                    int selectedIndex = SelectedVertexIndex;
                    Edge selectedVertexOwner = Edges[selectedIndex];

                    int startingIndexForward = selectedIndex;
                    int startingIndexBackward = selectedIndex - 1;
                    if (selectedVertexOwner.IsBezier)
                    {
                        if (selectedVertexOwner.IsControlVertex(SelectedVertex))
                        {
                            selectedVertexOwner.SetVerticesContinuityRelevantProperties(SelectedVertex);
                            ++startingIndexForward;
                        }
                        else
                        {
                            int index = selectedIndex == 0 ? Edges.Count - 1 : selectedIndex - 1;
                            Edge previousEdge = Edges[index];
                            previousEdge.CorrectStartPositionBasically();
                            previousEdge.SetVerticesContinuityRelevantProperties(SelectedVertex);
                            --startingIndexBackward;
                        }
                    }
                    else
                    {
                        selectedVertexOwner.CorrectEndPositionBasically();
                        ++startingIndexForward;
                        selectedVertexOwner.SetVerticesContinuityRelevantProperties(SelectedVertex);
                    }


                    bool correctionSucceeded = correctEdges(startingIndexForward, startingIndexBackward);

                    if (!correctionSucceeded)
                    {
                        foreach (Edge edge in Edges)
                            edge.Restore();

                        foreach (Edge edge in Edges)
                        {
                            edge.MoveOwnedVertices(delta.X, delta.Y);
                        }
                    }
                }
                catch (VertexCannotBeMoved)
                {

                    foreach (Edge edge in Edges)
                        edge.Restore();

                    foreach (Edge edge in Edges)
                    {
                        edge.MoveOwnedVertices(delta.X, delta.Y);
                    }
                }
                finally
                {
                    ResetVerticesPreviousPositions();
                    ResetVertexMovementFlags();
                }
            }
            else if (HoldPoint is Point hp && e.Button == MouseButtons.Right)
            {
                int dx = e.Location.X - hp.X;
                int dy = e.Location.Y - hp.Y;
                foreach (Edge edge in Edges)
                {
                    edge.MoveOwnedVertices(dx, dy);
                }

                HoldPoint = e.Location;
            }
            drawingPictureBox.Invalidate();

        }

        private void ResetVerticesPreviousPositions()
        {
            foreach (Edge edge in Edges)
            {
                edge.ResetOwnedMovedVerticesPreviousPositions();
            }
        }

        private bool correctEdges(int startingIndexForward, int startingIndexBackward)
        {
            int index = startingIndexForward;
            int i = 0;
            Edge.correctingStatus correctingStatus;
            do
            {
                if (index >= Edges.Count)
                {
                    index = 0;
                }
                correctingStatus = Edges[index++].CorrectEndPosition();

            } while (correctingStatus == Edge.correctingStatus.FurtherCorrectionNeeded && i++ < Edges.Count);

            if (correctingStatus == Edge.correctingStatus.CorrectionFailed)
                return false;

            index = startingIndexBackward;
            i = 0;
            do
            {
                if (index < 0)
                    index = Edges.Count - 1;
                correctingStatus = Edges[index--].CorrectStartPosition();
            } while (correctingStatus == Edge.correctingStatus.FurtherCorrectionNeeded && i++ < Edges.Count);

            return correctingStatus != Edge.correctingStatus.CorrectionFailed;
        }

        private void drawingPictureBox_Paint(object sender, PaintEventArgs e)
        {
            G.Clear(Color.White);
            foreach (Edge edge in Edges)
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

            foreach ((Edge edge, int index) in Edges.Select((edge, index) => (edge, index)))
            {
                if (edge.TryGetHitOwnedVertex(e.Location, out Vertex? v))
                {
                    SelectedVertexIndex = index;
                    SelectedVertex = v;
                    break;
                }
            }
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

        private void sta³aD³ugoœæToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LengthDialogForm.SetStartingValue((int)Math.Round(SelectedEdge!.Length));

            if (SelectedEdge!.IsBasic && LengthDialogForm.ShowDialog() == DialogResult.OK)
            {
                ResetVerticesPreviousPositions();
                int length = LengthDialogForm.Length;
                FixedEdge fixedEdge = new FixedEdge(SelectedEdge!.Start, SelectedEdge!.End, length);
                fixedEdge.SetVerticesContinuityRelevantProperties(SelectedEdge!.Start);
                fixedEdge.SetVerticesContinuityRelevantProperties(SelectedEdge!.End);


                int selectedIndex = SelectedEdgeIndex;
                Edges[selectedIndex] = fixedEdge;
                try
                {

                    correctEdges(selectedIndex + 1, selectedIndex - 1);

                }
                catch (VertexCannotBeMoved)
                {
                    Edges[selectedIndex] = SelectedEdge!;
                    foreach (Edge edge in Edges)
                    {
                        edge.Restore();
                    }
                    MessageBox.Show("Nowe ograniczenie nie mo¿e zostaæ dodane ze wzglêdu na pozosta³e ograniczenia.");
                }

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
                edge.ResetOwnedVerticesMovementFlags();
            }
        }
        private void pionowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedIndex = SelectedEdgeIndex;
            int leftIndex = selectedIndex == 0 ? Edges.Count - 1 : selectedIndex - 1;
            int rightIndex = selectedIndex == Edges.Count - 1 ? 0 : selectedIndex + 1;


            if (!SelectedEdge!.IsBasic)
                MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe.", "Nieprawid³owa operacja");
            else if (Edges[leftIndex].IsVertical || Edges[rightIndex].IsVertical)
                MessageBox.Show("Nie mo¿na ustawiæ ograniczenia pionowego dla dwóch s¹siednich krawêdzi");
            else if (SelectedEdge!.IsBasic)
            {
                ResetVerticesPreviousPositions();
                VerticalEdge verticalEdge = new VerticalEdge(SelectedEdge!.Start, SelectedEdge!.End);
                verticalEdge.SetVerticesContinuityRelevantProperties(SelectedEdge!.Start);
                verticalEdge.SetVerticesContinuityRelevantProperties(SelectedEdge!.End);


                Edges[selectedIndex] = verticalEdge;

                try
                {

                    correctEdges(selectedIndex + 1, selectedIndex - 1);

                }
                catch (VertexCannotBeMoved)
                {
                    Edges[selectedIndex] = SelectedEdge!;
                    foreach (Edge edge in Edges)
                    {
                        edge.Restore();
                    }
                    MessageBox.Show("Nowe ograniczenie nie mo¿e zostaæ dodane ze wzglêdu na pozosta³e ograniczenia.");
                }

                ResetVertexMovementFlags();
            }
        }

        private void poziomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedIndex = Edges.FindIndex(edge => edge == SelectedEdge);
            int leftIndex = selectedIndex == 0 ? Edges.Count - 1 : selectedIndex - 1;
            int rightIndex = selectedIndex == Edges.Count - 1 ? 0 : selectedIndex + 1;


            if (!SelectedEdge!.IsBasic)
                MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe.");
            else if (Edges[leftIndex].IsHorizontal || Edges[rightIndex].IsHorizontal)
                MessageBox.Show("Nie mo¿na ustawiæ ograniczenia poziomego dla dwóch s¹siednich krawêdzi");
            else if (SelectedEdge!.IsBasic)
            {
                ResetVerticesPreviousPositions();
                HorizontalEdge horizontalEdge = new HorizontalEdge(SelectedEdge!.Start, SelectedEdge!.End);
                horizontalEdge.SetVerticesContinuityRelevantProperties(SelectedEdge!.Start);
                horizontalEdge.SetVerticesContinuityRelevantProperties(SelectedEdge!.End);

                Edges[selectedIndex] = horizontalEdge;
                try
                {

                    correctEdges(selectedIndex + 1, selectedIndex - 1);

                }
                catch (VertexCannotBeMoved)
                {
                    Edges[selectedIndex] = SelectedEdge!;
                    foreach (Edge edge in Edges)
                    {
                        edge.Restore();
                    }
                    MessageBox.Show("Nowe ograniczenie nie mo¿e zostaæ dodane ze wzglêdu na pozosta³e ograniczenia.");
                }

                ResetVertexMovementFlags();
            }

        }

        private void beToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedEdge!.IsBasic)
            {
                ResetVerticesPreviousPositions();
                if (SelectedEdge!.Start.Continuity == Vertex.ContuinityType.G0)
                {
                    SelectedEdge!.Start.Continuity = DefaultContuinity;
                }
                if (SelectedEdge!.End.Continuity == Vertex.ContuinityType.G0)
                {
                    SelectedEdge!.End.Continuity = DefaultContuinity;
                }

                BezierEdge bezierEdge = new BezierEdge(SelectedEdge!.Start, SelectedEdge!.End);



                int selectedIndex = SelectedEdgeIndex;
                int previousIndex = selectedIndex == 0 ? Edges.Count - 1 : selectedIndex - 1;
                int nextIndex = selectedIndex == Edges.Count - 1 ? 0 : selectedIndex + 1;
                Edges[previousIndex].SetVerticesContinuityRelevantProperties(SelectedEdge!.Start);
                Edges[nextIndex].SetVerticesContinuityRelevantProperties(SelectedEdge!.End);


                Edges[selectedIndex] = bezierEdge;

                try
                {


                    correctEdges(selectedIndex, selectedIndex);


                }
                catch (VertexCannotBeMoved)
                {
                    Edges[selectedIndex] = SelectedEdge!;
                    foreach (Edge edge in Edges)
                    {
                        edge.Restore();
                    }
                    MessageBox.Show("Nowe ograniczenie nie mo¿e zostaæ dodane ze wzglêdu na pozosta³e ograniczenia.");
                }



                ResetVertexMovementFlags();
            }
            else
            {
                MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe.");
            }
        }

        private void dodajWierzcho³ekToolStripMenuItem_Click(object sender, EventArgs e)
        {


            Vertex start = SelectedEdge!.Start;
            Vertex end = SelectedEdge!.End;
            Vertex middle = new Vertex((start.X + end.X) / 2, (start.Y + end.Y) / 2);

            Edge edge1 = new Edge(start, middle);
            Edge edge2 = new Edge(middle, end);

            //int index = Edges.FindIndex(edge => edge == SelectedEdge);
            int index = SelectedEdgeIndex;
            int previousIndex = index == 0 ? Edges.Count - 1 : index - 1;
            int nextIndex = index == Edges.Count - 1 ? 0 : index + 1;

            if (!Edges[previousIndex].IsBezier)
            {
                start.Continuity = Vertex.ContuinityType.G0;
            }
            if (!Edges[nextIndex].IsBezier)
            {
                end.Continuity = Vertex.ContuinityType.G0;
            }

            Edges.RemoveAt(index);
            Edges.InsertRange(index, [edge1, edge2]);
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


        private void deleteVertexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Edges.Count <= 3)
            {
                Edges.Clear();
                return;
            }

            int selectedIndex = SelectedVertexIndex;
            int previousIndex = selectedIndex == 0 ? Edges.Count - 1 : selectedIndex - 1;
            int previousIndex2 = previousIndex == 0 ? Edges.Count - 1 : previousIndex - 1;
            int nextIndex1 = selectedIndex == Edges.Count - 1 ? 0 : selectedIndex + 1;
            if (!Edges[previousIndex2].IsBezier)
            {
                Edges[previousIndex2].End.Continuity = Vertex.ContuinityType.G0;
            }
            if (!Edges[nextIndex1].IsBezier)
            {
                Edges[nextIndex1].Start.Continuity = Vertex.ContuinityType.G0;
            }
            if (selectedIndex != -1)
            {
                Edge edge1 = Edges[previousIndex];
                Edge edge2 = Edges[selectedIndex];


                Edge edge = new Edge(edge1.Start, edge2.End);
                Edges[previousIndex] = edge;
                Edges.RemoveAt(selectedIndex);
            }
        }

        private void g0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectedVertex!.Continuity = Vertex.ContuinityType.G0;
        }

        private void g1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetVerticesPreviousPositions();
            SelectedVertex!.Continuity = Vertex.ContuinityType.G1;
            SelectedVertex!.ContinuityChanged = true;
            int previousIndex = SelectedVertexIndex == 0 ? Edges.Count - 1 : SelectedVertexIndex - 1;
            Edges[previousIndex].SetVerticesContinuityRelevantProperties(SelectedVertex);
            correctEdges(SelectedVertexIndex, SelectedVertexIndex - 1);
            SelectedVertex!.ContinuityChanged = false;
            ResetVertexMovementFlags();
        }

        private void c1ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ResetVerticesPreviousPositions();
            SelectedVertex!.Continuity = Vertex.ContuinityType.C1;
            SelectedVertex!.ContinuityChanged = true;
            int previousIndex = SelectedVertexIndex == 0 ? Edges.Count - 1 : SelectedVertexIndex - 1;
            Edges[previousIndex].SetVerticesContinuityRelevantProperties(SelectedVertex);
            correctEdges(SelectedVertexIndex, SelectedVertexIndex - 1);
            SelectedVertex!.ContinuityChanged = false;
            ResetVertexMovementFlags();
        }

        private void removeConstraintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = SelectedEdgeIndex;
            int previousIndex = index == 0 ? Edges.Count - 1 : index - 1;
            int nextIndex = index == Edges.Count - 1 ? 0 : index + 1;
            if (!Edges[previousIndex].IsBezier)
            {
                Edges[previousIndex].End.Continuity = Vertex.ContuinityType.G0;
            }
            if (!Edges[nextIndex].IsBezier)
            {
                Edges[nextIndex].Start.Continuity = Vertex.ContuinityType.G0;
            }
            Edge newEdge = new Edge(SelectedEdge!.Start, SelectedEdge.End);
            Edges[index] = newEdge;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.Show();
        }
    }
}
