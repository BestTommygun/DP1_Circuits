using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.Models.Nodes.NodeComponents
{
    public abstract class Component
    {
        public BaseNode Parent { get; set; }
        public Component(BaseNode parent) => this.Parent = parent;
    }
}
