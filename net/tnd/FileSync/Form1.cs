using System;
using System.Windows.Forms;

namespace FileSync
{
    public partial class Form1 : Form
    {

        private DateTime startTime;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BLL.StartCheckHanlder += BLL_StartCheckHanlder;
            BLL.StartCopyHanlder += BLL_StartCopyHanlder;
            BLL.CoppingHanlder += BLL_CoppingHanlder;
            BLL.EndCopyHanlder += BLL_EndCopyHanlder;
            FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            BLL.Close();
        }

        private void BLL_CoppingHanlder(object sender, EventArgsString e)
        {
            label5.Invoke(new Action(() =>
            {
                label5.Text = "正在复制文件 " + e.Value;
            }));
        }

        private void BLL_StartCopyHanlder(object sender, EventArgs e)
        {
            startTime = DateTime.Now;
            label5.Invoke(new Action(() =>
            {
                label5.Text = "开始复制文件 ";
                timer1.Start();
            }));
        }

        private void BLL_StartCheckHanlder(object sender, EventArgs e)
        {
            label5.Invoke(new Action(() => { label5.Text = "正在检测缺失文件..."; }));
        }

        private void BLL_EndCopyHanlder(object sender, EventArgs e)
        {
            label5.Invoke(new Action(() =>
            {
                timer1.Stop();
                label5.Text = "复制完成";
                button1.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                progressBar1.Value = 100;
                label7.Text = BLL.SuccessCount + "/" + BLL.AllSysncCount;
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("请选择原始目录");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("请选择目标目录");
                return;
            }

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            button1.Enabled = false;
            progressBar1.Value = 0;

            //检测并复制文件
            BLL.Start(textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim());
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox2.Text = folderBrowserDialog1.SelectedPath;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan span = DateTime.Now - startTime;
            label4.Text = span.Hours.ToString().PadLeft(2, '0') + ":"
                + span.Minutes.ToString().PadLeft(2, '0') + ":"
                + span.Seconds.ToString().PadLeft(2, '0');

            label7.Text = BLL.SuccessCount + "/" + BLL.AllSysncCount;

            if (BLL.AllSysncCount > 0)
                progressBar1.Value = (int)((float)BLL.SuccessCount / BLL.AllSysncCount * 100);
            else
                progressBar1.Value = 100;
        }
    }
}
