using Circuits.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Circuits.Models;
using Circuits.Models.Nodes.NodeComponents;
using System.CodeDom;

namespace DP1_Circuits.builders
{
    public class NodeBuilder
    {
        private BaseNode _currentNode;
        public NodeBuilder BuildNode(ParserData data)
        {
            _currentNode = data.Type switch 
            {
                "AND" => new AndNode(data.Id),
                "OR" => new OrNode(data.Id),
                "INPUT_HIGH" => new InputNode(data.Id, true),
                "INPUT_LOW" => new InputNode(data.Id, false),
                "PROBE" => new OutputNode(data.Id),
                "NOT" => new NotNode(data.Id),
                "NAND" => new NotNode(data.Id),
                _ => throw new NotImplementedException()
            };
            return this;
        }
        public NodeBuilder AddComponent(Component component)
        {
            if (_currentNode.ContainsComponent(component))
                throw new InvalidOperationException();

            if(component.GetType() == typeof(CalcComponent))
            {
                _currentNode.AddComponent(_currentNode.GetType().Name switch
                {
                    "OrNode" => new ORCalcComponent(_currentNode),
                    "AndNode" => new ANDCalcComponent(_currentNode),
                    "NotNode" => new NOTCalcComponent(_currentNode),
                    "InputNode" => null,
                    "OutputNode" => null,
                    _ => throw new NotImplementedException()
                });
            }
            else
                _currentNode.AddComponent(component);
            return this;
        }

        public void AddInputs(ParserData data, Dictionary<string, BaseNode> allNodes, Action<string> showErrorPopup)
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
        public void CheckY(string nodeId, Dictionary<string, BaseNode> allNodes)
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
        public void AddDelay(double delay)
        {
            _currentNode.Delay = delay;
        }

        public BaseNode GetNode()
        {
            return this._currentNode;
        }

        public void Reset()
        {
            this._currentNode = null;
        }
    }
}
