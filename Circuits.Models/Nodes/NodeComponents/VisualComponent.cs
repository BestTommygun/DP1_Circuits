using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.Models.Nodes.NodeComponents
{
    public class VisualComponent : Component
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Tuple<bool, double> SavedOutput;
        public VisualComponent(BaseNode parent) : base(parent)
        {
            X = 0;
            Y = 0;
            Name = this.Parent.Id;
            Type = this.Parent.GetType().Name;
        }
    }
}
