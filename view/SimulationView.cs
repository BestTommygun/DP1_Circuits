﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Circuits.Models.Nodes;

namespace DP1_Circuits.view
{
    public partial class SimulationView : UserControl
    {
        private const int xOffset = 150;
        private const int yOffset = 75;
        private readonly Size nodeSize = new Size(75, 50);
        private List<BaseNode> _nodes;
        public List<BaseNode> Nodes
        {
            get => _nodes;
            set
            {
                _nodes = value;
                if (value != null)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.SuspendLayout();
                        var X = Nodes.Max(n => n.X);
                        var Y = Nodes.Max(n => n.Y);

                        this.Size = new Size(X * xOffset + 35 * X, Y * yOffset + 35 * Y);
                        this.ResumeLayout();
                    }));
                }
            }
        }
        public SimulationView()
        {
            _nodes = new List<BaseNode>();
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Nodes != null)
            {
                foreach (var node in Nodes)
                {
                    foreach (var inputNode in node.Inputs)
                    {
                        //TODO: cool square lines?
                        //adjust line colour based on last simulation
                        Pen linePen = inputNode.SavedOutput != null && inputNode.SavedOutput.Item1 ? new Pen(Color.Green) : new Pen(Color.Red);
                        linePen.Width = 2;
                        e.Graphics.DrawLine(
                            linePen,
                            new Point(node.X * xOffset + 35, node.Y * yOffset + 12),
                            new Point(inputNode.X * xOffset + nodeSize.Width / 2, inputNode.Y * yOffset + nodeSize.Height / 2)
                            );
                    }
                }

                foreach (var node in Nodes)
                {
                    Color colour = node.SavedOutput != null && node.SavedOutput.Item1 ? Color.Green : Color.Red;
                    e.Graphics.FillRectangle(
                        new SolidBrush(colour),
                        new Rectangle(
                            new Point(node.X * xOffset - 1, node.Y * yOffset - 1),
                            new Size(nodeSize.Width + 2, nodeSize.Height + 2)
                            )
                        );
                    if (node is InputNode n)
                    {
                        var inputColour = n.CurrentValue ? Color.Green : Color.Red;
                        e.Graphics.FillRectangle(
                        new SolidBrush(inputColour),
                        new Rectangle(
                            new Point(node.X * xOffset - 1, node.Y * yOffset - 1),
                            new Size(nodeSize.Width + 2, nodeSize.Height + 2)
                            )
                        );
                    }
                    e.Graphics.FillRectangle(
                        new SolidBrush(Color.DarkGray),
                        new Rectangle(
                            new Point(node.X * xOffset, node.Y * yOffset),
                            nodeSize
                            )
                        );
                    e.Graphics.DrawString(
                        "" + node.GetType().Name,
                        Font,
                        new SolidBrush(Color.Red),
                        new Point(node.X * xOffset + 2, node.Y * yOffset + 1)
                        );
                    e.Graphics.DrawString(
                        "" + node.Id,
                        Font,
                        new SolidBrush(Color.Red),
                        new Point(node.X * xOffset + 2, node.Y * yOffset + 26)
                        );
                }
            }
        }
    }
}
