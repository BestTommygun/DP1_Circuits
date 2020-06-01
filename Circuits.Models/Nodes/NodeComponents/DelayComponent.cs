using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.Models.Nodes.NodeComponents
{
    public class DelayComponent : Component
    {
        private readonly double delay;
        public DelayComponent(BaseNode parent) : base(parent)
        {
            this.delay = 15;
        }
        public DelayComponent(BaseNode parent, double delay) : base(parent)
        {
            this.delay = delay;
        }

        public double GetDelay()
        {
            return delay;
        }
    }
}
