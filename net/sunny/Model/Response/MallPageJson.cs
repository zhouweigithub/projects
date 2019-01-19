using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Response
{
    public class MallPageJson
    {
        public List<BannerJson> top_images { get; set; }
        public List<CourseListJson> courses_jp { get; set; }
        public List<CourseListJson> courses_rm { get; set; }
    }
}
