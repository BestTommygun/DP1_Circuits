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
            foreach (string output in data.Ouputs)
            {
                if (allNodes.ContainsKey(output))
                {
                    allNodes[output].X = allNodes[data.Id].X >= allNodes[output].X ? allNodes[data.Id].X + 1 : allNodes[output].X;
                    var sameLevelNodes = allNodes.Values
                        .Where(n => n.X == allNodes[data.Id].X && n.Y >= allNodes[data.Id].Y).ToList();
                    allNodes[data.Id].Y = sameLevelNodes.Count > 0
                        ? sameLevelNodes.Max(n => n.Y) + 1
                        : allNodes[data.Id].Y;
                    allNodes[output].Inputs.Add(allNodes[data.Id]);
                    hasSetXY = true;
                    var heyImaTest = sameLevelNodes.Aggregate((i1, i2) => i1.Y > i2.Y ? i1 : i2);
                    if (heyImaTest.GetType() == typeof(OutputNode))
                    {
                        sameLevelNodes.Where(n => n.GetType() == typeof(OutputNode)).Cast<OutputNode>().Aggregate((i1, i2) => i1.Y > i2.Y ? i1 : i2).Y = allNodes[data.Id].Y;
                        allNodes[data.Id].Y -= 1;
                    }
                }
                else
                {
                    showErrorPopup.Invoke("Node tries to connect to a non-existing node");
                }
            }
            if (!hasSetXY) //means we need to set this nodes X and Y just to be sure we don't draw a bunch of nodes on top of eachother
            {//TODO: theres no guaraintee that the output node will be the last, maybe seperate loop?
                //TODO: if it isn't the last it fucks up the entire fucking hierarchy
                //TODO: this does space correctly, it's just really fucked up looking
                //TODO: this is needed for a correct format but it should be overwritten?
                var sameLevelNodes = allNodes
                    .Where(d => d.Value.X == allNodes[data.Id].X && d.Value.Y >= allNodes[data.Id].Y).ToList();
                allNodes[data.Id].Y = sameLevelNodes.Max(n => n.Value.Y) + 1;
            }
        }
        //checks the Y of the given ID and if it isn't right will reset the entire row
        public void checkY(string nodeId)
        {
            var sameLevelNodes = allNodes.Values.Where(n => n.X == allNodes[nodeId].X && n.Y >= allNodes[nodeId].Y).ToList();
            if(sameLevelNodes.Count > 0)
            {
                sameLevelNodes.Sort((n1, n2) => n1.Y > n2.Y ? n1.Y : n2.Y);
                if (sameLevelNodes.First().Y > 2)
                {
                    sameLevelNodes.FirstOrDefault().Y = 1;
                    sameLevelNodes.Skip(1).ToList()
                        .ForEach(node => node.Y = sameLevelNodes[sameLevelNodes.IndexOf(node) - 1].Y + 1);
                }
            }
            else
            {
                allNodes[nodeId].Y = 1;
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
            return this.allNodes.Values.ToList();
        }

        public void reset()
        {
            this.allNodes = null;
            this._currentNode = null;
        }
    }
}
