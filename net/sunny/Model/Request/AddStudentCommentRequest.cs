using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Request
{
    public class AddStudentCommentRequest
    {
        public int classId;
        public string token;
        public float marking;
        public string comment;
    }
}
