using Lab1.GeometryModel;
using Lab1.GeometryModel.Edges;
using Lab1.Visitors.VoidVisitors;
using static Lab1.GeometryModel.Edges.Edge;

namespace Lab1.Visitors.CorrectionStatusVisitors
{
    public class FullCorrectionEdgeVisitor : PolygonShapeKeepingEdgeVisitor, IEdgeCorrectionStatusVisitor
    {
        public bool Forward { get; set; }
        public CorrectionStatus Visit(Edge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;

            return CorrectStraightEdge(edge, correctSecondVertex);

            CorrectionStatus correctSecondVertex(Vertex firstVertex, Vertex secondVertex, double angle)
            {


                double newX, newY;

                if (firstVertex.Continuity == Vertex.ContinuityType.G0)
                {
                    secondVertex.ControlAngle = GetControlAngle(start, end);
                    secondVertex.ControlLength = GetControlLength(edge);
                    //if (secondVertex.Continuity == Vertex.ContinuityType.G0)
                    //{
                    //    return CorrectionStatus.FurtherCorrectionNotNeeded;
                    //}
                    //return CorrectionStatus.FurtherCorrectionNeeded;

                    return secondVertex.Continuity == Vertex.ContinuityType.G0 ? CorrectionStatus.FurtherCorrectionNotNeeded: CorrectionStatus.FurtherCorrectionNeeded;
                }

                if (secondVertex.WasMoved)
                    return CorrectionStatus.CorrectionFailed;

                double length = edge.Length;
                if (firstVertex.Continuity == Vertex.ContinuityType.C1)
                    length = firstVertex.ControlLength * 3;

                newX = firstVertex.X + length * Math.Cos(angle);
                newY = firstVertex.Y + length * Math.Sin(angle);

                secondVertex.SetPosition((float)newX, (float)newY);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(start, end);
                secondVertex.ControlLength = GetControlLength(edge);
                return CorrectionStatus.FurtherCorrectionNeeded;
            }
        }

        public CorrectionStatus Visit(HorizontalEdge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;

            return CorrectStraightEdge(edge, correctSecondVertex);

            CorrectionStatus correctSecondVertex(Vertex firstVertex, Vertex secondVertex, double angle)
            {
                if (firstVertex.Continuity != Vertex.ContinuityType.G0 && firstVertex.ControlAngle != 0 && firstVertex.ControlAngle != Math.PI || secondVertex.WasMoved)
                {
                    return CorrectionStatus.CorrectionFailed;
                }

                double newX, newY = firstVertex.Y;

                if (firstVertex.Continuity == Vertex.ContinuityType.G0)
                {
                    if (firstVertex.Position.Y == secondVertex.Position.Y)
                    {
                        secondVertex.ControlLength = GetControlLength(edge);
                        return secondVertex.Continuity == Vertex.ContinuityType.C1 ? CorrectionStatus.FurtherCorrectionNeeded : CorrectionStatus.FurtherCorrectionNotNeeded;
                    }

                    newX = secondVertex.X;
                }
                else if (firstVertex.Continuity == Vertex.ContinuityType.G1)
                {
                    if (secondVertex.X > firstVertex.X == (firstVertex.ControlAngle == 0) && firstVertex.Y == secondVertex.Y)
                    {
                        secondVertex.ControlAngle = GetControlAngle(start, end);
                        secondVertex.ControlLength = GetControlLength(edge);

                        return secondVertex.Continuity == Vertex.ContinuityType.G0 ? CorrectionStatus.FurtherCorrectionNotNeeded : CorrectionStatus.FurtherCorrectionNeeded;
                    }

                    newX = firstVertex.X + Math.Cos(angle) * edge.Length;
                }
                else if (firstVertex.Continuity == Vertex.ContinuityType.C1)
                {
                    double length = firstVertex.ControlLength * 3;
                    newX = firstVertex.X + Math.Cos(angle) * length;
                }
                else
                    throw new NotImplementedException();


                secondVertex.SetPosition((float)newX, (float)newY);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(start, end);
                secondVertex.ControlLength = GetControlLength(edge);
                return CorrectionStatus.FurtherCorrectionNeeded;
            }
        }

        public CorrectionStatus Visit(VerticalEdge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;

            return CorrectStraightEdge(edge, correctSecondVertex);

            CorrectionStatus correctSecondVertex(Vertex firstVertex, Vertex secondVertex, double angle)
            {
                if (firstVertex.Continuity != Vertex.ContinuityType.G0 && firstVertex.ControlAngle != Math.PI / 2 && firstVertex.ControlAngle != -Math.PI / 2 || secondVertex.WasMoved)
                {
                    return CorrectionStatus.CorrectionFailed;
                }

                double newX = firstVertex.X, newY;

                if (firstVertex.Continuity == Vertex.ContinuityType.G0)
                {
                    if (firstVertex.X == secondVertex.X)
                    {
                        secondVertex.ControlLength = GetControlLength(edge);
                        return secondVertex.Continuity == Vertex.ContinuityType.C1 ? CorrectionStatus.FurtherCorrectionNeeded : CorrectionStatus.FurtherCorrectionNotNeeded;
                    }

                    newY = secondVertex.Y;
                }
                else if (firstVertex.Continuity == Vertex.ContinuityType.G1)
                {
                    if (secondVertex.Y > firstVertex.Y == (firstVertex.ControlAngle == Math.PI / 2) && firstVertex.Y == secondVertex.Y)
                    {
                        secondVertex.ControlAngle = GetControlAngle(start, end);
                        secondVertex.ControlLength = GetControlLength(edge);
                        return secondVertex.Continuity == Vertex.ContinuityType.G0 ? CorrectionStatus.FurtherCorrectionNotNeeded : CorrectionStatus.FurtherCorrectionNeeded;
                    }

                    newY = firstVertex.Y + Math.Sin(angle) * edge.Length;
                }
                else if (firstVertex.Continuity == Vertex.ContinuityType.C1)
                {
                    double length = firstVertex.ControlLength * 3;
                    newY = firstVertex.Y + Math.Sin(angle) * length;
                }
                else
                    throw new NotImplementedException();

                secondVertex.SetPosition((float)newX, (float)newY);
                secondVertex.WasMoved = true;
                secondVertex.ControlAngle = GetControlAngle(start, end);
                secondVertex.ControlLength = GetControlLength(edge);
                return CorrectionStatus.FurtherCorrectionNeeded;
            }
        }


        public CorrectionStatus Visit(FixedEdge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;

            return CorrectStraightEdge(edge, correctSecondVertex);


            CorrectionStatus correctSecondVertex(Vertex firstVertex, Vertex secondVertex, double angle)
            {
                if (firstVertex.Continuity == Vertex.ContinuityType.C1 && !firstVertex.WasMoved && firstVertex.ContinuityPropertiesChanged || secondVertex.WasMoved)
                {
                    return CorrectionStatus.CorrectionFailed;
                }

                bool wasMoved = firstVertex.WasMoved;
                bool isG1 = firstVertex.Continuity == Vertex.ContinuityType.G1;

                if (wasMoved || isG1)
                {
                    if (wasMoved)
                        secondVertex.Move(firstVertex.PositionDifference);
                    else
                    {
                        double newX = firstVertex.X + edge.SetLength * Math.Cos(angle);
                        double newY = firstVertex.Y + edge.SetLength * Math.Sin(angle);
                        secondVertex.SetPosition((float)newX, (float)newY);
                    }
                    secondVertex.WasMoved = true;
                    secondVertex.ControlAngle = GetControlAngle(start, end);
                    secondVertex.ControlLength = GetControlLength(edge);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }

                return CorrectionStatus.FurtherCorrectionNotNeeded;
            }
        }

        private CorrectionStatus CorrectStraightEdge(Edge edge, Func<Vertex, Vertex, double, CorrectionStatus> func)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;
            if (Forward)
            {
                double angle = start.ControlAngle;
                return func(start, end, angle);
            }
            else
            {
                double angle = end.ControlAngle + Math.PI;
                return func(end, start, angle);
            }
        }


        public CorrectionStatus Visit(BezierEdge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;
            Vertex v1 = edge.V1;
            Vertex v2 = edge.V2;

            return Forward ? CorrectVertices(end, v2, v1, start, -1) : CorrectVertices(start, v1, v2, end, 1);



            CorrectionStatus CorrectVertices(Vertex firstVertex, Vertex secondVertex, Vertex thirdVertex, Vertex fourthVertex, int angleMultiplier)
            {
                if (fourthVertex.Continuity != Vertex.ContinuityType.G0 && !thirdVertex.WasMoved && fourthVertex.ContinuityPropertiesChanged)
                {
                    double length = fourthVertex.ControlLength;
                    if (fourthVertex.Continuity == Vertex.ContinuityType.G1)
                    {
                        length = Vertex.Distance(thirdVertex.Position, fourthVertex.PreviousPosition);
                    }

                    double newX = fourthVertex.X - length * Math.Cos(fourthVertex.ControlAngle) * angleMultiplier;
                    double newY = fourthVertex.Y - length * Math.Sin(fourthVertex.ControlAngle) * angleMultiplier;
                    thirdVertex.SetPosition((float)newX, (float)newY);
                    thirdVertex.WasMoved = true;
                }

                if (secondVertex.WasMoved)
                {
                    firstVertex.ControlAngle = GetControlAngle(firstVertex, secondVertex) * angleMultiplier;
                    firstVertex.ControlLength = GetBezierControlLength(firstVertex, secondVertex);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }
                return CorrectionStatus.FurtherCorrectionNotNeeded;
            }
        }

    }
}
