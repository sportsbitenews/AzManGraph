using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace AzManGraph
{
    public class SecurityGraph : BidirectionalGraph<AzManItem, AzManRelationship>
    {
        public SecurityGraph()
        {
            // Nothing
        }

        public SecurityGraph(bool allowParallelEdges)
            : base(allowParallelEdges)
        {
            // Nothing
        }

        public SecurityGraph(bool allowParallelEdges, int vertexCapacity)
            : base(allowParallelEdges, vertexCapacity)
        {
            // Nothing
        }
    }
}
