using DP1_Circuits;
using DP1_Circuits.controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.ConsoleInput.Options
{
    public class HelpCommand : BaseCommand
    {
        private readonly List<string> helpCommands;
        public HelpCommand(MainController receiver, List<string> commands) 
            : base(receiver)
        {
            helpCommands = commands;
            helpCommands.Add("help");
        }
        public override void Execute(string commandText)
        {
            Program.log.Invoke("─────────[help: ]────────");
            foreach (string command in helpCommands)
            {
                Program.log.Invoke("> " + command);
            }
            Program.log.Invoke("─────────────────────────");
        }
    }
}
