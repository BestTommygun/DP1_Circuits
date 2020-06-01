using Circuits.Models.Nodes.NodeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuits.Models.Nodes
{
    public class NandNode : BaseNode
    {
        private readonly NorNode norNode; //since a Nand has the same output as a NOR we can just give it a nor node as a child
        public NandNode(string id, NorNode node) : base(id)
        {
            norNode = node;
        }
        public override void SetInputs(List<BaseNode> newInputs)
        {
            norNode.SetInputs(newInputs);
        }
        public override void AddInput(BaseNode newInput)
        {
            norNode.AddInput(newInput);
        }
        public override List<BaseNode> GetInputs()
        {
            return norNode.GetInputs();
        }
        public override Tuple<bool, double> CalcOutput()
        {
            if (norNode != null)
            {
                var savedOutput = norNode.CalcOutput();
                savedOutput = new Tuple<bool, double>(savedOutput.Item1, savedOutput.Item2 + this.Delay);
                if (components.OfType<VisualComponent>().Any())
                    components.OfType<VisualComponent>().FirstOrDefault().SavedOutput = savedOutput;
                return savedOutput;
            }
            else throw new InvalidOperationException();
        }
    }
}
