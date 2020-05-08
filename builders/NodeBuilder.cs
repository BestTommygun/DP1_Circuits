using Circuits.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Circuits.Models;

namespace DP1_Circuits.builders
{
    public class NodeBuilder
    {
        public Dictionary<string, BaseNode> allNodes = new Dictionary<string, BaseNode>();
        private BaseNode _currentNode;
        public NodeBuilder buildNode(ParserData data)
        {
            switch (data.Type)
            {
                case "AND":
                    _currentNode = new AndNode();
                    break;
                case "OR":
                    _currentNode = new AndNode();
                    break;
                case "INPUT_HIGH":
                    _currentNode = new InputNode(true);
                    break;
                case "INPUT_LOW":
                    _currentNode = new InputNode(false);
                    break;
                case "PROBE":
                    _currentNode = new OutputNode();
                    break;
                case "NOT":
                    _currentNode = new NotNode();
                    break;
                default:
                    break;
            }
            allNodes.Add(data.Id, _currentNode);

            return this;
        }

        public NodeBuilder addInputs(ParserData data)
        {
            foreach (string output in data.Ouputs)
            {
                if (allNodes.ContainsKey(output))
                {
                    allNodes[output].Inputs.Add(allNodes[data.Id]);
                }
                else
                {
                    //TODO: throw error that it's trying to connect to a non existing node (bad file!)
                }

            }
            return this;
        }
        public NodeBuilder addDelay(double delay)
        {
            _currentNode.Delay = delay;
            return this;
        }

        public BaseNode getNode()
        {
            return this._currentNode;
        }
    }
}
