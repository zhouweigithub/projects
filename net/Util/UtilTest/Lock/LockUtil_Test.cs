using System;

namespace UtilTest.Lock
{
    using Util.Lock;

    public class LockUtil_Test
    {
        /// <summary>
        /// 测试方法
        /// </summary>
        public static void Test()
        {
            LockUtil lockUtil = new LockUtil();
            String key = "key1";
            Object obj = lockUtil.GetLock(key);

            lockUtil.ReleaseLock(key);
            lockUtil.ReleaseAllLock();
        }
    }
}
