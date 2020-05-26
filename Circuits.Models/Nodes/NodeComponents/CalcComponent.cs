using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.Models.Nodes.NodeComponents
{
    public class CalcComponent : Component
    {
        public CalcComponent(BaseNode parent) : base(parent)
        {
        }
        public virtual Tuple<bool, double> calcOutput()
        {
            return new Tuple<bool, double>(false, 15);
        }
    }
}
