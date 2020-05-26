using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.Models.Nodes
{
    public class OutputNode : BaseNode
    {
        public OutputNode(string id) : base(id)
        { }

        public override Tuple<bool, double> CalcOutput()
        {
            if(inputs.Count > 1)
            {
                throw new FormatException();
            }
            else
            {
                var inputOutput = inputs[0].CalcOutput();
                SavedOutput = new Tuple<bool, double>(inputOutput.Item1, inputOutput.Item2 + this.Delay);
                return SavedOutput;
            }
        }
    }
}
