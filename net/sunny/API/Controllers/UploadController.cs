using Newtonsoft.Json;
using Sunny.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

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

                            Util.Log.LogUtil.Write("item.LocalFileName：" + item.LocalFileName, Util.Log.LogType.Debug);
                            Util.Log.LogUtil.Write("newFileName：" + newFileName, Util.Log.LogType.Debug);

                            File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));

                            //Request.RequestUri.PathAndQury为需要去掉域名的后面地址
                            //如上述请求为http://localhost:80824/api/upload/post，这就为api/upload/post
                            //Request.RequestUri.AbsoluteUri则为http://localhost:8084/api/upload/post

                            Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                            string fileRelativePath = rootPath.Replace(Const.RootWebPath, string.Empty) + "\\" + newFileName;
                            Uri fileFullPath = new Uri(baseuri, fileRelativePath);
                            savedFilePath.Add(fileFullPath.ToString().Replace("\\", "//"));
                        }
                        catch (Exception ex)
                        {
                            Util.Log.LogUtil.Write("上传文件失败：" + ex, Util.Log.LogType.Error);
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.Created, savedFilePath);
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
                    result = "/Images/coach_images";
                    break;
                case UploadType.CoachComment:
                    result = "/Images/coach_comment";
                    break;
                default:
                    result = "/Images/undefined";
                    break;
            }

            return result;
        }

        /// <summary>
        /// 本地路径转换成URL相对路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string UrlConvertToR(string url)
        {
            string tmpRootDir = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath.ToString()); //获取程序根目录
            string imageurl2 = url.Replace(tmpRootDir, "");  //转换成相对路径
            imageurl2 = imageurl2.Replace(@"\", @"/");
            return imageurl2;
        }
    }
}
