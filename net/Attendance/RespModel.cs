using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance
{
    public class RespModel
    {
        public int code { get; set; }
        public int count { get; set; }
        public string message { get; set; }
        public RespData data { get; set; }
    }

    public class RespData
    {
        public List<DateTime> logData { get; set; }
        public string name { get; set; }
    }

    public class ViewData
    {
        public DateTime date { get; set; }
        public string minTime { get; set; }
        public string maxTime { get; set; }
        public string overTimeState
        {
            get
            {
                if (maxTime.CompareTo("20:00") >= 0)
                    return "加班";
                else
                    return string.Empty;
            }
        }
        public double workTime
        {
            get
            {
                return Math.Round((DateTime.Parse("2000-01-01 " + maxTime) - DateTime.Parse("2000-01-01 " + minTime)).TotalMinutes / 60d, 2);
            }
        }
        public double overTime
        {
            get
            {
                return Math.Round((DateTime.Parse("2000-01-01 " + maxTime) - DateTime.Parse("2000-01-01 18:00")).TotalMinutes / 60d, 2);
            }
        }
        public int mealMoney
        {
            get
            {
                if (maxTime.CompareTo("20:00") >= 0)
                    return 15;
                else return 0;
            }
        }
        public int carMoney
        {
            get
            {
                if (maxTime.CompareTo("21:00") >= 0)
                    return 20;
                else return 0;
            }
        }
        public string isLate { get; set; }
        public string leaveEarly
        {
            get
            {
                if (maxTime.CompareTo("18:00") < 0)
                    return "早退";
                else return string.Empty;
            }
        }
        public int weekMoney
        {
            get
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    return 50;
                }
                else return 0;
            }
        }
    }
}
