﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DP1_Circuits.view
{
    public partial class ValidationPopup : Form
    {
        public ValidationPopup()
        {
            InitializeComponent();
        }
        public void SetMessage(string text)
        {
            this.ErrorMessage.Text = text;
        }

        private void OkayButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}