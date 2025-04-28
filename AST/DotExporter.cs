using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.AST
{
    public class DotExporter
    {
        private int nodeId = 0;
        private StringBuilder sb = new();

        public string Export(ASTNode root)
        {
            sb.AppendLine("digraph G {");
            Visit(root);
            sb.AppendLine("}");
            return sb.ToString();
        }

        private int Visit(ASTNode node, int? parentId = null)
        {
            int currentId = nodeId++;
            sb.AppendLine($"  node{currentId} [label=\"{node.Label.Replace("\"", "\\\"")}\"];");

            if (parentId.HasValue)
                sb.AppendLine($"  node{parentId} -> node{currentId};");

            foreach (var child in node.Children)
                Visit(child, currentId);

            return currentId;
        }
    }
}
