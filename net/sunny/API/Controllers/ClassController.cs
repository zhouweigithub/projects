using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Sunny.BLL.API;
using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Custom;

namespace API.Controllers
{
    public class ClassController : ApiController
    {

        [HttpGet]
        [Route("api/class/get")]
        public IHttpActionResult GetById(int id)
        {
            Class result = DBData.GetInstance(DBTable.class_).GetEntityByKey<Class>(id);
            return Json(result);
        }

        [HttpGet]
        [Route("api/class/bystudent")]
        public IHttpActionResult GetByStudent(string token, short state)
        {
            int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
            List<Class> classList = ClassDAL.GetClassByStudentId(student_id, state);
            return Json(classList);
        }

        [HttpGet]
        [Route("api/class/bycoach")]
        public IHttpActionResult GetByCoach(string token, short state)
        {
            int coach_id = DBData.GetInstance(DBTable.coach).GetEntity<Coach>($"username='{token}'").id;
            List<Class> classList = ClassDAL.GetClassByCoachId(coach_id, state);
            return Json(classList);
        }

        [HttpGet]
        [Route("api/class/studentclasshistory")]
        public IHttpActionResult GetStudentClassHistoryList(string token)
        {
            int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
            List<StudentClassHistoryJson> classList = ClassDAL.GetStudentClassHistoryList(student_id);
            return Json(classList);
        }

        [HttpGet]
        [Route("api/class/coachclasshistory")]
        public IHttpActionResult GetCoachClassHistoryList(string token)
        {
            int coach_id = DBData.GetInstance(DBTable.coach).GetEntity<Coach>($"username='{token}'").id;
            List<CoachClassHistoryJson> classList = ClassDAL.GetCoachClassHistoryList(coach_id);
            return Json(classList);
        }

        [HttpPost]
        [Route("api/class/CoachCompleteClass")]
        public IHttpActionResult CoachCompleteClass(int classId)
        {
            bool result = ClassBLL.CoachCompleteClass(classId);
            return Json(result);
        }

        [HttpPost]
        [Route("api/class/CoachAddClassComment")]
        public IHttpActionResult CoachAddClassComment(int classId, string commentString, List<string> images, List<string> videos)
        {
            bool result = ClassBLL.CoachAddClassComment(classId, commentString, images, videos);
            return Json(result);
        }

        [HttpPost]
        [Route("api/class/AddStudentComment")]
        public IHttpActionResult AddStudentComment(int classId, string token, float marking, string comment)
        {
            int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{token}'").id;
            Dictionary<string, object> fieldValueDic = new Dictionary<string, object>();
            fieldValueDic.Add("marking", marking);
            fieldValueDic.Add("comment", comment);
            int count = DBData.GetInstance(DBTable.class_student).Update(fieldValueDic, $"class_id='{classId}' and student_id='{student_id}'");
            return Json(count > 0);
        }

    }
}
