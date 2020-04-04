using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CheckFileRepeat
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string folder = textBox1.Text;
            if (string.IsNullOrWhiteSpace(folder))
            {
                MessageBox.Show("请选择目录");
                return;
            }
            BLL.CheckRepeatFiles(folder, textBox2.Text, checkBox1.Checked);
            List<FileModel> repeatedFiles = BLL.GetRepeatedFiles();
            repeatedFiles = repeatedFiles.OrderByDescending(a => a.RepeatTimes).ThenBy(b => b.FileName).ToList();
            dataGridView1.DataSource = repeatedFiles;
        }

        private void textBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            FileModel currentModle = dataGridView1.Rows[e.RowIndex].DataBoundItem as FileModel;
            DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
            if (column is DataGridViewButtonColumn)
            {
                new RepeatedDetail(currentModle.FileName).ShowDialog();
            }
        }
    }
}
