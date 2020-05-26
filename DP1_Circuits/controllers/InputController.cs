using Circuits.ConsoleInput.Options;
using DP1_Circuits.builders;
using DP1_Circuits.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP1_Circuits.controllers
{
    public class InputController
    {
        private Dictionary<string, BaseCommand> commands;
        public void SetDefaultSetup(MainController mainController)
        {
            OptionsBuilder optionsBuilder = new OptionsBuilder(mainController);
            commands = optionsBuilder
                .AddCommand("run")
                .AddCommand("inputs")
                .AddCommand("changeinput")
                .AddHelpCommand(optionsBuilder.GetCommands().Keys.ToList())
                .GetCommands();
            Program.log.Invoke("> Please enter a command.");
        }

        public InputController()
        {
            Program.CommandRecievedEvent += (string text) => HandleInput(text);
        }

        public void HandleInput(string commandText)
        {
            var inputText = commandText.Split(' ');
            if (commands.ContainsKey(inputText[0]))
            {
                commands[inputText[0]].Execute(commandText);
            }
            else 
            { 
                Program.log.Invoke("> Invalid command..."); 
            }
            Program.log.Invoke("> Please enter a command.");
        }
    }
}
