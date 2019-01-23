using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Util.Log;
using Sunny.DAL;
using Sunny.Model;

namespace Sunny.Service
{
    public class AppointeBLL
    {
        /// <summary>
        /// 处理预约请求
        /// </summary>
        public AppointeBLL()
        {
            Start();
        }

        private void Start()
        {
            //启动一个线程，去执行具体任务
            Task.Factory.StartNew(OneMinuteMethod);
        }

        private void OneMinuteMethod()
        {
            //在限定时间内上次教练未接单的，给其他可接单的教练发送可接单的提示信息，同时删去该限定数据
            while (true)
            {
                try
                {
                    string currentTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    List<Coach> coachs = AppointmentDAL.GetCoachInfoCouldRecieveAppointment(currentTimeString);
                    foreach (Coach item in coachs)
                    {
                        //item.phone
                        //发送可预约短信
                    }

                    //删除限定接单的教练队列
                    DBData.GetInstance(DBTable.booking_coach_queue).Delete($"end_time<='{currentTimeString}'");
                }
                catch (Exception ex)
                {
                    LogUtil.Write("AppointeBLL.OneMinuteMethod 出错：" + ex.ToString(), LogType.Error);
                }
                finally
                {
                    //休眠到整分数
                    Thread.Sleep((60 - DateTime.Now.Second) * 1000);
                }
            }
        }

    }
}
