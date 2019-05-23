// ****************************************
// FileName:MailUtil.cs
// Description:邮件助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace Util.Mail
{
    using Util.Log;

    /// <summary>
    /// 邮件助手类
    /// </summary>
    public static class MailUtil
    {
        //定义邮件服务器及系统账户信息
        private static String mMailHost = String.Empty;
        private static String mSenderAddress = String.Empty;
        private static String mSenderPassword = String.Empty;

        #region 内部调用方法

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">收件人</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="attachFiles">附件路径</param>
        /// <param name="isBodyHtml">内容是否为Html</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.ObjectDisposedException"></exception>
        /// <exception cref="System.Net.Mail.SmtpException"></exception>
        /// <exception cref="System.Net.Mail.SmtpFailedRecipientsException"></exception>        
        private static void Send(IEnumerable<String> mailTo, String subject, String body, IEnumerable<String> attachFiles, Boolean isBodyHtml)
        {
            MailMessage mailMessage = new MailMessage();

            try
            {
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.From = new MailAddress(mSenderAddress);
                mailMessage.IsBodyHtml = isBodyHtml;
                foreach (String to in mailTo)
                {
                    mailMessage.To.Add(to);
                }

                //创建附件
                if (attachFiles != null && attachFiles.Count() > 0)
                {
                    foreach (String attach in attachFiles)
                    {
                        mailMessage.Attachments.Add(new Attachment(attach));
                    }
                }

                //初始化邮件发送客户端
                SmtpClient smtp = new SmtpClient(mMailHost)
                {
                    Credentials = new NetworkCredential(mSenderAddress, mSenderPassword)
                };

                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mailMessage.Attachments != null)
                {
                    mailMessage.Attachments.Dispose();
                }
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="state">相关参数</param>
        private static void Send(Object state)
        {
            //获得参数
            Dictionary<String, Object> param = (Dictionary<String, Object>)state;

            IEnumerable<String> mailTo = (IEnumerable<String>)param["MailTo"];
            String subject = param["Subject"].ToString();
            String body = param["Body"].ToString();
            Boolean isBodyHtml = Convert.ToBoolean(param["IsBodyHtml"]);
            IEnumerable<String> attachFiles = null;
            if (param.ContainsKey("AttachFiles"))
            {
                attachFiles = (IEnumerable<String>)param["AttachFiles"];
            }

            try
            {
                Send(mailTo, subject, body, attachFiles, isBodyHtml);
            }
            catch (Exception ex)
            {
                LogUtil.Write(ex.StackTrace == null ? ex.Message : ex.StackTrace + Environment.NewLine + ex.Message, LogType.Error);
            }
        }

        #endregion

        #region 供外部调用的方法

        /// <summary>
        /// 设置发送者信息（一次设定，永久生效）
        /// </summary>
        /// <param name="mailHost">邮箱主机地址</param>
        /// <param name="address">发送者邮件地址</param>
        /// <param name="password">发送者密码</param>
        public static void SetSenderInfo(String mailHost, String address, String password)
        {
            mMailHost = mailHost;
            mSenderAddress = address;
            mSenderPassword = password;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">收件人地址</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="sendPattern">发送模式</param>
        /// <param name="isBodyHtml">内容是否为Html</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.ObjectDisposedException"></exception>
        /// <exception cref="System.Net.Mail.SmtpException"></exception>
        /// <exception cref="System.Net.Mail.SmtpFailedRecipientsException"></exception>
        public static void SendMail(String[] mailTo, String subject, String body, SendPattern sendPattern, Boolean isBodyHtml = false)
        {
            if (mailTo == null || mailTo.Length == 0) throw new ArgumentNullException("mailTo", "mailTo can't be null or empty");
            if (String.IsNullOrEmpty(subject)) throw new ArgumentNullException("subject", "subject can't be null or empty");
            if (String.IsNullOrEmpty(body)) throw new ArgumentNullException("body", "body can't be null or empty");

            if (sendPattern == SendPattern.Synchronous)
            {
                Send(mailTo, subject, body, null, isBodyHtml);
            }
            else
            {
                Dictionary<String, Object> param = new Dictionary<String, Object>();
                param["MailTo"] = mailTo;
                param["Subject"] = subject;
                param["Body"] = body;
                param["IsBodyHtml"] = isBodyHtml;

                ThreadPool.QueueUserWorkItem(new WaitCallback(Send), param);
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">收件人</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="attachFiles">附件</param>
        /// <param name="sendPattern">发送模式</param>
        /// <param name="isBodyHtml">内容是否为Html</param>
        public static void SendMail(IEnumerable<String> mailTo, String subject, String body, IEnumerable<String> attachFiles, SendPattern sendPattern, Boolean isBodyHtml = false)
        {
            if (mailTo == null || mailTo.Count() == 0)
                throw new ArgumentNullException("mailTo", "mailTo can't be null or empty");
            if (String.IsNullOrEmpty(subject))
                throw new ArgumentNullException("subject", "subject can't be null or empty");
            if (String.IsNullOrEmpty(body))
                throw new ArgumentNullException("body", "body can't be null or empty");

            if (sendPattern == SendPattern.Synchronous)
            {
                Send(mailTo, subject, body, attachFiles, isBodyHtml);
            }
            else
            {
                Dictionary<String, Object> param = new Dictionary<String, Object>();
                param["MailTo"] = mailTo;
                param["Subject"] = subject;
                param["Body"] = body;
                param["IsBodyHtml"] = isBodyHtml;
                if (attachFiles != null && attachFiles.Count() > 0)
                {
                    param["AttachFiles"] = attachFiles;
                }                

                ThreadPool.QueueUserWorkItem(new WaitCallback(Send), param);
            }
        }

        #endregion
    }
}