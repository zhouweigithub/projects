using Newtonsoft.Json;
using Sunny.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sunny.API.Controllers
{
    public class UploadController : ApiController
    {

        /// <summary>
        /// 教练上课后上传图片与视频
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/upload/UploadCoachClassComment")]
        public Task<HttpResponseMessage> UploadCoachClassComment()
        {
            return Upload(UploadType.CoachComment);
        }

        /// <summary>
        /// 教练上传证件图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/upload/UploadCoachImage")]
        public Task<HttpResponseMessage> UploadCoachImage()
        {
            return Upload(UploadType.CoachImg);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="uploadType">上传类型，用于保存到不同的目录中</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> Upload(UploadType uploadType)
        {
            List<string> savedFilePath = new List<string>();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string rootPath = Const.RootWebPath + GetUploadFolder(uploadType);
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
                            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff")
                                + Function.GetRangeCharaters(4, RangeType.Number)
                                + Path.GetExtension(name);

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

        /// <summary>
        /// 获取各上传类型的顶级目录
        /// </summary>
        /// <param name="uploadType"></param>
        /// <returns></returns>
        private string GetUploadFolder(UploadType uploadType)
        {
            string result = string.Empty;
            switch (uploadType)
            {
                case UploadType.CoachImg:
                    result = "coach_images";
                    break;
                case UploadType.CoachComment:
                    result = "coach_comment";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
