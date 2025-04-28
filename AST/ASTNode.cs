using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.AST
{
    public class ASTNode
    {
        public string Label { get; set; }
        public List<ASTNode> Children { get; set; } = new();

        public ASTNode(string label)
        {
            Label = label;
        }

        public void AddChild(ASTNode child)
        {
            Children.Add(child);
        }
    }
}
