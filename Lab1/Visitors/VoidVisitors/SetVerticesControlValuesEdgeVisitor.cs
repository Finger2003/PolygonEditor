using Lab1.GeometryModel;
using Lab1.GeometryModel.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Visitors.VoidVisitors
{
    public class SetVerticesControlValuesEdgeVisitor : PolygonShapeKeepingEdgeVisitor, IEdgeVoidVisitor
    {
        public Vertex? Vertex { get; set; }
        private void SetStraightEdgeControlValues(Edge edge)
        {
            Vertex start = edge.Start;
            Vertex end = edge.End;
            Vertex!.ControlAngle = GetControlAngle(start, end);
            Vertex!.ControlLength = GetControlLength(edge);
        }
        public void Visit(Edge edge) => SetStraightEdgeControlValues(edge);
        public void Visit(HorizontalEdge edge) => SetStraightEdgeControlValues(edge);
        public void Visit(VerticalEdge edge) => SetStraightEdgeControlValues(edge);
        public void Visit(FixedEdge edge) => SetStraightEdgeControlValues(edge);

        public void Visit(BezierEdge edge)
        {
            Vertex v0 = edge.Start;
            Vertex v1 = edge.V1;
            Vertex v2 = edge.V2;
            Vertex v3 = edge.End;

            if (Vertex == v0 || Vertex == v1)
            {
                v0.ControlAngle = GetControlAngle(v0, v1);
                v0.ControlLength = GetControlLength(v0, v1);
            }
            else if (Vertex == v2 || Vertex == v3)
            {
                v3.ControlAngle = GetControlAngle(v2, v3);
                v3.ControlLength = GetControlLength(v2, v3);
            }

            //double getControlAngle(Vertex v, Vertex w) => Math.Atan2(w.Y - v.Y, w.X - v.X);
            //double getControlLength(Vertex v, Vertex w) => Vertex.Distance(v, w);
        }
    }
}
