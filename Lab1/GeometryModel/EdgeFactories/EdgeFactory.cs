using Lab1.GeometryModel.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.GeometryModel.EdgeFactories
{
    public abstract class EdgeFactory
    {
        public abstract Edge CreateEdge(Vertex start, Vertex end);
    }
}
