using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Response
{
    public class CourseSpecificationJson
    {
        public List<Campus> campus;
        public Dictionary<int, List<Venue>> campu_venues;
        public List<CourseType> types;
        public Dictionary<string, decimal> prices;
    }
}
