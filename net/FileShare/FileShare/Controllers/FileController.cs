using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using FileShare.BLL;
using FileShare.Models;
using Public.CSUtil.Log;

namespace FileShare.Controllers
{
    public class FileController : Controller
    {

        [HttpPost]
        public ActionResult Upload(String folder)
        {
            Int32 code = 0;
            //上传成功的文件数量
            Int32 successCount = 0;
            String msg = "ok";

            try
            {
                for (Int32 i = 0; i < Request.Files.Count; i++)
                {
                    UploadBLL.Upload(Request.Files[i], folder, Request.UserHostAddress);

                    successCount++;
                }
            }
            catch (Exception e)
            {
                code = -1;
                msg = e.Message;
                LogUtil.Error(e.ToString());
            }

            return Json(new { code, msg, successCount });
        }

        public ActionResult GetList(String folder)
        {
            List<FileDetail> datas = FileBLL.GetFolderAndFiles(folder, Request.UserHostAddress);
            return Json(new { data = datas }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult DownLoad(String filePath)
        {
            String fullPath = FileBLL.GetFullFileName(filePath);
            String suffix = Path.GetExtension(filePath);
            String fileName = Path.GetFileName(fullPath);
            String contentType = ContentTypeBLL.GetContentType(suffix);
            if (System.IO.File.Exists(fullPath))
            {
                UploadBLL.WriteLog(Request.UserHostAddress, "Download", filePath);
                return File(fullPath, contentType, fileName);
            }
            else
            {
                Response.StatusCode = 404;
                return new EmptyResult();
            }
        }

        public ActionResult Delete(String filePath, Boolean isFolder)
        {
            String msg = FileBLL.Delete(filePath, isFolder, Request.UserHostAddress);
            Boolean isOk = String.IsNullOrEmpty(msg);
            return Json(new { code = isOk ? 0 : -1, msg = isOk ? "删除成功" : msg }, JsonRequestBehavior.AllowGet);
        }
    }
}