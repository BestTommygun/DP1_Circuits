using Circuits.ConsoleInput.Options;
using DP1_Circuits.Commands;
using DP1_Circuits.controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP1_Circuits.builders
{
    public class OptionsBuilder
    {
        private string _currentText;
        private BaseCommand _currentCommand;
        private readonly MainController _currentController;
        private Dictionary<string, BaseCommand> _commands;
        public OptionsBuilder(MainController controller)
        {
            _currentController = controller;
        }
        public OptionsBuilder AddCommand(string commandText) //TODO: maybe implement cachedrun?
        {
            if (_commands == null) _commands = new Dictionary<string, BaseCommand>();
            _currentText = commandText;
            _currentCommand = commandText switch
            {
                "run" => new RunCommand(_currentController),
                "inputs" => new ShowInputsCommand(_currentController),
                "changeinput" => new ChangeInputCommand(_currentController),
                _ => throw new NotImplementedException()
            };
            _commands.Add(_currentText, _currentCommand);
            return this;
        }
        public OptionsBuilder AddHelpCommand(List<string> commands)
        {
            _currentText = "help";
            _currentCommand = new HelpCommand(_currentController, commands);
            _commands.Add(_currentText, _currentCommand);
            return this;
        }
        public Dictionary<string, BaseCommand> GetCommands()
        {
            return _commands;
        }
    }
}
