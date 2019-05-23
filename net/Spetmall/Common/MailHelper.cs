//***********************************************************************************
// 文件名称：SendMail.cs
// 功能描述：邮件帮助类
// 数据表：
// 作者：TP
// 日期：2015-03-30
// 修改记录：
//***********************************************************************************

using Util.Log;
using System;
using System.Net;
using System.Threading;
using System.Net.Mail;

namespace Spetmall.Common
{

    public class MailHelper
    {
        #region 属性定义
        /// <summary>
        /// 主题
        /// </summary>
        private String subject;

        /// <summary>
        /// 内容
        /// </summary>
        private String body;

        /// <summary>
        /// 发送列表
        /// </summary>
        private String[] mailTo;

        /// <summary>
        /// 主邮箱automail2@app366.com
        /// </summary>
        private static String FromEmailAddress = "Monitor10@app366.com";

        /// <summary>
        /// 主邮箱密码Moqikaka366
        /// </summary>
        private static String FromEmailPassword = "Monitor_moqikaka366";

        /// <summary>
        /// 主机
        /// </summary>
        private static String EmailHost = "smtp.exmail.qq.com";

        /// <summary>
        /// 重发次数
        /// </summary>
        private static Int32 ReSendTimes = 3;

        /// <summary>
        /// 重发间隔时间
        /// </summary>
        private static Int32 SleepMinute = 120;

        #endregion

        #region 邮件相关操作

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mailTo">发送列表</param>
        /// <param name="subject">主题</param>
        /// <param name="body">正文</param>
        public MailHelper(String[] mailTo, String subject, String body)
        {
            this.mailTo = mailTo;
            this.subject = subject;
            this.body = body;
        }

        /// <summary>
        /// 前台发送
        /// </summary>
        public void Send()
        {
            Send(mailTo, subject, body, 1);
        }

        /// <summary>
        /// 后台发送
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static void SendAtBackground(String[] mailTo, String subject, String body)
        {
            MailHelper newMail = new MailHelper(mailTo, subject, body);
            Thread thread = new Thread(newMail.Send);
            thread.Start();
        }

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="times"></param>
        private static void Send(String[] mailTo, String subject, String body, Int32 times)
        {
            MailMessage msg = new MailMessage();
            try
            {
                msg.From = new MailAddress(FromEmailAddress);

                foreach (String address in mailTo)
                {
                    msg.To.Add(address);
                }

                msg.Subject = subject;
                msg.Body = body;

                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = FromEmailAddress;
                NetworkCred.Password = FromEmailPassword;
                smtp.Credentials = NetworkCred;
                smtp.Host = EmailHost;
                smtp.Port = 25;
                smtp.Timeout = 500000;
                smtp.Send(msg);
            }
            catch (Exception ex)
            {
                if (times++ < ReSendTimes)
                {
                    Thread.Sleep(SleepMinute);
                    Send(mailTo, subject, body, times);
                }
                else
                {
                    LogUtil.Write(string.Format("邮件发送失败！\r\n{0}\r\n{1}\r\n{2}\r\n{3}", subject, string.Join("|", mailTo), body, ex), LogType.Error);
                }
            }
        }
        #endregion
    }
}
