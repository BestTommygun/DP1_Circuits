using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.ConsoleInput.Options
{
    public class Invoker //TODO: actually use this stupid class since the command pattern is bloated bullshit that has to be used even though it's ridiculously inefficient
    {
        private BaseCommand _command;

        public void SetCommand(BaseCommand command)
        {
            this._command = command;
        }

        public void ExecuteCommand()
        {
            _command.Execute("ea sperts");
        }
    }
}
