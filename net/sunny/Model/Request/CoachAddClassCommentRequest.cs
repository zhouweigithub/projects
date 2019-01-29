using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Request
{
    public class CoachAddClassCommentRequest
    {
        public int classId;
        public string commentString;
        public List<string> images;
        public List<string> videos;
    }
}
