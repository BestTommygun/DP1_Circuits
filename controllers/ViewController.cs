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
        public ViewController()
        {
            mainView = new MainView();
        }
        public void runView()
        {
            mainView.fileOpened += (string file) => fileOpened(file);
            Application.Run(mainView);
        }
    }
}
