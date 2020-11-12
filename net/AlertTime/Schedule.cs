using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlertTime
{
    [XmlRoot]
    public class ScheduleList
    {
        [XmlElement]
        public List<Schedule> Schedule { get; set; }
    }


    /// <summary>
    /// 日程
    /// </summary>
    public class Schedule
    {
        [XmlAttribute]
        public String Ttile { get; set; }

        [XmlAttribute]
        public String Content { get; set; }

        [XmlAttribute]
        public DateTime StartTime { get; set; }

        [XmlAttribute]
        public DateTime EndTime { get; set; }

        [XmlAttribute]
        public Boolean IsAlive { get; set; }
    }
}
