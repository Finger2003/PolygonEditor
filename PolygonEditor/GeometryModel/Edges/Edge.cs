using PolygonEditor.GeometryModel;
using PolygonEditor.Visitors.CorrectionStatusVisitors;
using PolygonEditor.Visitors.VoidVisitors;

namespace PolygonEditor.GeometryModel.Edges
{
    public class Edge : IEdgeVoidVisitable, IEdgeCorrectionStatusVisitable
    {
        public enum CorrectionStatus
        {
            FurtherCorrectionNeeded,
            FurtherCorrectionNotNeeded,
            CorrectionFailed
        }

        public virtual Vertex Start { get; set; }
        public virtual Vertex End { get; set; }

        public float Length => Vertex.Distance(Start, End);

        public virtual bool IsControlVertex(Vertex v)
        {
            return v == Start || v == End;
        }

        public Edge(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
        }

        public virtual bool IsBasic { get => true; }
        public virtual bool IsHorizontal { get => false; }
        public virtual bool IsVertical { get => false; }
        public virtual bool IsFixed { get => false; }
        public virtual bool IsBezier { get => false; }


        public virtual bool IsHit(Point p)
        {
            double distance = DistanceToPoint(p);
            return distance < 5;
        }

        private double DistanceToPoint(Point p)
        {
            float x0 = p.X;
            float y0 = p.Y;
            float x1 = Start.X;
            float y1 = Start.Y;
            float x2 = End.X;
            float y2 = End.Y;

            double numerator = Math.Abs((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1);
            double denominator = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));

            return numerator / denominator;
        }

        public virtual void Accept(IEdgeVoidVisitor visitor) => visitor.Visit(this);
        public virtual CorrectionStatus Accept(IEdgeCorrectionStatusVisitor visitor) => visitor.Visit(this);

        public virtual bool TryGetHitOwnedVertex(Point p, out Vertex? vertex)
        {
            vertex = null;

            if (Start.IsHit(p))
                vertex = Start;

            return vertex is not null;
        }
        public virtual bool TryGetHitOwnedVertex(float x, float y, out Vertex? vertex)
        {
            vertex = null;

            if (Start.IsHit(x, y))
                vertex = Start;

            return vertex is not null;
        }
    }
}
