using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AlertTime
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// 界面显示时长（秒）
        /// </summary>
        const Int16 waitSeconds = 10;
        /// <summary>
        /// 界面是否显示
        /// </summary>
        private Boolean isVisible = false;
        /// <summary>
        /// 界面已显示时长（秒）
        /// </summary>
        private Int16 waitedSeconds = 0;


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SetWindowPos(IntPtr hWnd, Int32 hWndInsertAfter, Int32 x, Int32 y, Int32 Width, Int32 Height, Int32 flags);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(Object sender, EventArgs e)
        {
            this.Opacity = 0;
            this.timer1.Interval = 1000;
            this.timer1.Tick += Timer1_Tick;
            this.notifyIcon1.Text = "timer manager";
            this.timer1.Start();
            this.ShowInTaskbar = false;

            this.BeginInvoke(new Action(() =>
            {
                this.Hide();
                this.Opacity = 1;
            }));
        }

        private void Timer1_Tick(Object sender, EventArgs e)
        {
            if (isVisible)
            {
                if (waitedSeconds <= waitSeconds)
                {
                    //未到显示的时长
                    waitedSeconds++;
                }
                else
                {
                    //显示时间到，隐藏界面
                    this.Hide();
                    isVisible = false;
                }
            }

            if (DateTime.Now.Second != 0)
                return;

            DateTime now = DateTime.Now;

            if (now.Minute % 10 == 0)
            {
                String time = now.ToString("HH:mm");
                String msg = GetMessage();
                this.notifyIcon1.ShowBalloonTip(1000, "清风报时", $"现在时间 {time}\n{msg}", ToolTipIcon.Info);
            }

            //整点
            if (now.Minute == 0)
            {
                String msg = GetMessage();
                this.label3.Text = now.Hour.ToString();
                waitedSeconds = 0;
                isVisible = true;
                this.Show();

                //窗口置顶
                SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 1 | 2);
            }

        }


        private static List<String> GetMessages()
        {
            String path = Application.StartupPath + "\\message.txt";
            List<String> lines = File.ReadAllLines(path, Encoding.GetEncoding("GB2312")).ToList();
            lines.RemoveAll(a => a == String.Empty);
            return lines;
        }

        private static String GetMessage()
        {
            List<String> msgs = GetMessages();
            Random rnd = new Random();
            Int32 index = rnd.Next(0, msgs.Count);
            return msgs[index];
        }
    }
}
