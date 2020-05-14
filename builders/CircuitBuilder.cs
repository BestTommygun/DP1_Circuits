using Circuits.Models;
using Circuits.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace DP1_Circuits.builders
{
    public class CircuitBuilder
    {
        private NodeBuilder _nodeBuilder;

        public CircuitBuilder()
        {
            _nodeBuilder = new NodeBuilder();
        }
        public Circuit buildCircuit(List<ParserData> parserData, Action<string> showErrorPopup)
        {
            if(parserData.Count > 0)
            {
                Circuit circuit = new Circuit();

                foreach (ParserData data in parserData)
                {
                    _nodeBuilder.buildNode(data);
                }

                foreach (ParserData data in parserData)
                {
                    _nodeBuilder.addInputs(data, showErrorPopup);
                    if (_nodeBuilder.allNodes[data.Id].GetType().Equals(typeof(OutputNode))) circuit.EndNodes.Add(_nodeBuilder.allNodes[data.Id]);
                }
                circuit.AllNodes = _nodeBuilder.getAllNodes();
                if (validateCircuit(circuit, showErrorPopup))
                {
                    _nodeBuilder.reset();
                    return circuit;
                }
                else
                {
                    //showErrorPopup.Invoke("something went wrong with validating your circuit");
                    return null;
                }
            }
            showErrorPopup.Invoke("parserData is empty");
            return null;
        }

        private bool validateCircuit(Circuit circuit, Action<string> showErrorPopup)
        {
            return !validateCircuitCircularity(circuit, showErrorPopup) && validateOutputInputConnectivity(circuit, showErrorPopup) && validateCircuitNotConnected(circuit, showErrorPopup);
        }

        public bool validateCircuitNotConnected(Circuit circuit, Action<string> showErrorPopup)
        {
            Dictionary<BaseNode, double> checkedNodes = new Dictionary<BaseNode, double>();
            foreach (var node in circuit.AllNodes)
            {
                if(!checkedNodes.ContainsKey(node) && node.Inputs.Count > 0)
                    checkedNodes.Add(node, 0);
                foreach (var input in node.Inputs)
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

        public bool validateOutputInputConnectivity(Circuit circuit, Action<string> showErrorPopup)
        {
            bool canReachInput(BaseNode curNode)
            {
                if (curNode.GetType() == typeof(InputNode)) return true;
                else
                {
                    foreach (var nextNode in curNode.Inputs)
                    {
                        if (canReachInput(nextNode)) return true;
                    }
                }
                showErrorPopup.Invoke("Non reachable node detected in the circuit");
                return false;
            }
            return circuit.AllNodes.Where(n => n.GetType() == typeof(OutputNode)).All(n => canReachInput(n));
        }

        public bool validateCircuitCircularity(Circuit circuit, Action<string> showErrorPopup)
        {
            bool returnValue = false;
            foreach (BaseNode node in circuit.AllNodes)
            {
                if (validateCircular(node))
                {
                    showErrorPopup.Invoke("Circuit contains circular reference");
                    return true;
                }

            }
            return returnValue;
        }


        private bool validateCircular(BaseNode curNode)
        {
            bool isCircular(BaseNode node, BaseNode baseNode, IReadOnlyList<BaseNode> prevNodes)
            {
                return (node.Inputs == null || node.Inputs.Count == 0)
                    ? true
                    : node.Inputs.All(n => {
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
            return !curNode.Inputs.All(n  =>  isCircular(n, curNode, new List<BaseNode> { curNode }));
        }
    }
}
