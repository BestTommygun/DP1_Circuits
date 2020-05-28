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
        private readonly IProgress<byte> threadSafeRefresh;
        public delegate void FileOpenedHandler(string file);
        public event FileOpenedHandler FileOpened;

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
        /// <summary>
        /// resets all the nodes, refreshes the view (since the view is on a seperate thread we must use the IProgress workaround for a thread safe refresh)
        /// </summary>
        /// <param name="allNodes">the new nodes to be drawn</param>
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
