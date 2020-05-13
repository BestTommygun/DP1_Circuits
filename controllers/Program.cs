using DP1_Circuits.controllers;
using System;
using System.Windows.Forms;

namespace DP1_Circuits
{
    public static class Program
    {
        public delegate void LogDelegate(string text);
        public static LogDelegate log; //send text to log
        public delegate void CommandDelegate(string commandText);
        public static event CommandDelegate CommandRecievedEvent; //get text from log

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            log.Invoke("────────────[warming up logger...]────────────");
            MainController mainController = new MainController();
        }

        public static void SendCommand(string commandText) => CommandRecievedEvent?.Invoke(commandText);
    }
}
