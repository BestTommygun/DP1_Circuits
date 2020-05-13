using Circuits.Models.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DP1_Circuits.view
{
    public partial class MainView : Form
    {
        public MainView()
        {
            InitializeComponent();
        }

        public delegate void FileOpenedHandler(string file);
        public event FileOpenedHandler fileOpened;

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = ".txt";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "TXT files (*.txt)|*.txt|XML files (*.xml)|*.xml";
            openFileDialog1.Title = "Open a circuit File...";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileDialog1.FileOk += (object s, CancelEventArgs Cargs) => fileOpened(openFileDialog1.FileName);
            }
        }

        public void setNodes(List<BaseNode> allNodes)
        {
            simulationView1.Nodes = allNodes;
        }

        public void displayPopup(string message)
        {
            ValidationPopup errorPopup = new ValidationPopup();
            errorPopup.setMessage(message);
            errorPopup.ShowDialog();
        }
    }
}
