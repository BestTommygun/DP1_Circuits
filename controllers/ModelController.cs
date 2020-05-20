using Circuits.Models;
using Circuits.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP1_Circuits.controllers
{
    public class ModelController
    {
        private Circuit _circuit;

        public ModelController()
        {

        }
        public string GetCircuitName()
        {
            return _circuit.Name;
        }
        public void SetCircuit(Circuit newCircuit)
        {
            _circuit = newCircuit;
        }

        public List<BaseNode> GetNodes()
        {
            return _circuit?.AllNodes;
        }
        public List<InputNode> GetInputs()
        {
            return _circuit?.GetInputs();
        }
        public void ResetNodes()
        {
            _circuit?.AllNodes.ForEach(n => n.SavedOutput = null);
        }
        public Tuple<List<Tuple<BaseNode, bool>>, double> RunSim()
        {
            double executionTime = 0;
            List<Tuple<BaseNode, bool>> outputs = new List<Tuple<BaseNode, bool>>();
            if(_circuit != null && _circuit.EndNodes != null)
            {
                foreach (BaseNode node in _circuit.EndNodes)
                {
                    var curOutput = node.CalcOutput();
                    executionTime += curOutput.Item2;
                    outputs.Add(new Tuple<BaseNode, bool>(node, curOutput.Item1));
                }
            }
            return new Tuple<List<Tuple<BaseNode, bool>>, double>(outputs, executionTime);
        }
    }
}
