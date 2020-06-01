using Circuits.Models;
using Circuits.Models.Nodes;
using Circuits.Models.Nodes.NodeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace DP1_Circuits.builders
{
    public class CircuitBuilder
    {
        private readonly NodeBuilder _nodeBuilder;

        public CircuitBuilder()
        {
            _nodeBuilder = new NodeBuilder();
        }
        /// <summary>
        /// Builds a circuit
        /// </summary>
        /// <param name="parserData">the data out of which the circuit will be built</param>
        /// <param name="showErrorPopup">the error popup that the circuit can fire during validation</param>
        /// <returns>the completed circuit</returns>
        public Circuit BuildCircuit(List<ParserData> parserData, Action<string> showErrorPopup)
        {
            if(parserData.Count > 0)
            {
                var header = parserData[0];
                parserData.RemoveAt(0);
                Circuit circuit = new Circuit(header.HeaderData);
                Dictionary<string, BaseNode> allNodes = new Dictionary<string, BaseNode>();

                foreach (ParserData data in parserData)
                {
                    _nodeBuilder.BuildNode(data)
                        .AddComponent(new VisualComponent(_nodeBuilder.GetNode()))
                        .AddComponent(new CalcComponent(_nodeBuilder.GetNode()))
                        .AddComponent(new DelayComponent(_nodeBuilder.GetNode(), 15));
                    allNodes.Add(data.Id, _nodeBuilder.GetNode());
                }

                foreach (ParserData data in parserData)
                {
                    _nodeBuilder.AddInputs(data, allNodes, showErrorPopup);
                }

                var outputNodes = allNodes.Values.ToList();
                //this makes sure the y position of all nodes are correct
                List<int> checkedX = new List<int>();
                foreach (var node in outputNodes)
                {
                    if(!checkedX.Contains(node.X))
                    {
                        _nodeBuilder.CheckY(node.Id, allNodes);
                        checkedX.Add(node.X);
                    }
                }
                circuit.AllNodes = allNodes.Values.ToList();
                circuit.EndNodes = allNodes.Values.OfType<OutputNode>().Cast<BaseNode>().ToList();
                if (ValidateCircuit(circuit, showErrorPopup))
                {
                    _nodeBuilder.Reset();
                    return circuit;
                }
                else
                {
                    showErrorPopup.Invoke("something went wrong with validating your circuit");
                    return null;
                }
            }
            showErrorPopup.Invoke("parserData is empty");
            return null;
        }

        private bool ValidateCircuit(Circuit circuit, Action<string> showErrorPopup)
        {
            ValidateCircuitAbnormality(circuit, showErrorPopup);
            return !ValidateCircuitCircularity(circuit, showErrorPopup)
                && ValidateOutputInputConnectivity(circuit, showErrorPopup)
                && ValidateCircuitNotConnected(circuit, showErrorPopup);
        }
        /// <summary>
        /// checks the circuit for nodes that do not have outputs
        /// </summary>
        /// <param name="circuit"></param>
        /// <param name="showErrorPopup"></param>
        /// <returns>wether the circuit has nodes without outputs</returns>
        public bool ValidateCircuitNotConnected(Circuit circuit, Action<string> showErrorPopup)
        {
            Dictionary<BaseNode, double> checkedNodes = new Dictionary<BaseNode, double>();
            foreach (var node in circuit.AllNodes)
            {
                if(!checkedNodes.ContainsKey(node) && node.GetInputs().Count > 0)
                    checkedNodes.Add(node, 0);
                foreach (var input in node.GetInputs())
                {
                    if (checkedNodes.ContainsKey(input))
                        checkedNodes[input]++;
                    else
                        checkedNodes.Add(input, 1);
                }
            }
            foreach (var node in circuit.AllNodes)
            {
                if (node.GetType() != typeof(OutputNode))
                {
                    if (checkedNodes.ContainsKey(node) && checkedNodes[node] < 1)
                    {
                        showErrorPopup.Invoke("Circuit contains connected nodes with no output.");
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// checks if the all outputNodes can reach an inputNode
        /// </summary>
        /// <param name="circuit"></param>
        /// <param name="showErrorPopup"></param>
        /// <returns>wether all outputNodes can connect to an inputnode</returns>
        public bool ValidateOutputInputConnectivity(Circuit circuit, Action<string> showErrorPopup)
        {
            bool canReachInput(BaseNode curNode)
            {
                if (curNode.GetType() == typeof(InputNode)) return true;
                else
                {
                    foreach (var nextNode in curNode.GetInputs())
                    {
                        if (canReachInput(nextNode)) return true;
                    }
                }
                showErrorPopup.Invoke("Non reachable node detected in the circuit");
                return false;
            }
            return circuit.AllNodes.OfType<OutputNode>().All(n => canReachInput(n));
        }
        /// <summary>
        /// validates the circuit for circular references
        /// </summary>
        /// <param name="circuit"></param>
        /// <param name="showErrorPopup"></param>
        /// <returns>wether the circuit contains a circular reference</returns>
        public bool ValidateCircuitCircularity(Circuit circuit, Action<string> showErrorPopup)
        {
            bool returnValue = false;
            foreach (BaseNode node in circuit.AllNodes)
            {
                if (ValidateCircular(node))
                {
                    showErrorPopup.Invoke("Circuit contains circular reference");
                    return true;
                }

            }
            return returnValue;
        }
        /// <summary>
        /// validates the node for circular references in its inputs
        /// </summary>
        /// <param name="curNode"></param>
        /// <returns>wether the inputs contain a circular reference</returns>
        private bool ValidateCircular(BaseNode curNode)
        {
            bool isCircular(BaseNode node, BaseNode baseNode, IReadOnlyList<BaseNode> prevNodes)
            {
                return (node.GetInputs() == null || node.GetInputs().Count == 0) || node.GetInputs().All(n => {
                        if(n == baseNode)
                        {
                            return false;
                        }
                        if (prevNodes.Contains(n))
                        {
                            return true;
                        }
                        List<BaseNode> nodes = prevNodes.Append(n).ToList();
                        return isCircular(n,  baseNode, nodes);
                    });
            }
            return !curNode.GetInputs().All(n  =>  isCircular(n, curNode, new List<BaseNode> { curNode }));
        }
        /// <summary>
        /// checks all node for abnormalities, will give the user a pop up if something is found, 
        /// this function is purely for user friendlyness it does not actually validate anything.
        /// </summary>
        /// <param name="circuit">the circuit to be checked for abnormalities</param>
        /// <param name="showErrorPopup">the error popup action</param>
        private void ValidateCircuitAbnormality(Circuit circuit, Action<string> showErrorPopup)
        {
            void isAbnormal(BaseNode node)
            {
                if (node.GetType() == typeof(NotNode) && node.GetInputs().Count > 1) showErrorPopup.Invoke("Your circuit contains a NOT node with more than one input \n Circuit will ignore all but the first");
            }
            foreach (var node in circuit.AllNodes)
            {
                isAbnormal(node);
            }
        }
    }
}
