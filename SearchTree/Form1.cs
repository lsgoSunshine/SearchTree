using SearchTree.RedBlackTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SearchTree
{
    public partial class Form1 : Form
    {
        private AVLTree tree ;
        //private RedBlackTree<int> rtree;
        public Form1()
        {
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int x = Convert.ToInt32(textBox1.Text);
            tree.Insert(x);
            textBox1.Text = null;
            this.Invalidate();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (tree == null)
            {
                tree = new AVLTree();
                return;
            }
            Graphics dc = e.Graphics;

            tree.drawTree(dc);
        }
    }
}
