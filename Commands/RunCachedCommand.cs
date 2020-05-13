using Circuits.ConsoleInput.Options;
using DP1_Circuits.controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP1_Circuits.Commands
{
    public class RunCachedCommand : BaseCommand
    {
        public RunCachedCommand(MainController receiver)
           : base(receiver)
        { }

        public override void Execute(string commandText)
        {
            base.Execute(commandText);
        }
    }
}
