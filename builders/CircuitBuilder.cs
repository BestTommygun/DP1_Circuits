using Circuits.Models;
using Circuits.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP1_Circuits.builders
{
    public class CircuitBuilder
    {
        private NodeBuilder _nodeBuilder;

        public CircuitBuilder()
        {
            _nodeBuilder = new NodeBuilder();
        }
        public Circuit buildCircuit(List<ParserData> parserData)
        {
            Circuit circuit = new Circuit();

            foreach (ParserData data in parserData)
            {
                _nodeBuilder.buildNode(data);
            }

            foreach (ParserData data in parserData)
            {
                _nodeBuilder.addInputs(data);
                if(_nodeBuilder.allNodes[data.Id].GetType().Equals(typeof(OutputNode))) circuit.EndNodes.Add(_nodeBuilder.allNodes[data.Id]);
            }

            return circuit;
        }
    }
}
