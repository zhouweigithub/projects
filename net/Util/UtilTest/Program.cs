using System;
using System.IO;
using System.Threading;

namespace UtilTest
{
    using Util.Log;
    using Util.Message;
    using UtilTest.Lock;

    class Program
    {
        static Program()
        {
            LogUtil.SetLogPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log"));
        }

        static void Main(string[] args)
        {
            LockUtil_Test.Test();
            MonitorUtil_Test.Test();
            ReaderWriterLockUtil_Test.Test();

            Console.ReadLine();
        }

        static void TestMessageUtil()
        {
            //MessageUtil messageUtil = new MessageUtil("Message", 1, 10, 100);
            MessageSKUtil messageUtil = new MessageSKUtil("Message", 1, 10, 100, new SKConfig("10.1.0.3", 20003));

            Thread.Sleep(1000 * 10);
            messageUtil.SendMessage(new SendMessageObject("http://192.168.1.1/receive.aspx", "message", true));

            Thread.Sleep(1000 * 10);
            messageUtil.SendMessage(new SendMessageObject("http://192.168.1.1/receive.aspx", "message", true));
        }
    }
}
