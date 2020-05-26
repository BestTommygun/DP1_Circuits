using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Circuits.logger
{
    /// <summary>
    /// A very simple seperate console logger for registering user input and printing status messages.
    /// This was built to demonstrate more patterns
    /// </summary>
    static class Program
    {

        static void Main()
        {
            Thread thread = new Thread(DP1_Circuits.Program.Main);
            DP1_Circuits.Program.log = Program.Writeline;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            while (thread.IsAlive)
            {
                string commandText = Console.ReadLine();
                DP1_Circuits.Program.SendCommand(commandText);
            }
            thread.Join();
        }

        public static void Writeline(string line)
        {
            Console.WriteLine(line);
        }
    }
}
