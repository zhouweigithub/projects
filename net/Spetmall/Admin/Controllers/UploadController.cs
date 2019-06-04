using Spetmall.BLL.Page.Upload;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Spetmall.Admin.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Upload/

        [HttpPost]
        public ActionResult Upload(FormCollection collection)
        {
            bool result = false;
            string strMsg = string.Empty;
            string saveFilePath = string.Empty;
            if (Request.Files.Count > 0)
            {
                try
                {
                    Image img = new Bitmap(Request.Files[0].InputStream);
                    img = UploadBLL.Resize(img, 250, 250, true);
                    string fileName = string.Format("{0}.jpeg", DateTime.Now.ToString("yyyyMMddhhmmssfff"));
                    saveFilePath = "/images/upload/" + fileName;
                    string saveFileAbsolutePath = HttpContext.Server.MapPath(saveFilePath);
                    UploadBLL.SaveMap(img, saveFileAbsolutePath, false);
                    result = true;
                }
                catch (Exception e)
                {
                    result = false;
                    strMsg = "ERROR : " + e.Message;
                }
            }

            return Json(new
            {
                code = result ? 0 : 1,
                imgurl = saveFilePath,
                msg = strMsg,
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
