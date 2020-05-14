using Circuits.ConsoleInput.Options;
using DP1_Circuits.controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP1_Circuits.Commands
{
    public class InsertCircuitCommand : BaseCommand
    {
        public InsertCircuitCommand(MainController receiver)
            : base(receiver)
        { }

        public override void Execute(string commandText)
        {
            if (commandText.Split(' ')[1] != null)
                receiver.insertCircuit(commandText.Split(' ')[1]);
        }
    }
}
