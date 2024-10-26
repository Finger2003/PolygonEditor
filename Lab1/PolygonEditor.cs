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


        private int SelectedVertexIndex { get; set; } = -1;
        private int SelectedEdgeIndex { get; set; } = -1;


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
                SelectedVertex = null;
                SelectedVertexIndex = Edges.FindIndex(edge => edge.Start.IsHit(e.Location));
                //SelectedVertex = Edges.Find(edge => edge.Start.IsHit(e.Location))?.Start;
                if (SelectedVertexIndex >= 0)
                {
                    SelectedVertex = Edges[SelectedVertexIndex].Start;
                    verticesContextMenuStrip.Show(drawingPictureBox, e.Location);
                    return;
                }

                SelectedEdge = null;
                SelectedEdgeIndex = Edges.FindIndex(edge => edge.IsHit(e.Location));
                //SelectedEdge = Edges.Find(edge => edge.IsHit(e.Location));
                if (SelectedEdgeIndex >= 0)
                {
                    SelectedEdge = Edges[SelectedEdgeIndex];
                    //ContextMenuStrip!.Show(drawingPictureBox, e.Location);
                    //drawingPictureBox.ContextMenuStrip = edgesContextMenuStrip;
                    dodajOgraniczenieToolStripMenuItem.Enabled = SelectedEdge.IsBasic;
                    //edgesContextMenuStrip.Items[1].Enabled = SelectedEdge.IsBasic;
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

                    //if(SelectedVertex)

                    //int selectedIndex = SelectedVertexIndex;
                    //int index = selectedIndex;
                    //int i = 0;
                    //do
                    //{

                    //} while (Edges[index++ % Edges.Count].CorrectEndPosition() && i++ < Edges.Count);

                    ////i = index - 1;
                    //index = selectedIndex - 1;
                    //i = 0;
                    //do
                    //{
                    //    if(index < 0)
                    //        index = Edges.Count - 1;
                    //} while(Edges[index--].CorrectStartPosition() && i++ < Edges.Count);

                    correctEdges(startingIndexForward, startingIndexBackward);


                    //SelectedVertex.InvokeStartPositionChanged();
                    //SelectedVertex.InvokeEndPositionChanged();
                    foreach (Edge edge in Edges)
                        edge.OnMoved();
                    //ResetVertexMovementFlags();
                    //drawingPictureBox.Invalidate();
                }
                catch (VertexCannotBeMoved)
                {
                    //Vector2 delta = SelectedVertex.PositionDifference;

                    foreach (Edge edge in Edges)
                        edge.Restore();

                    foreach (Edge edge in Edges)
                    {
                        //edge.Start.Position = new Point(edge.Start.Position.X + dx, edge.Start.Position.Y + dy);
                        //edge.End.Position = new Point(edge.End.Position.X + dx, edge.End.Position.Y + dy);
                        edge.MoveOwnedVertices(delta.X, delta.Y);
                    }
                    //MessageBox.Show("Wierzcho³ek nie mo¿e zostaæ przesuniêty ze wzglêdu na ograniczenia");
                }
                finally
                {
                    //foreach (Edge edge in Edges)
                    //    edge.Start.ResetPreviousPosition();
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

        private void ResetVerticesPreviousPositions()
        {
            foreach (Edge edge in Edges)
            {
                edge.ResetOwnedMovedVerticesPreviousPositions();
            }
        }

        private void correctEdges(int startingIndexForward, int startingIndexBackward)
        {
            int index = startingIndexForward;
            int i = 0;
            do
            {
                if (index >= Edges.Count)
                {
                    index = 0;
                }

            } while (Edges[index++].CorrectEndPosition() && i++ < Edges.Count);

            index = startingIndexBackward;
            i = 0;
            do
            {
                if (index < 0)
                    index = Edges.Count - 1;
            } while (Edges[index--].CorrectStartPosition() && i++ < Edges.Count);
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
            //Point p1 = SelectedEdge!.Start.Position;
            //Point p2 = SelectedEdge!.End.Position;
            //int deltaX = p2.X - p1.X;
            //int deltaY = p2.Y - p1.Y;
            LengthDialogForm.SetStartingValue((int)SelectedEdge!.Length);

            if (SelectedEdge!.IsBasic && LengthDialogForm.ShowDialog() == DialogResult.OK)
            {
                int length = LengthDialogForm.Length;
                //SelectedEdge.UnsubscribeVertices();
                FixedEdge fixedEdge = new FixedEdge(SelectedEdge!.Start, SelectedEdge!.End, length);

                ResetVerticesPreviousPositions();
                //int index = Edges.FindIndex(edge => edge == SelectedEdge);
                int selectedIndex = SelectedEdgeIndex;
                Edges[selectedIndex] = fixedEdge;
                try
                {
                    //int index = selectedIndex + 1;
                    //int i = 0;
                    //do
                    //{

                    //} while (Edges[index++ % Edges.Count].CorrectEndPosition() && i++ < Edges.Count);

                    ////i = index - 1;
                    //index = selectedIndex - 1;
                    //i = 0;
                    //do
                    //{
                    //    if (index < 0)
                    //        index = Edges.Count - 1;
                    //} while (Edges[index--].CorrectStartPosition() && i++ < Edges.Count);
                    
                    correctEdges(selectedIndex + 1, selectedIndex - 1);

                    fixedEdge.RemoveConstraintButton.Click += removeConstraint!;
                    fixedEdge.RemoveConstraintButton.Parent = drawingPictureBox;
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
                //edge.Start.WasMoved = false;
                //edge.Start.WasChecked = false;
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

            //edge.UnsubscribeVertices();
            Edge newEdge = new Edge(edge.Start, edge.End);
            int index = Edges.FindIndex(e => e == edge);
            Edges[index] = newEdge;
        }

        private void pionowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //int index = Edges.FindIndex(edge => edge == SelectedEdge);
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
                //SelectedEdge.UnsubscribeVertices();
                VerticalEdge verticalEdge = new VerticalEdge(SelectedEdge!.Start, SelectedEdge!.End);



                //int selectedIndex = SelectedEdgeIndex;
                Edges[selectedIndex] = verticalEdge;

                try
                {
                    //int index = selectedIndex + 1;
                    //int i = 0;
                    //do
                    //{

                    //} while (Edges[index++ % Edges.Count].CorrectEndPosition() && i++ < Edges.Count);

                    ////i = index - 1;
                    //index = selectedIndex - 1;
                    //i = 0;
                    //do
                    //{
                    //    if (index < 0)
                    //        index = Edges.Count - 1;
                    //} while (Edges[index--].CorrectStartPosition() && i++ < Edges.Count);

                    correctEdges(selectedIndex + 1, selectedIndex - 1);

                    verticalEdge.RemoveConstraintButton.Click += removeConstraint!;
                    verticalEdge.RemoveConstraintButton.Parent = drawingPictureBox;
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

                //Edges[index] = verticalEdge;
                ResetVertexMovementFlags();
            }
            //else
            //{
            //    MessageBox.Show("KrawêdŸ mo¿e mieæ tylko jedno ograniczenie. Spróbuj usun¹æ aktualne ograniczenie, a nastêpnie ustawiæ nowe");
            //}
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
                SelectedEdge.UnsubscribeVertices();
                HorizontalEdge horizontalEdge = new HorizontalEdge(SelectedEdge!.Start, SelectedEdge!.End);

                //int index = Edges.FindIndex(edge => edge == SelectedEdge);

                Edges[selectedIndex] = horizontalEdge;
                try
                {
                    //int index = selectedIndex + 1;
                    //int i = 0;
                    //do
                    //{

                    //} while (Edges[index++ % Edges.Count].CorrectEndPosition() && i++ < Edges.Count);

                    ////i = index - 1;
                    //index = selectedIndex - 1;
                    //i = 0;
                    //do
                    //{
                    //    if (index < 0)
                    //        index = Edges.Count - 1;
                    //} while (Edges[index--].CorrectStartPosition() && i++ < Edges.Count);

                    correctEdges(selectedIndex + 1, selectedIndex - 1);

                    horizontalEdge.RemoveConstraintButton.Click += removeConstraint!;
                    horizontalEdge.RemoveConstraintButton.Parent = drawingPictureBox;
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

                //Edges[index] = horizontalEdge;
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
                ResetVerticesPreviousPositions();
                SelectedEdge.UnsubscribeVertices();
                BezierEdge fixedEdge = new BezierEdge(SelectedEdge!.Start, SelectedEdge!.End);


                int selectedIndex = SelectedEdgeIndex;
                Edges[selectedIndex] = fixedEdge;

                try
                {
                    //int index = selectedIndex + 1;
                    //int i = 0;
                    //do
                    //{

                    //} while (Edges[index++ % Edges.Count].CorrectEndPosition() && i++ < Edges.Count);

                    ////i = index - 1;
                    //index = selectedIndex - 1;
                    //i = 0;
                    //do
                    //{
                    //    if (index < 0)
                    //        index = Edges.Count - 1;
                    //} while (Edges[index--].CorrectStartPosition() && i++ < Edges.Count);

                    correctEdges(selectedIndex + 1, selectedIndex - 1);

                    fixedEdge.RemoveConstraintButton.Click += removeConstraint!;
                    fixedEdge.RemoveConstraintButton.Parent = drawingPictureBox;
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


                //int index = Edges.FindIndex(edge => edge == SelectedEdge);
                //Edges[index] = fixedEdge;
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
            Vertex middle = new Vertex((start.X + end.X) / 2, (start.Y + end.Y) / 2);

            Edge edge1 = new Edge(start, middle);
            Edge edge2 = new Edge(middle, end);

            //int index = Edges.FindIndex(edge => edge == SelectedEdge);
            Edges.RemoveAt(SelectedEdgeIndex);
            Edges.InsertRange(SelectedEdgeIndex, [edge1, edge2]);
        }

        //private void usuñWierczho³ekToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    //int index1 = Edges.FindIndex(edge => edge.End == SelectedVertex);
        //    int index1 = SelectedEdgeIndex;
        //    int index2 = (index1 + 1) % Edges.Count;
        //    if (index1 != -1)
        //    {
        //        Edge edge1 = Edges[index1];
        //        Edge edge2 = Edges[index2];
        //        //edge1.UnsubscribeVertices();
        //        //edge2.UnsubscribeVertices();

        //        if (edge1 is SpecialEdge se1)
        //        {
        //            Button button = se1.RemoveConstraintButton;
        //            button.Click -= removeConstraint!;
        //            drawingPictureBox.Controls.Remove(button);
        //        }
        //        if (edge2 is SpecialEdge se2)
        //        {
        //            Button button = se2.RemoveConstraintButton;
        //            button.Click -= removeConstraint!;
        //            drawingPictureBox.Controls.Remove(button);
        //        }


        //        Edge edge = new Edge(edge1.Start, edge2.End);
        //        Edges[index1] = edge;
        //        Edges.RemoveAt(index2);
        //    }

        //}

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
            //int index1 = Edges.FindIndex(edge => edge.End == SelectedVertex);
            int index1 = SelectedVertexIndex;
            int index2 = SelectedVertexIndex == 0 ? Edges.Count - 1 : SelectedVertexIndex - 1;
            if (index1 != -1)
            {
                Edge edge1 = Edges[index1];
                Edge edge2 = Edges[index2];
                //edge1.UnsubscribeVertices();
                //edge2.UnsubscribeVertices();

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

        private void g0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //g0ToolStripMenuItem.Checked = true;
            //g1ToolStripMenuItem.Checked = false;
            //c1ToolStripMenuItem.Checked = false;
            SelectedVertex!.Continuity = Vertex.ContuinityType.G0;
        }

        private void g1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //g0ToolStripMenuItem.Checked = false;
            //g1ToolStripMenuItem.Checked = true;
            //c1ToolStripMenuItem.Checked = false;
            ResetVerticesPreviousPositions();
            SelectedVertex!.Continuity = Vertex.ContuinityType.G1;
            SelectedVertex!.ContinuityChanged = true;
            correctEdges(SelectedVertexIndex, SelectedVertexIndex - 1);
            SelectedVertex!.ContinuityChanged = false;
            ResetVertexMovementFlags();
        }

        private void c1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //g0ToolStripMenuItem.Checked = false;
            //g1ToolStripMenuItem.Checked = false;
            //c1ToolStripMenuItem.Checked = true;
            ResetVerticesPreviousPositions();
            SelectedVertex!.Continuity = Vertex.ContuinityType.C1;
            SelectedVertex!.ContinuityChanged = true;
            correctEdges(SelectedVertexIndex, SelectedVertexIndex - 1);
            SelectedVertex!.ContinuityChanged = false;
            ResetVertexMovementFlags();
        }
    }
}
