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
using Sunny.Model.Request;
using Sunny.Model.Response;

namespace API.Controllers
{
    public class ClassController : ApiController
    {

        [HttpGet]
        [Route("api/class/get")]
        public IHttpActionResult GetById(int id)
        {
            ResponseResult result = null;
            try
            {
                Class list = DBData.GetInstance(DBTable.class_).GetEntityByKey<Class>(id);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/class/get 出错 id {id} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/class/bystudent")]
        public IHttpActionResult GetByStudent(string token)
        {
            ResponseResult result = null;
            try
            {
                int student_id = GeneralBLL.GetStudentByUserName(token).id;
                List<ClassStudentJson> list = ClassDAL.GetClassByStudentId(student_id);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/class/bystudent 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/class/bycoach")]
        public IHttpActionResult GetByCoach(string token)
        {
            ResponseResult result = null;
            try
            {
                int coach_id = GeneralBLL.GetCoachByUserName(token).id;
                List<ClassCoachJson> list = ClassDAL.GetClassByCoachId(coach_id);
                List<CoachClassStudentJson> students = ClassDAL.GetStudentInfoByCoachId(coach_id);
                foreach (ClassCoachJson item in list)
                {
                    item.students.AddRange(students.Where(a => a.class_id == item.id));
                }
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/class/GetByCoach 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/class/studentclasshistory")]
        public IHttpActionResult GetStudentClassHistoryList(string token)
        {
            ResponseResult result = null;
            try
            {
                int student_id = GeneralBLL.GetStudentByUserName(token).id;
                List<StudentClassHistoryJson> list = ClassDAL.GetStudentClassHistoryList(student_id);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/class/studentclasshistory 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpGet]
        [Route("api/class/coachclasshistory")]
        public IHttpActionResult GetCoachClassHistoryList(string token)
        {
            ResponseResult result = null;
            try
            {
                int coach_id = GeneralBLL.GetCoachByUserName(token).id;
                List<CoachClassHistoryJson> list = ClassDAL.GetCoachClassHistoryList(coach_id);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/class/coachclasshistory 出错 token {token} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpPost]
        [Route("api/class/CoachCompleteClass")]
        public IHttpActionResult CoachCompleteClass(CoachCompleteClassRequest model)
        {
            ResponseResult result = null;
            try
            {
                bool list = ClassBLL.CoachCompleteClass(model.classId);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/class/CoachCompleteClass 出错 classId {model.classId} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpPost]
        [Route("api/class/CoachAddClassComment")]
        public IHttpActionResult CoachAddClassComment(CoachAddClassCommentRequest request)
        {
            ResponseResult result = null;
            try
            {
                bool list = ClassBLL.CoachAddClassComment(request.classId, request.commentString, request.images, request.videos);
                result = new ResponseResult(0, "ok", list);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/class/CoachAddClassComment 出错 classId {request.classId} commentString {request.commentString} images.count {request.images.Count} videos.count {request.videos.Count} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

        [HttpPost]
        [Route("api/class/AddStudentComment")]
        public IHttpActionResult AddStudentComment(AddStudentCommentRequest request)
        {
            ResponseResult result = null;
            try
            {
                int student_id = DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{request.token}'").id;
                Dictionary<string, object> fieldValueDic = new Dictionary<string, object>();
                fieldValueDic.Add("marking", request.marking);
                fieldValueDic.Add("comment", request.comment);
                int count = DBData.GetInstance(DBTable.class_student).Update(fieldValueDic, $"class_id='{request.classId}' and student_id='{student_id}'");
                result = new ResponseResult(0, "ok", count > 0);
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"api/class/AddStudentComment 出错 classId {request.classId} token {request.token} marking {request.marking} comment {request.comment} \r\n {e}", Util.Log.LogType.Error);
                result = new ResponseResult(-1, "服务内部错误", null);
            }
            return Json(result);
        }

    }
}
