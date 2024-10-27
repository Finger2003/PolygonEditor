using Lab1.GeometryModel.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Visitors.VoidVisitors
{
    public class MoveEdgeVisitor : IEdgeVoidVisitor
    {
        public float Dx { get; set; }
        public float Dy { get; set; }
        private void MoveStart(Edge edge)
        {
            edge.Start.Move(Dx, Dy);
        }
        public void Visit(Edge edge) => MoveStart(edge);
        public void Visit(HorizontalEdge edge) => MoveStart(edge);
        public void Visit(VerticalEdge edge) => MoveStart(edge);
        public void Visit(FixedEdge edge) => MoveStart(edge);
        public void Visit(BezierEdge edge)
        {
            MoveStart(edge);
            edge.V1.Move(Dx, Dy);
            edge.V2.Move(Dx, Dy);
        }
    }
}
