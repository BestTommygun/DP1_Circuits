using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuits.Models.Nodes.NodeComponents
{
    public class NOTCalcComponent : CalcComponent
    { 
        public NOTCalcComponent(BaseNode parent) : base(parent)
        {
        }

        public override Tuple<bool, double> calcOutput()
        {
            if (Parent.GetInputs().Count == 1)
            {
                var output = Parent.GetInputs().First().CalcOutput();
                return new Tuple<bool, double>(!output.Item1, output.Item2);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
