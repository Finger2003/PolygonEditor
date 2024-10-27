using Lab1.GeometryModel.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.GeometryModel.EdgeFactories
{
    public class VerticalEdgeFactory : EdgeFactory
    {
        public override Edge CreateEdge(Vertex start, Vertex end)
        {
            return new VerticalEdge(start, end);
        }
    }
}
