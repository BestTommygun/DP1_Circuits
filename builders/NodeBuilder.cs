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
        public void buildNode(ParserData data)
        {
            switch (data.Type)
            {
                case "AND":
                    _currentNode = new AndNode(data.Id);
                    break;
                case "OR":
                    _currentNode = new OrNode(data.Id);
                    break;
                case "INPUT_HIGH":
                    _currentNode = new InputNode(data.Id, true);
                    break;
                case "INPUT_LOW":
                    _currentNode = new InputNode(data.Id, false);
                    break;
                case "PROBE":
                    _currentNode = new OutputNode(data.Id);
                    break;
                case "NOT":
                    _currentNode = new NotNode(data.Id);
                    break;
                case "NAND":
                    _currentNode = new NotNode(data.Id);
                    break;
                default:
                    throw new NotImplementedException();
            }
            allNodes.Add(data.Id, _currentNode);
        }

        public void addInputs(ParserData data, Action<string> showErrorPopup)//TODO: maybe make circuitbuilder responsible for allnode collection
        {
            bool hasSetXY = false;
            if (data.Id == "AND3")
                Console.WriteLine("e!");
            foreach (string output in data.Ouputs)
            {
                if (allNodes.ContainsKey(output))
                {
                    allNodes[output].X = allNodes[data.Id].X >= allNodes[output].X ? allNodes[data.Id].X + 1 : allNodes[output].X;
                    var sameLevelNodes = allNodes
                        .Where(d => d.Value.X == allNodes[data.Id].X && d.Value.Y >= allNodes[data.Id].Y).ToList();
                    allNodes[data.Id].Y = sameLevelNodes.Count > 0
                        ? sameLevelNodes.Max(n => n.Value.Y) + 1
                        : allNodes[data.Id].Y;
                    allNodes[output].Inputs.Add(allNodes[data.Id]);
                    hasSetXY = true;
                }
                else
                {
                    showErrorPopup.Invoke("Node tries to connect to a non-existing node");
                }
            }
            if (!hasSetXY) //means we need to set this nodes X and Y just to be sure we don't draw a bunch of nodes on top of eachother
            {//TODO: theres no guaraintee that the output node will be the last, maybe seperate loop?
                //TODO: if it isn't the last it fucks up the entire fucking hierarchy
                var sameLevelNodes = allNodes
                    .Where(d => d.Value.X == allNodes[data.Id].X && d.Value.Y >= allNodes[data.Id].Y).ToList();
                allNodes[data.Id].Y = sameLevelNodes.Max(n => n.Value.Y) + 1;
            }
        }
        public void addDelay(double delay)
        {
            _currentNode.Delay = delay;
        }

        public BaseNode getNode()
        {
            return this._currentNode;
        }

        public List<BaseNode> getAllNodes()
        {
            return this.allNodes.Values.ToList(); ;
        }

        public void reset()
        {
            this.allNodes = null;
            this._currentNode = null;
        }
    }
}
