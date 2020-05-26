using Circuits.Models.Nodes.NodeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circuits.Models.Nodes
{
    public class NorNode : BaseNode
    {
        private readonly OrNode orNode;
        private readonly NotNode notNode;
        public NorNode(string id, OrNode orNode, NotNode notNode) : base(id)
        {
            this.orNode = orNode;
            this.notNode = notNode;
        }
        public override void SetInputs(List<BaseNode> newInputs)
        {
            orNode.SetInputs(newInputs);
        }
        public override void AddInput(BaseNode newInput)
        {
            orNode.AddInput(newInput);
        }
        public override List<BaseNode> GetInputs()
        {
            return orNode.GetInputs();
        }
        public override Tuple<bool, double> CalcOutput()
        {
            if (notNode != null && orNode != null)
            {
                var savedOutput = notNode.CalcOutput();
                if (components.OfType<VisualComponent>().Any())
                    components.OfType<VisualComponent>().FirstOrDefault().SavedOutput = savedOutput;
                return savedOutput;
            }
            return new Tuple<bool, double>(false, 15);
        }
    }
}
