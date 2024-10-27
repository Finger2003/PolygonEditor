using Lab1.GeometryModel.EdgeFactories;
using Lab1.GeometryModel.Edges;
using Lab1.Visitors.VoidVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.GeometryModel
{
    public class Polygon
    {
        public List<Edge> Edges { get; } = [];
        private Vertex.ContuinityType DefaultContuinity { get; } = Vertex.ContuinityType.C1;

        private ResetVerticesPreviousPositionsEdgeVisitor ResetVerticesPreviousPositionsEdgeVisitor { get; } = new();
        private ResetVerticesFlagsEdgeVisitor ResetVerticesFlagsEdgeVisitor { get; } = new();
        private RestoreEdgeVisitor RestoreEdgeVisitor { get; } = new();
        private MoveEdgeVisitor MoveEdgeVisitor { get; } = new();
        private SetVerticesControlValuesEdgeVisitor SetVerticesControlValuesEdgeVisitor { get; } = new();
        private InitialCorrectionEdgeVisitor InitialCorrectionEdgeVisitor { get; } = new();

        public void SetVertexPosition( int vertexIndex, Vertex vertex, float x, float y)
        {
            ResetVerticesPreviousPositions();
            ResetVerticesFlags();
            vertex.SetPosition(x, y);
            Vector2 delta = vertex.PositionDifference;
            vertex.WasMoved = true;
            Edge vertexOwner = Edges[vertexIndex];

            int startingIndexForward = vertexIndex;
            int startingIndexBackward = GetPreviousIndex(vertexIndex);


            if (vertexOwner.IsBezier)
            {
                if (vertexOwner.IsControlVertex(vertex))
                {
                    //SetVerticesControlValuesEdgeVisitor.Vertex = vertex;
                    //vertexOwner.Accept(SetVerticesControlValuesEdgeVisitor);
                    SetVerticesControlValues(vertexOwner, vertex);
                    //vertexOwner.SetVerticesContinuityRelevantProperties(vertex);
                    startingIndexForward = GetNextIndex(startingIndexForward);
                }
                else
                {
                    int previousIndex = GetPreviousIndex(vertexIndex);
                    Edge previousEdge = Edges[previousIndex];
                    InitialCorrectionEdgeVisitor.Forwards = false;
                    previousEdge.Accept(InitialCorrectionEdgeVisitor);
                    //previousEdge.CorrectStartPositionBasically();

                    //SetVerticesControlValuesEdgeVisitor.Vertex = vertex;
                    //previousEdge.Accept(SetVerticesControlValuesEdgeVisitor);
                    SetVerticesControlValues(previousEdge, vertex);
                    //previousEdge.SetVerticesContinuityRelevantProperties(vertex);
                    startingIndexBackward = GetPreviousIndex(startingIndexBackward);
                }
            }
            else
            {
                //vertexOwner.CorrectEndPositionBasically();
                InitialCorrectionEdgeVisitor.Forwards = true;
                vertexOwner.Accept(InitialCorrectionEdgeVisitor);
                startingIndexForward = GetNextIndex(startingIndexForward);
                //SetVerticesControlValuesEdgeVisitor.Vertex = vertex;
                //vertexOwner.Accept(SetVerticesControlValuesEdgeVisitor);
                SetVerticesControlValues(vertexOwner, vertex);
                //vertexOwner.SetVerticesContinuityRelevantProperties(vertex);
            }

            bool correctionSucceeded = CorrectEdges(startingIndexForward, startingIndexBackward);

            if (!correctionSucceeded)
            {
                Restore();
                Move(delta.X, delta.Y);
            }

            ResetVerticesPreviousPositions();
            ResetVerticesFlags();
        }


        private void Restore()
        {
            //foreach (Edge edge in Edges)
            //    edge.Restore();
            Edges.ForEach(edge => edge.Accept(RestoreEdgeVisitor));
        }
        public void Move(float dx, float dy)
        {
            MoveEdgeVisitor.Dx = dx;
            MoveEdgeVisitor.Dy = dy;
            Edges.ForEach(edge => edge.Accept(MoveEdgeVisitor));
            //foreach (Edge edge in Edges)
            //{
            //    edge.MoveOwnedVertices(dx, dy);
            //}
        }


        private void ResetVerticesPreviousPositions()
        {
            Edges.ForEach(edge => edge.Accept(ResetVerticesPreviousPositionsEdgeVisitor));
            //foreach (Edge edge in Edges)
            //{
            //    //edge.ResetOwnedMovedVerticesPreviousPositions();
            //    edge.Accept(resetVerticesPreviousPositionsEdgeVisitor);
            //}
        }
        private void ResetVerticesFlags()
        {
            Edges.ForEach(edge => edge.Accept(ResetVerticesFlagsEdgeVisitor));
            //foreach (Edge edge in Edges)
            //{
            //    edge.ResetOwnedVerticesMovementFlags();
            //}
        }
        public int GetPreviousIndex(int index)
        {
            return index == 0 ? Edges.Count - 1 : index - 1;
        }
        public int GetNextIndex(int index)
        {
            return index == Edges.Count - 1 ? 0 : index + 1;
        }
        private bool CorrectEdges(int startingIndexForward, int startingIndexBackward)
        {
            int index = startingIndexForward;
            int i = 0;
            Edge.CorrectionStatus correctingStatus;
            do
            {
                if (index >= Edges.Count)
                {
                    index = 0;
                }
                correctingStatus = Edges[index++].CorrectEndPosition();

            } while (correctingStatus == Edge.CorrectionStatus.FurtherCorrectionNeeded && i++ < Edges.Count);

            if (correctingStatus == Edge.CorrectionStatus.CorrectionFailed)
                return false;

            index = startingIndexBackward;
            i = 0;
            do
            {
                if (index < 0)
                    index = Edges.Count - 1;
                correctingStatus = Edges[index--].CorrectStartPosition();
            } while (correctingStatus == Edge.CorrectionStatus.FurtherCorrectionNeeded && i++ < Edges.Count);

            return correctingStatus != Edge.CorrectionStatus.CorrectionFailed;
        }

        public (int, Vertex?) GetHitVertexWithIndex(float x, float y)
        {
            int returnIndex = -1;
            Vertex? returnVertex = null;

            foreach ((Edge edge, int index) in Edges.Select((edge, index) => (edge, index)))
            {
                if (edge.TryGetHitOwnedVertex(x, y, out Vertex? vertex))
                {
                    returnIndex = index;
                    returnVertex = vertex;
                    break;
                }
            }
            return (returnIndex, returnVertex);
        }

        public bool TrySetFixedEdge(int index, Edge edge, int length)
        {
            //ResetVerticesPreviousPositions();
            //ResetVertexMovementFlags();
            //FixedEdge fixedEdge = new FixedEdge(edge.Start, edge.End, length);
            //fixedEdge.SetVerticesContinuityRelevantProperties(edge!.Start);
            //fixedEdge.SetVerticesContinuityRelevantProperties(edge!.End);

            //Edges[index] = fixedEdge;
            //if (!CorrectEdges(GetNextIndex(index), GetPreviousIndex(index)))
            //{
            //    Edges[index] = edge;
            //    Restore();
            //    return false;
            //}
            //return true;
            return TrySetConstraintForEdge(index, edge, new FixedEdgeFactory(length));
        }

        public bool TrySetVerticalEdge(int index, Edge edge)
        {
            //ResetVerticesPreviousPositions();
            //ResetVertexMovementFlags();
            //VerticalEdge verticalEdge = new VerticalEdge(edge.Start, edge.End);
            //verticalEdge.SetVerticesContinuityRelevantProperties(edge.Start);
            //verticalEdge.SetVerticesContinuityRelevantProperties(edge.End);

            //Edges[index] = verticalEdge;

            //if (!CorrectEdges(GetNextIndex(index), GetPreviousIndex(index)))
            //{
            //    Edges[index] = edge;
            //    Restore();
            //    return false;
            //}
            //return true;
            return TrySetConstraintForEdge(index, edge, new VerticalEdgeFactory());
        }

        public bool TrySetHorizontalEdge(int index, Edge edge)
        {
            //ResetVerticesPreviousPositions();
            //ResetVertexMovementFlags();
            //HorizontalEdge horizontalEdge = new HorizontalEdge(edge.Start, edge.End);
            //horizontalEdge.SetVerticesContinuityRelevantProperties(edge.Start);
            //horizontalEdge.SetVerticesContinuityRelevantProperties(edge.End);

            //Edges[index] = horizontalEdge;

            //if (!CorrectEdges(GetNextIndex(index), GetPreviousIndex(index)))
            //{
            //    Edges[index] = edge;
            //    Restore();
            //    return false;
            //}
            //return true;
            return TrySetConstraintForEdge(index, edge, new HorizontalEdgeFactory());
        }
        private bool TrySetConstraintForEdge(int index, Edge edge, EdgeFactory edgeFactory)
        {
            ResetVerticesPreviousPositions();
            ResetVerticesFlags();

            Edge newEdge = edgeFactory.CreateEdge(edge.Start, edge.End);

            //newEdge.SetVerticesContinuityRelevantProperties(edge.Start);
            //newEdge.SetVerticesContinuityRelevantProperties(edge.End);
            SetVerticesControlValues(newEdge, edge.Start);
            SetVerticesControlValues(newEdge, edge.End);


            Edges[index] = newEdge;

            if (!CorrectEdges(GetNextIndex(index), GetPreviousIndex(index)))
            {
                Edges[index] = edge;
                Restore();
                return false;
            }
            return true;
        }

        public bool TrySetBezierCurve(int index, Edge edge)
        {
            ResetVerticesPreviousPositions();
            ResetVerticesFlags();

            if (edge.Start.Continuity == Vertex.ContuinityType.G0)
                edge.Start.Continuity = DefaultContuinity;

            if (edge.End.Continuity == Vertex.ContuinityType.G0)
                edge.End.Continuity = DefaultContuinity;


            BezierEdge bezierEdge = new BezierEdge(edge.Start, edge.End);
            int previousIndex = GetPreviousIndex(index);
            int nextIndex = GetNextIndex(index);
            //Edges[previousIndex].SetVerticesContinuityRelevantProperties(edge.Start);
            //Edges[nextIndex].SetVerticesContinuityRelevantProperties(edge.End);

            SetVerticesControlValues(Edges[previousIndex], edge.Start);
            SetVerticesControlValues(Edges[nextIndex], edge.End);

            Edges[index] = bezierEdge;

            if (!CorrectEdges(index, index))
            {
                Edges[index] = edge;
                Restore();
                return false;
            }

            return true;
        }

        public void AddVertexInEdge(int index, Edge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;
            Vertex middle = new Vertex((start.X + end.X) / 2, (start.Y + end.Y) / 2);

            Edge edge1 = new Edge(start, middle);
            Edge edge2 = new Edge(middle, end);

            int previousIndex = GetPreviousIndex(index);
            int nextIndex = GetNextIndex(index);

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

        public void DeleteVertex(int index, Vertex vertex)
        {
            if (Edges.Count <= 3)
            {
                Edges.Clear();
                return;
            }

            int previousIndex = GetPreviousIndex(index);
            int nextIndex = GetNextIndex(index);
            Edge previousPreviousEdge = Edges[GetPreviousIndex(previousIndex)];
            Edge nextEdge = Edges[nextIndex];

            if (!previousPreviousEdge.IsBezier)
                previousPreviousEdge.End.Continuity = Vertex.ContuinityType.G0;

            if(!nextEdge.IsBezier)
                nextEdge.Start.Continuity = Vertex.ContuinityType.G0;

            Edge edge1 = Edges[previousIndex];
            Edge edge2 = Edges[index];

            Edge newEdge = new Edge(edge1.Start, edge2.End);
            Edges[previousIndex] = newEdge;
            Edges.RemoveAt(index);
        }

        public bool TrySetContinuityInVertex(int index, Vertex vertex, Vertex.ContuinityType continuity)
        {
            if (continuity == Vertex.ContuinityType.G0)
            {
                vertex.Continuity = continuity;
                return true;
            }

            ResetVerticesPreviousPositions();
            ResetVerticesFlags();

            Vertex.ContuinityType oldContinuity = vertex.Continuity;
            int previousIndex = GetPreviousIndex(index);
            int nextIndex = GetNextIndex(index);

            vertex.Continuity = continuity;
            vertex.ContinuityChanged = true;


            //Edges[previousIndex].SetVerticesContinuityRelevantProperties(vertex);

            SetVerticesControlValues(Edges[previousIndex], vertex);


            if (!CorrectEdges(GetNextIndex(index), GetPreviousIndex(index)))
            {
                vertex.ContinuityChanged = true;
                Restore();
                vertex.Continuity = oldContinuity;
                return false;
            }

            vertex.ContinuityChanged = true;
            return true;
        }

        public void RemoveConstraint(int index, Edge edge)
        {
            int previousIndex = GetPreviousIndex(index);
            int nextIndex = GetNextIndex(index);

            if (!Edges[previousIndex].IsBezier)
                Edges[previousIndex].End.Continuity = Vertex.ContuinityType.G0;

            if (!Edges[nextIndex].IsBezier)
                Edges[nextIndex].Start.Continuity = Vertex.ContuinityType.G0;

            Edge newEdge = new Edge(edge.Start, edge.End);
            Edges[index] = newEdge;
        }

        private void SetVerticesControlValues(Edge edge, Vertex vertex)
        {
            SetVerticesControlValuesEdgeVisitor.Vertex = vertex;
            edge.Accept(SetVerticesControlValuesEdgeVisitor);
        }
    }
}
