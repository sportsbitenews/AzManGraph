using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AzManGraph
{
    public class Loader
    {
        public SecurityGraph Graph { get; private set; }

        public Loader()
        {
            Graph = new SecurityGraph();
        }

        public void LoadFromFile(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
            {
                LoadFromStream(stream);
            }
        }

        public void LoadFromStream(Stream stream)
        {
            var document = XDocument.Load(stream);
            foreach (var application in Find(document.Root, "/AzAdminManager/AzApplication"))
            {
                AddApplication(application);
            }
        }

        private void AddApplication(XElement node)
        {
            var name = (string)node.Attribute("Name");
            var description = (string)node.Attribute("Description");
            var guid = (string)node.Attribute("Guid");
            var application = new AzManItem("Application", name, "Application", description);
            mItems[guid] = application;
            Graph.AddVertex(application);

            foreach (var operation in Find(node, "AzOperation"))
            {
                AddOperation(operation);
            }

            foreach (var task in Find(node, "AzTask"))
            {
                AddTask(task);
            }

            foreach (var task in Find(node, "AzTask"))
            {
                AddTaskLinks(task);
            }

            foreach (var role in Find(node, "AzTask[@RoleDefinition]"))
            {
                var roleGuid = (string)role.Attribute("Guid");

                var relationship = new AzManRelationship(application, mItems[roleGuid]);
                Graph.AddEdge(relationship);
            }
        }

        private void AddOperation(XElement node)
        {
            var name = (string)node.Attribute("Name");
            var description = (string)node.Attribute("Description");
            var guid = (string)node.Attribute("Guid");
            var operationId = (string)node.Element("OperationID");
            var operation = new AzManItem("Operation", name, string.Format("Operation #{0}", operationId), description);
            mItems[guid] = operation;
            Graph.AddVertex(operation);
        }

        private void AddTask(XElement node)
        {
            var name = (string)node.Attribute("Name");
            var description = (string)node.Attribute("Description");
            var guid = (string)node.Attribute("Guid");

            var isRole = (bool?)node.Attribute("RoleDefinition");

            var nodeType = isRole == true ? "Role" : "Task";
            var task = new AzManItem(nodeType, name, nodeType, description);
            mItems[guid] = task;
            Graph.AddVertex(task);
        }

        private void AddTaskLinks(XElement node)
        {
            var guid = (string)node.Attribute("Guid");
            var task = mItems[guid];

            AddLinks(task, node, "OperationLink");
            AddLinks(task, node, "TaskLink");
        }

        private IEnumerable<XElement> Find(XElement root, string xpath)
        {
            return ((IEnumerable<object>)root.XPathEvaluate(xpath)).Cast<XElement>();
        }

        private void AddLinks(AzManItem item, XElement node, string xpath)
        {
            foreach (var link in Find(node, xpath))
            {
                var linkedItem = mItems[(string)link];
                var relationship = new AzManRelationship(item, linkedItem);
                Graph.AddEdge(relationship);
            }
        }

        private readonly Dictionary<string, AzManItem> mItems = new Dictionary<string, AzManItem>();
    }
}
