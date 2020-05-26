using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.Models.Nodes
{
    public class InputNode : BaseNode
    {
        public InputNode(string id, bool value) 
            : base(id)
        {
            this.CurrentValue = value;
        }

        public bool CurrentValue { get; set; }
        public override Tuple<bool, double> CalcOutput()
        {
            SavedOutput = new Tuple<bool, double>(CurrentValue, this.Delay);
            return SavedOutput;
        }
    }
}
