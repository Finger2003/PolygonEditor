﻿using Lab1.GeometryModel;
using Lab1.GeometryModel.Edges;

namespace Lab1.Visitors.VoidVisitors
{
    public class ResetVerticesFlagsEdgeVisitor : IEdgeVoidVisitor
    {
        private void ResetVertex(Vertex vertex)
        {
            vertex.WasMoved = false;
            vertex.ContinuityPropertiesChanged = false;
        }
        public void Visit(Edge edge) => ResetVertex(edge.Start);
        public void Visit(HorizontalEdge edge) => ResetVertex(edge.Start);
        public void Visit(VerticalEdge edge) => ResetVertex(edge.Start);
        public void Visit(FixedEdge edge) => ResetVertex(edge.Start);
        public void Visit(BezierEdge edge)
        {
            ResetVertex(edge.Start);
            ResetVertex(edge.V1);
            ResetVertex(edge.V2);
        }
    }
}
