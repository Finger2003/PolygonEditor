using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Edges
{
    public abstract class SpecialEdge : Edge
    {
        SpecialEdge(Vertex start, Vertex end) : base(start, end) { }
        public override bool IsBasic { get => false; }
    }
}
