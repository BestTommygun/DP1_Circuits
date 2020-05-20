using Circuits.Models;
using Circuits.Models.Nodes;
using DP1_Circuits.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DP1_Circuits.controllers
{
    public class ViewController
    {
        private readonly MainView mainView;
        public delegate void FileOpenedHandler(string file);
        public event FileOpenedHandler FileOpened;
        private readonly IProgress<byte> threadSafeRefresh;

        public ViewController()
        {
            mainView = new MainView();
            mainView.FileOpened += (string file) => FileOpened(file);
            threadSafeRefresh = new Progress<byte>(e  => { this.mainView.Refresh(); });
        }
        public void RunView()
        {
            Application.Run(mainView);
        }

        public void DrawFrame(List<BaseNode> allNodes)
        {
            mainView.SetNodes(allNodes);
            threadSafeRefresh.Report(0);
        }
        public void ShowErrorPopup(string message)
        {
            mainView.DisplayPopup(message);
        }
    }
}
