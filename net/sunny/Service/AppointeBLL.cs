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

        public AppointeBLL()
        {
            Start();
        }

        public void Start()
        {
            //启动一个线程，去执行具体任务
            Task.Factory.StartNew(OneMinuteMethod);
        }

        private void OneMinuteMethod()
        {
            while (true)
            {
                try
                {
                    string currentTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    IList<BookingCoachQueue> bookingCoaches = DBData.GetInstance(DBTable.booking_coach_queue).GetList<BookingCoachQueue>($"end_time='{currentTimeString}'");

                    List<Coach> coachs = AppointmentDAL.GetCoachInfoCouldRecieveAppointment();
                    foreach (Coach item in coachs)
                    {
                        //发送可预约短信
                    }

                    //删除限定接单的教练队列
                    DBData.GetInstance(DBTable.booking_coach_queue).Delete($"end_time='{currentTimeString}'");

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
