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
        public IHttpActionResult GetByStudent(int sudentid, short state)
        {
            List<Class> classList = ClassDAL.GetClassByStudentId(sudentid, state);
            return Json(classList);
        }

        [HttpGet]
        [Route("api/class/bycoach")]
        public IHttpActionResult GetByCoach(int coachid, short state)
        {
            List<Class> classList = ClassDAL.GetClassByCoachId(coachid, state);
            return Json(classList);
        }

        [HttpPost]
        [Route("api/class/CompleteClass")]
        public IHttpActionResult CompleteClass(int classId)
        {
            bool result = ClassBLL.CompleteClass(classId);
            return Json(result);
        }

        [HttpPost]
        [Route("api/class/CompleteClass")]
        public IHttpActionResult AddStudentComment(int classId, int studentId, float marking, string comment)
        {
            Dictionary<string, object> fieldValueDic = new Dictionary<string, object>();
            fieldValueDic.Add("marking", marking);
            fieldValueDic.Add("comment", comment);
            int count = DBData.GetInstance(DBTable.class_student).Update(fieldValueDic, $"class_id='{classId}' and student_id='{studentId}'");
            return Json(count > 0);
        }

        [HttpPost]
        [Route("api/class/UploadCoachComment")]
        public Task<HttpResponseMessage> UploadCoachComment()
        {
            List<string> savedFilePath = new List<string>();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var substringBin = AppDomain.CurrentDomain.BaseDirectory.IndexOf("bin");
            var path = AppDomain.CurrentDomain.BaseDirectory.Substring(0, substringBin);
            string rootPath = path + "upload";
            var provider = new MultipartFileStreamProvider(rootPath);
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsCanceled || t.IsFaulted)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }
                    foreach (MultipartFileData item in provider.FileData)
                    {
                        try
                        {
                            string name = item.Headers.ContentDisposition.FileName.Replace("\"", "");
                            string newFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(name);
                            File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));
                            //Request.RequestUri.PathAndQury为需要去掉域名的后面地址
                            //如上述请求为http://localhost:80824/api/upload/post，这就为api/upload/post
                            //Request.RequestUri.AbsoluteUri则为http://localhost:8084/api/upload/post

                            Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                            string fileRelativePath = rootPath + "\\" + newFileName;
                            Uri fileFullPath = new Uri(baseuri, fileRelativePath);
                            savedFilePath.Add(fileFullPath.ToString());
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.Created, JsonConvert.SerializeObject(savedFilePath));
                });
            return task;
        }
    }
}
