using Circuits.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuits.Models
{
    public class Circuit
    {
        public Circuit(string name)
        {
            this.Name = name;
            EndNodes = new List<BaseNode>();
        }
        public List<BaseNode> EndNodes { get; set; }
        public List<BaseNode> AllNodes { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public readonly string Name;
        public List<InputNode> GetInputs()
        {
            return AllNodes.OfType<InputNode>().ToList();
        }
    }
}
