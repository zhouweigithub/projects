using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wzq_bll;

namespace wzq
{
    public partial class Form1 : Form
    {
        private bll bll;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bll = new bll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bll.reset();
            pictureBox1.Image = Properties.Resources.background;
            pictureBox1.Refresh();
            timer1.Stop();
            button3.Text = "托管";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image img = bll.regret();
            pictureBox1.Image = img;
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //textBox3.Text = e.X.ToString();
            //textBox4.Text = e.Y.ToString();

            (int px1, int py1) = bll.getPoint(e.X, e.Y);
            textBox1.Text = py1.ToString();
            textBox2.Text = px1.ToString();

            Image img = bll.humanGo(e.X, e.Y, pictureBox1.Image);
            if (img != null)
            {
                pictureBox1.Image = img;
                pictureBox1.Refresh();

                (int x, int y) = bll.getPoint(e.X, e.Y);
                if (bll.iswin(y, x))
                {
                    button3.Text = "托管";
                    MessageBox.Show("you win", "over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    timer1.Start();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!bll.IsAutoPlaying)
                timer1.Stop();

            (int px, int py, Image img) = bll.aiGo(pictureBox1.Image);
            if (img != null)
            {
                pictureBox1.Image = img;
                pictureBox1.Refresh();
                if (bll.iswin(px, py))
                {
                    timer1.Stop();
                    button3.Text = "托管";
                    string msg = bll.IsHumanGo ? "you win" : "you lost";
                    MessageBoxIcon icon = bll.IsHumanGo ? MessageBoxIcon.Information : MessageBoxIcon.Warning;
                    MessageBox.Show(msg, "over", MessageBoxButtons.OK, icon);
                }
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bll.IsAutoPlaying = !bll.IsAutoPlaying;
            button3.Text = bll.IsAutoPlaying ? "结束托管" : "托管";
            if (bll.IsAutoPlaying)
                timer1.Start();
        }
    }
}
