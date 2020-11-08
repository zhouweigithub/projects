using System;
using System.IO;
using System.Web;

namespace FileShare.BLL
{
    public class UploadBLL
    {

        public static void Upload(HttpPostedFileBase fileInfo, String folder, String ip)
        {
            String rootPath = HttpContext.Current.Server.MapPath("~");

            String absolutePath = Path.Combine(rootPath, Common.UploadFolder, folder, fileInfo.FileName);

            String folderName = Path.GetDirectoryName(absolutePath);

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
                LogCacheBLL.Add(ip, folder);
            }

            fileInfo.SaveAs(absolutePath);

            //删除当前目录的缓存数据
            FileCacheBLL.Remove(folder);

            //写操作日志
            String relativeFileName = Path.Combine(folder, fileInfo.FileName);

            //写文件操作日志
            LogCacheBLL.Add(ip, relativeFileName);
            WriteLog(ip, "Upload", relativeFileName);

            //写目录操作日志
            AddFolderCache(ip, folder, Path.GetDirectoryName(fileInfo.FileName));
        }

        /// <summary>
        /// 添加路径的上传日志
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="baseFolder">上传文件的当前目录</param>
        /// <param name="path">上传的新增目录</param>
        private static void AddFolderCache(String ip, String baseFolder, String path)
        {
            String[] pathArray = path.Split(new Char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            String tmpPath = baseFolder;
            foreach (var item in pathArray)
            {
                tmpPath += "/" + item;
                tmpPath = tmpPath.TrimStart('/');
                if (!LogCacheBLL.IsExists(ip, tmpPath))
                {
                    WriteLog(ip, "Upload", tmpPath);
                    LogCacheBLL.Add(ip, tmpPath);
                }
            }
        }

        /// <summary>
        /// 写文件操作日志
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="type"></param>
        /// <param name="fileName"></param>
        public static void WriteLog(String ip, String type, String fileName)
        {
            try
            {
                fileName = fileName.Replace("\\", "/").TrimStart('/');

                String logFolderTmp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Common.UploadLogFolder);

                //分操作方式的日志文件名
                String logFilePath = Path.Combine(logFolderTmp, $"{DateTime.Today:yyyy-MM-dd}.{type}.txt");
                //汇集所有操作的日志文件名
                String logFileAllPath = Path.Combine(logFolderTmp, $"{DateTime.Today:yyyy-MM-dd}.txt");

                String folder = Path.GetDirectoryName(logFilePath);

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                String contentAll = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|{ip}|{type}|{fileName}\r\n";
                String content2 = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}   {ip}   {type}   {fileName}\r\n";

                File.AppendAllText(logFilePath, content2);

                if (type != "Download")
                    File.AppendAllText(logFileAllPath, contentAll);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}