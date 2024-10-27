using Lab1.GeometryModel;
using Lab1.GeometryModel.Edges;
using Lab1.Visitors.VoidVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            if (Forward)
            {
                double angle = start.ControlAngle;
                return correctSecondVertex(start, end, angle);
            }
            else
            {
                double angle = end.ControlAngle + Math.PI;
                return correctSecondVertex(end, start, angle);
            }

            CorrectionStatus correctSecondVertex(Vertex firstVertex, Vertex secondVertex, double angle)
            {
                float newX, newY;

                if (firstVertex.Continuity == Vertex.ContinuityType.G0)
                {
                    secondVertex.ControlAngle = GetControlAngle(start, end);
                    secondVertex.ControlLength = GetControlLength(edge);
                    if (secondVertex.Continuity == Vertex.ContinuityType.G0)
                    {
                        return CorrectionStatus.FurtherCorrectionNotNeeded;
                    }
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }

                double length = edge.Length;
                if (firstVertex.Continuity == Vertex.ContinuityType.C1)
                {
                    length = firstVertex.ControlLength * 3;
                }

                newX = (float)(firstVertex.X + length * Math.Cos(angle));
                newY = (float)(firstVertex.Y + length * Math.Sin(angle));

                secondVertex.SetPosition(newX, newY);
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
            if (Forward)
            {
                return correctSecondVertex(start, end);
            }
            else
            {
                return correctSecondVertex(end, start);
            }


            CorrectionStatus correctSecondVertex(Vertex firstVertex, Vertex secondVertex)
            {
                if (firstVertex.Continuity != Vertex.ContinuityType.G0 && firstVertex.ControlAngle != 0 && firstVertex.ControlAngle != Math.PI /*&& !firstVertex.ContinuityChanged */|| secondVertex.WasMoved)
                {
                    return CorrectionStatus.CorrectionFailed;
                }

                if (firstVertex.Continuity == Vertex.ContinuityType.G0)
                {
                    if (firstVertex.Position.Y == secondVertex.Position.Y)
                    {
                        secondVertex.ControlLength = GetControlLength(edge);
                        if (secondVertex.Continuity == Vertex.ContinuityType.C1)
                        {
                            return CorrectionStatus.FurtherCorrectionNeeded;
                        }
                        return CorrectionStatus.FurtherCorrectionNotNeeded;
                    }

                    secondVertex.SetPosition(secondVertex.X, firstVertex.Y);
                    secondVertex.WasMoved = true;
                    secondVertex.ControlAngle = GetControlAngle(start, end);
                    secondVertex.ControlLength = GetControlLength(edge);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }
                else if (firstVertex.Continuity == Vertex.ContinuityType.G1)
                {
                    if (secondVertex.X > firstVertex.X == (firstVertex.ControlAngle == 0) && firstVertex.Y == secondVertex.Y)
                    {
                        secondVertex.ControlAngle = GetControlAngle(start, end);
                        secondVertex.ControlLength = GetControlLength(edge);
                        if (secondVertex.Continuity == Vertex.ContinuityType.G0)
                        {
                            return CorrectionStatus.FurtherCorrectionNotNeeded;
                        }
                        return CorrectionStatus.FurtherCorrectionNeeded;
                    }

                    float newX = firstVertex.X + edge.Length * (secondVertex.X > firstVertex.X ? 1 : -1);

                    secondVertex.SetPosition(newX, firstVertex.Y);
                    secondVertex.WasMoved = true;
                    secondVertex.ControlAngle = GetControlAngle(start, end);
                    secondVertex.ControlLength = GetControlLength(edge);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }
                else if (firstVertex.Continuity == Vertex.ContinuityType.C1)
                {
                    double angle = firstVertex.ControlAngle;
                    double length = firstVertex.ControlLength * 3;

                    double newX = firstVertex.X + (secondVertex.X > firstVertex.X ? 1 : -1) * length;
                    double newY = firstVertex.Y;

                    secondVertex.SetPosition((float)newX, (float)newY);
                    secondVertex.WasMoved = true;
                    secondVertex.ControlAngle = GetControlAngle(start, end);
                    secondVertex.ControlLength = GetControlLength(edge);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }
                else
                {
                    return CorrectionStatus.FurtherCorrectionNotNeeded;
                }
            }
        }

        public CorrectionStatus Visit(VerticalEdge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;
            if (Forward)
            {
                return correctSecondVertex(start, end);
            }
            else
            {
                return correctSecondVertex(end, start);
            }

            CorrectionStatus correctSecondVertex(Vertex firstVertex, Vertex secondVertex)
            {
                if (firstVertex.Continuity != Vertex.ContinuityType.G0 && firstVertex.ControlAngle != Math.PI / 2 && firstVertex.ControlAngle != -Math.PI / 2 /*&& !firstVertex.ContinuityChanged*/ || secondVertex.WasMoved)
                {
                    return CorrectionStatus.CorrectionFailed;
                }

                if (firstVertex.Continuity == Vertex.ContinuityType.G0)
                {
                    if (firstVertex.X == secondVertex.X)
                    {
                        secondVertex.ControlLength = GetControlLength(edge);
                        if (secondVertex.Continuity == Vertex.ContinuityType.C1)
                        {
                            return CorrectionStatus.FurtherCorrectionNeeded;
                        }
                        return CorrectionStatus.FurtherCorrectionNotNeeded;
                    }

                    secondVertex.SetPosition(firstVertex.X, secondVertex.Y);
                    secondVertex.WasMoved = true;
                    secondVertex.ControlAngle = GetControlAngle(start, end);
                    secondVertex.ControlLength = GetControlLength(edge);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }
                else if (firstVertex.Continuity == Vertex.ContinuityType.G1)
                {
                    if (secondVertex.Y > firstVertex.Y == (firstVertex.ControlAngle == Math.PI / 2) && firstVertex.Y == secondVertex.Y)
                    {
                        secondVertex.ControlAngle = GetControlAngle(start, end);
                        secondVertex.ControlLength = GetControlLength(edge);
                        if (secondVertex.Continuity == Vertex.ContinuityType.G0)
                        {
                            return CorrectionStatus.FurtherCorrectionNotNeeded;
                        }
                        return CorrectionStatus.FurtherCorrectionNeeded;
                    }

                    float newY = firstVertex.Y + edge.Length * (secondVertex.Y > firstVertex.Y ? 1 : -1);

                    secondVertex.SetPosition(firstVertex.X, newY);
                    secondVertex.WasMoved = true;
                    secondVertex.ControlAngle = GetControlAngle(start, end);
                    secondVertex.ControlLength = GetControlLength(edge);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }
                else if (firstVertex.Continuity == Vertex.ContinuityType.C1)
                {
                    double angle = firstVertex.ControlAngle;
                    double length = firstVertex.ControlLength * 3;

                    double newX = firstVertex.X;
                    double newY = firstVertex.Y + length * (secondVertex.Y > firstVertex.Y ? 1 : -1);

                    secondVertex.SetPosition((float)newX, (float)newY);
                    secondVertex.WasMoved = true;
                    secondVertex.ControlAngle = GetControlAngle(start, end);
                    secondVertex.ControlLength = GetControlLength(edge);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }
                else
                {
                    return CorrectionStatus.FurtherCorrectionNotNeeded;
                }
            }
        }


        public CorrectionStatus Visit(FixedEdge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;
            if (Forward)
            {
                double angle = start.ControlAngle;
                return correctSecondVertex(start, end, angle);
            }
            else
            {
                double angle = end.ControlAngle + Math.PI;
                return correctSecondVertex(end, start, angle);
            }

            CorrectionStatus correctSecondVertex(Vertex firstVertex, Vertex secondVertex, double angle)
            {
                if (firstVertex.Continuity == Vertex.ContinuityType.C1 && !firstVertex.WasMoved && /*!firstVertex.ContinuityChanged &&*/ firstVertex.ContinuityPropertiesChanged || secondVertex.WasMoved)
                {
                    return CorrectionStatus.CorrectionFailed;
                }

                if (firstVertex.WasMoved)
                {
                    secondVertex.Move(firstVertex.PositionDifference);
                    secondVertex.WasMoved = true;
                    secondVertex.ControlAngle = GetControlAngle(start, end);
                    secondVertex.ControlLength = GetControlLength(edge);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }
                else if (firstVertex.Continuity == Vertex.ContinuityType.G1)
                {
                    double newX = firstVertex.X + edge.SetLength * Math.Cos(angle);
                    double newY = firstVertex.Y + edge.SetLength * Math.Sin(angle);

                    secondVertex.SetPosition((float)newX, (float)newY);
                    secondVertex.WasMoved = true;
                    secondVertex.ControlAngle = GetControlAngle(start, end);
                    secondVertex.ControlLength = GetControlLength(edge);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }
                return CorrectionStatus.FurtherCorrectionNotNeeded;
            }

        }


        public CorrectionStatus Visit(BezierEdge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;
            Vertex v1 = edge.V1;
            Vertex v2 = edge.V2;

            if (Forward)
            {
                if (start.Continuity != Vertex.ContinuityType.G0 && !v1.WasMoved && start.ContinuityPropertiesChanged)
                {
                    double length = start.ControlLength;
                    if (start.Continuity == Vertex.ContinuityType.G1)
                    {
                        length = Vertex.Distance(start.PreviousPosition, v1.Position);
                    }

                    double newX = start.X + length * Math.Cos(start.ControlAngle);
                    double newY = start.Y + length * Math.Sin(start.ControlAngle);
                    v1.SetPosition((float)newX, (float)newY);
                    //v1.WasMoved = true;
                }

                if (v2.WasMoved)
                {
                    end.ControlAngle = GetControlAngle(v2, end);
                    end.ControlLength = GetBezierControlLength(v2, end);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }
                return CorrectionStatus.FurtherCorrectionNotNeeded;
            }
            else
            {
                if (end.Continuity != Vertex.ContinuityType.G0 && !v2.WasMoved && end.ContinuityPropertiesChanged)
                {
                    double length = end.ControlLength;
                    if (end.Continuity == Vertex.ContinuityType.G1)
                    {
                        length = Vertex.Distance(v2.Position, end.PreviousPosition);
                    }

                    double newX = end.X - length * Math.Cos(end.ControlAngle);
                    double newY = end.Y - length * Math.Sin(end.ControlAngle);
                    v2.SetPosition((float)newX, (float)newY);
                    //v2.WasMoved = true;
                }

                if (v1.WasMoved)
                {
                    start.ControlAngle = GetControlAngle(start, v1);
                    start.ControlLength = GetBezierControlLength(start, v1);
                    return CorrectionStatus.FurtherCorrectionNeeded;
                }
                return CorrectionStatus.FurtherCorrectionNotNeeded;
            }
        }

    }
}
