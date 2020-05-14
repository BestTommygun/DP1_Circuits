using DP1_Circuits;
using DP1_Circuits.controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Circuits.ConsoleInput.Options
{
    public class RunCommand : BaseCommand
    {
        public RunCommand(MainController receiver)
            : base(receiver)
        {
        }
        public override void Execute(string commandText)
        {
            var results = receiver.runSimulation();

            Program.log.Invoke("───────[ran simulation with the following results:]──────");
            foreach (var result in results.Item1)
            {
                Program.log.Invoke("> " + result.Item1.Id + ": " + result.Item2);
            }
            Program.log.Invoke("simulation took " + results.Item2 + " nanoseconds");
        }
    }
}
