using DP1_Circuits.controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.ConsoleInput.Options
{
    public abstract class BaseCommand
    {
        protected MainController receiver;

        public BaseCommand(MainController receiver)
        {
            this.receiver = receiver;
        }
        public virtual void Execute(string commandText) { throw new Exception("Execution of base command class not possible"); }
    }
}
