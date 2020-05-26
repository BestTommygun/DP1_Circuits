using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.Models.Nodes.NodeComponents
{
    public class ANDCalcComponent : CalcComponent
    {
        public ANDCalcComponent(BaseNode parent) : base(parent)
        {
        }
        public override Tuple<bool, double> calcOutput()
        {
            double biggestDelay = this.Parent.Delay;
            if (Parent.GetInputs().Count > 0)
            {
                foreach (BaseNode input in Parent.GetInputs())
                {
                    var inputOutput = input.CalcOutput();
                    if (inputOutput.Item2 > biggestDelay) biggestDelay = inputOutput.Item2;
                    if (inputOutput.Item1 == false)
                    { 
                        return new Tuple<bool, double>(false, biggestDelay + this.Parent.Delay);
                    }
                }
                return new Tuple<bool, double>(true, biggestDelay + this.Parent.Delay);
            }
            else
            {
                return new Tuple<bool, double>(false, this.Parent.Delay);
            }
        }
    }
}
