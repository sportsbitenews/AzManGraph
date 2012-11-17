using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace AzManGraph
{
    public class AzManRelationship : Edge<AzManItem>
    {
        public AzManRelationship(AzManItem start, AzManItem finish)
            : base(start, finish)
        {

        }
    }
}
