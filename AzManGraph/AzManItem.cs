using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzManGraph
{
    [DebuggerDisplay("{NodeType}: {Title}")]
    public class AzManItem
    {
        public string NodeType { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }

        public AzManItem(string nodeType, string title, string subtitle, string description)
        {
            NodeType = nodeType;
            Title = title;
            Subtitle = subtitle;
            Description = description;
        }
    }
}
