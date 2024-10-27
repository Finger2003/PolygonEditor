using Lab1.GeometryModel;
using Lab1.GeometryModel.Edges;

namespace Lab1.Visitors.VoidVisitors
{
    public class InitialCorrectionEdgeVisitor : PolygonShapeKeepingEdgeVisitor, IEdgeVoidVisitor
    {
        public bool Forwards { get; set; }
        public void Visit(Edge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;

            if (Forwards)
                Correct(end);
            else
                Correct(start);

            void Correct(Vertex vertex)
            {
                vertex.ControlAngle = GetControlAngle(start, end);
                vertex.ControlLength = GetControlLength(edge);
            }
        }
        public void Visit(HorizontalEdge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;

            if (Forwards)
                Correct(start, end);
            else
                Correct(end, start);

            void Correct(Vertex firstVertex, Vertex secondVertex)
            {
                if (firstVertex.Y == secondVertex.Y)
                    secondVertex.WasMoved = false;
                else
                {
                    secondVertex.SetPosition(secondVertex.X, firstVertex.Y);
                    secondVertex.WasMoved = true;
                }

                secondVertex.ControlAngle = GetControlAngle(start, end);
                secondVertex.ControlLength = GetControlLength(edge);
            }
        }

        public void Visit(VerticalEdge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;

            if (Forwards)
                Correct(start, end);
            else
                Correct(end, start);

            void Correct(Vertex firstVertex, Vertex secondVertex)
            {
                if (firstVertex.X == secondVertex.X)
                    secondVertex.WasMoved = false;
                else
                {
                    secondVertex.SetPosition(firstVertex.X, secondVertex.Y);
                    secondVertex.WasMoved = true;
                }

                secondVertex.ControlAngle = GetControlAngle(start, end);
                secondVertex.ControlLength = GetControlLength(edge);
            }
        }

        public void Visit(FixedEdge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;

            if (Forwards)
                Correct(start, end);
            else
                Correct(end, start);

            void Correct(Vertex firstVertex, Vertex secondVertex)
            {
                secondVertex.Move(firstVertex.PositionDifference);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(start, end);
                secondVertex.ControlLength = GetControlLength(edge);
            }
        }

        public void Visit(BezierEdge edge) { }
    }
}
