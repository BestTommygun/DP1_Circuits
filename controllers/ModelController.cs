using Circuits.Models;
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

        public void setCircuit(Circuit newCircuit) //TODO: maybe only set the endNodes
        {
            _circuit = newCircuit;
            bool outputtest1 = _circuit.EndNodes[0].calcOutput();
            bool outputtest2 = _circuit.EndNodes[1].calcOutput();
            var test = "eeeeeeeeeeeeeee";
        }
    }
}
