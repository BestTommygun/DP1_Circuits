using Circuits.ConsoleInput.Options;
using Circuits.Models.Nodes;
using DP1_Circuits.controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP1_Circuits.Commands
{
    public class ShowInputsCommand : BaseCommand
    {
        public ShowInputsCommand(MainController receiver)
            : base(receiver)
        { }

        public override void Execute(string commandText)
        {
            var inputs = receiver.GetInputs();
            if(inputs != null && inputs.Count > 0)
            {
                Program.log.Invoke("─────────[inputs: ]────────");
                foreach (InputNode input in inputs)
                {
                    Program.log.Invoke("> " + input.Id + ": " + input.CurrentValue);
                }
            }
            else
            {
                Program.log.Invoke("───────────────────────────");
                Program.log.Invoke("no inputs were found");
            }
            Program.log.Invoke("───────────────────────────");
            Program.log.Invoke("please use a space for ");
            Program.log.Invoke("the node you want to change");
            Program.log.Invoke("I.E: changeInput node3");
            Program.log.Invoke("───────────────────────────");
        }
    }
}
