using System;
using System.Threading;
using System.Threading.Tasks;

namespace UtilTest.Lock
{
    using Util.Lock;

    public class ReaderWriterLockUtil_Test
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        public static void Test()
        {
            ReaderWriterLockUtil util = new ReaderWriterLockUtil();
            Int32 num = 0;
            for (var i = 0; i < 10; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        using (util.GetLock("hello", ReaderWriterLockUtil.LockTypeEnum.Reader))
                        {
                            Console.WriteLine("num={0}", num);
                        }

                        Thread.Sleep(1000);
                    }
                });
            }

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    using (util.GetLock("hello", ReaderWriterLockUtil.LockTypeEnum.Writer))
                    {
                        num++;
                    }

                    Thread.Sleep(1000);
                }
            });
        }
    }
}
