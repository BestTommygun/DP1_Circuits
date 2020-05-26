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
        /// <summary>
        /// builds a node out of parserData
        /// </summary>
        /// <param name="data">the data</param>
        /// <returns>this, so you can easily chain commands</returns>
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
                "NAND" => BuildNAND(data.Id),
                "NOR" => BuildNOR(data.Id),
                "XNOR" => BuildXNOR(data.Id),
                _ => throw new NotImplementedException()
            };
            return this;
        }
        /// <summary>
        /// builds node based on type and gives it an id
        /// </summary>
        /// <param name="type">the node type</param>
        /// <param name="id">the id of the node</param>
        /// <returns>the node</returns>
        private BaseNode BuildNode(string type, string id)
        {
            return type switch
            {
                "AND" => new AndNode(id),
                "OR" => new OrNode(id),
                "INPUT_HIGH" => new InputNode(id, true),
                "INPUT_LOW" => new InputNode(id, false),
                "PROBE" => new OutputNode(id),
                "NOT" => new NotNode(id),
                "NAND" => BuildNAND(id),
                "NOR" => BuildNORVisual(id),
                _ => throw new NotImplementedException()
            };
        }
        private NorNode BuildNORVisual(string id)
        {
            NorNode node = BuildNOR(id);
            node.AddComponent(new VisualComponent(node));
            return node;
        }
        private NorNode BuildNOR(string id)
        {
            OrNode orNode = (OrNode)BuildNode("OR", id + "|OR");
            orNode.AddComponent(new ORCalcComponent(orNode));
            NotNode notNode = (NotNode)BuildNode("NOT", id + "|NOT");
            notNode.AddComponent(new NOTCalcComponent(notNode));
            notNode.AddInput(orNode);
            NorNode node = new NorNode(id, orNode, notNode);
            node.AddComponent(new VisualComponent(node));
            return node;
        }
        private NandNode BuildNAND(string id)
        {
            NorNode nor = BuildNOR(id + "|NOR");
            NandNode node = new NandNode(id, nor);
            node.AddComponent(new VisualComponent(node));
            return node;
        }
        public XNorNode BuildXNOR(string id)
        {
            NorNode one = BuildNOR(id + "|NOR_ONE");
            NorNode two = BuildNOR(id + "|NOR_TWO");
            NorNode three = BuildNOR(id + "|NOR_THREE");
            NorNode four = BuildNOR(id + "|NOR_FOUR");
            four.AddInput(two);
            four.AddInput(three);
            two.AddInput(one);
            three.AddInput(one);
            XNorNode node = new XNorNode(id, one, two, three, four);
            node.AddComponent(new VisualComponent(node));
            return node;
        }
        /// <summary>
        /// adds a component to the current node, only use this after calling the build function
        /// </summary>
        /// <param name="component">the component to be added</param>
        /// <returns>this, for easy chaining</returns>
        public NodeBuilder AddComponent(Component component)
        {
            if (_currentNode == null) throw new InvalidOperationException();
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
                    "NorNode" => null,
                    "XNorNode" => null,
                    "NandNode" => null,
                    _ => throw new NotImplementedException()
                });;
            }
            else
                _currentNode.AddComponent(component);
            return this;
        }
        /// <summary>
        /// adds inputs to the node
        /// </summary>
        /// <param name="data">the node data</param>
        /// <param name="allNodes">the list of all already built nodes</param>
        /// <param name="showErrorPopup">an error popup for internal validation</param>
        public void AddInputs(ParserData data, Dictionary<string, BaseNode> allNodes, Action<string> showErrorPopup)
        {
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
                    allNodes[output].AddInput(allNodes[data.Id]);
                }
                else
                {
                    showErrorPopup.Invoke("Node tries to connect to a non-existing node");
                }
            }
        }
        /// <summary>
        /// checks the Y of the given ID and if it isn't right will reset the entire row
        /// </summary>
        /// <param name="nodeId">the node to be checked</param>
        /// <param name="allNodes">all nodes</param>
        public void CheckY(string nodeId, Dictionary<string, BaseNode> allNodes)
        {
            var sameLevelNodes = allNodes.Values.Where(n => n.X == allNodes[nodeId].X && n.Y >= allNodes[nodeId].Y).ToList();
            if(sameLevelNodes.Count > 0)
            {
                sameLevelNodes.Sort((n1, n2) => n1.Y > n2.Y ? n1.Y : n2.Y);
                sameLevelNodes[0].Y = 1;
                sameLevelNodes.Skip(1).ToList()
                    .ForEach(node => node.Y = sameLevelNodes[sameLevelNodes.IndexOf(node) - 1].Y + 1);
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
