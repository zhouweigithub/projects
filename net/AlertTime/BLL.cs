using System;
using System.Text;
using System.Windows.Forms;

namespace AlertTime
{
    public static class BLL
    {
        public static readonly Encoding utf8 = Encoding.UTF8;

        public static readonly String scheduleFilePath = Application.ExecutablePath + @"App_Data\ScheduleList.xml";

        public static ScheduleList GetScheduleList()
        {
            ScheduleList list = XmlHelper.GetXmlInfo<ScheduleList>(scheduleFilePath, utf8);
            return list;
        }

        public static void SetScheduleList(ScheduleList list)
        {
            XmlHelper.XmlSerializeToFile(list, scheduleFilePath, utf8);
        }
    }
}
