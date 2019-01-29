using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Response
{
    public class ClassCoachJson : Class
    {
        public string course_name { get; set; }
        public string main_img { get; set; }
        public string summary { get; set; }
        public int student_count { get; set; }
        public string venue_name { get; set; }
        public string campus_name { get; set; }
    }
}
