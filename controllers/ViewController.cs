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
        private MainView mainView;
        public delegate void FileOpenedHandler(string file);
        public event FileOpenedHandler fileOpened;
        private IProgress<byte> threadSafeRefresh;

        public ViewController()
        {
            mainView = new MainView();
            mainView.fileOpened += (string file) => fileOpened(file);
            threadSafeRefresh = new Progress<byte>(e  => { this.mainView.Refresh(); });
        }
        public void runView()
        {
            Application.Run(mainView);
        }

        public void drawFrame(List<BaseNode> allNodes)
        {
            mainView.setNodes(allNodes);
            threadSafeRefresh.Report(0);
        }
        public void showErrorPopup(string message)
        {
            mainView.displayPopup(message);
        }
    }
}
