using Lab1.GeometryModel.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Visitors.CorrectionStatusVisitors
{
    public interface IEdgeCorrectionStatusVisitable
    {
        Edge.CorrectionStatus Accept(IEdgeCorrectionStatusVisitor visitor);
    }
}
