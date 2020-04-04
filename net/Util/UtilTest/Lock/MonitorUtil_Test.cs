using System;
using System.Threading;

namespace UtilTest.Lock
{
    using Util.Lock;

    public class MonitorUtil_Test
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        public static void Test()
        {
            MonitorUtil util = new MonitorUtil();
            int num = 0;
            for (int index = 0; index < 20; index++)
            {
                var th = new Thread(new ThreadStart(() =>
                {
                    Thread.Sleep(100);
                    for (int i = 0; i < 100; i++)
                    {
                        using (util.GetLock("hello"))
                        {
                            num++;
                            Console.WriteLine("num={0}", num);
                        }
                    }
                }));

                th.Start();
            }
        }
    }
}
