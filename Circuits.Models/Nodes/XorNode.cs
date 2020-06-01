using Circuits.Models.Nodes.NodeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuits.Models.Nodes
{
    public class XorNode : BaseNode
    {
        // node is made out of XNOR gate and an inverter node in this structure:
        // a ┐
        //   ├1─2-output
        // b ┘
        // in our configuration an XNor can only have two inputs
        private readonly XNorNode one;
        private readonly NotNode two;
        public XorNode(string Id, XNorNode one, NotNode two) : base(Id)
        {
            this.one = one;
            this.two = two;
        }
        public override void SetInputs(List<BaseNode> newInputs)
        {
            one.SetInputs(newInputs);
        }
        public override void AddInput(BaseNode newInput)
        {
            one.AddInput(newInput);
        }
        public override List<BaseNode> GetInputs()
        {
            return one.GetInputs();
        }
        public override Tuple<bool, double> CalcOutput()
        {
            if(one != null && two != null)
            {
                var savedOutput = two.CalcOutput();
                //since the node contains 2 subnodes we need to remove one delay
                savedOutput = new Tuple<bool, double>(savedOutput.Item1, savedOutput.Item2 + this.Delay);
                if (components.OfType<VisualComponent>().Any())
                    components.OfType<VisualComponent>().FirstOrDefault().SavedOutput = savedOutput;
                return savedOutput;
            }

            return null;
        }
    }
}
