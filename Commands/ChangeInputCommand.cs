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
    public class ChangeInputCommand : BaseCommand
    {
        public ChangeInputCommand(MainController receiver)
            : base(receiver)
        { }

        public override void Execute(string commandText)
        {
            InputNode input = receiver.GetInputs()?.FindAll(n => n.GetType() == typeof(InputNode)).Cast<InputNode>().ToList()
                .Find(n => n.Id.Equals(commandText.Split(' ')[1]));
            if(input != null)
            {
                input.CurrentValue = !input.CurrentValue;
                receiver.ResetCircuit();
                Program.log.Invoke("> " + input.Id + " has been changed to " + input.CurrentValue);
            }
            else
            {
                Program.log.Invoke("> node " + commandText.Split(' ')[1] + " does not exist");
            }

        }
    }
}
