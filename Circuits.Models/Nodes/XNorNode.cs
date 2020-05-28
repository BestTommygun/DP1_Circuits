using Circuits.Models.Nodes.NodeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuits.Models.Nodes
{
    public class XNorNode : BaseNode
    {
        //node is made up out of 4 NOR nodes in the following configuration:
        // a ┬─2─┐
        //   ├1┤ 4-output
        // b ┴─3─┘
        // in our configuration an XNor can only have two inputs
        private readonly NorNode one, two, three, four;
        public XNorNode(string Id, NorNode one, NorNode two, NorNode three, NorNode four) : base(Id)
        {
            this.one   = one;
            this.two   = two;
            this.three = three;
            this.four  = four;
        }

        public override void SetInputs(List<BaseNode> newInputs)
        {
            if (newInputs.Count == 2)
            {
                one.SetInputs(newInputs);

                var twoList = new List<BaseNode>
                {
                    newInputs[0]
                };
                two.SetInputs(twoList);

                var threeList = new List<BaseNode>
                {
                    newInputs[1]
                };
                three.SetInputs(threeList);
            }
            else throw new InvalidOperationException();
                
        }
        public override void AddInput(BaseNode newInput)
        {
            var inputs = new List<BaseNode>();
            inputs.AddRange(two.GetInputs());
            inputs.AddRange(three.GetInputs());
            if (one.GetInputs().Count == 0)
            {
                one.AddInput(newInput);
                two.AddInput(newInput);
            }
            else if (one.GetInputs().Count == 1)
            {
                one.AddInput(newInput);
                three.AddInput(newInput);
            }
            else throw new InvalidOperationException();
        }
        public override List<BaseNode> GetInputs()
        {
            return one.GetInputs();
        }
        public override Tuple<bool, double> CalcOutput()
        {
            var savedOutput = four.CalcOutput();
            //since the node contains 4 subnodes we need to remove the delay of 3 of these nodes
            savedOutput = new Tuple<bool, double>(savedOutput.Item1, savedOutput.Item2 - (two.Delay + three.Delay + four.Delay));
            if (components.OfType<VisualComponent>().Any())
                components.OfType<VisualComponent>().FirstOrDefault().SavedOutput = savedOutput;
            return savedOutput;
        }
    }
}
