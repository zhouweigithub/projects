using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileShare.Models;
using Public.CSUtil.Log;

namespace FileShare.BLL
{
    public class FileBLL
    {
        private static readonly String rootPath = AppDomain.CurrentDomain.BaseDirectory;


        /// <summary>
        /// 获取目录内的文件与子目录
        /// </summary>
        /// <param name="folder">目录路径</param>
        /// <returns></returns>
        public static List<FileDetail> GetFolderAndFiles(String folder, String ip)
        {
            //如果缓存里存在则直接从缓存取数据
            if (FileCacheBLL.IsExists(folder))
            {
                List<FileDetail> datas = FileCacheBLL.GetData(folder);
                foreach (var item in datas)
                {   //刷新是否为创建者
                    item.IsCreater = IsManagerOrCreater(ip, folder + "/" + item.Name);
                }
                return datas;
            }

            List<FileDetail> result = new List<FileDetail>();
            String path = Path.Combine(rootPath, Common.UploadFolder, folder);
            if (!Directory.Exists(path))
                return result;

            LogUtil.Debug($"读取磁盘目录内容：{folder}");

            String[] files = Directory.GetFiles(path);
            foreach (String item in files)
            {
                FileInfo fi = new FileInfo(item);
                result.Add(new FileDetail()
                {
                    Name = fi.Name,
                    IsFolder = false,
                    Size = fi.Length,
                    LastModifyTime = fi.LastWriteTime,
                    IsCreater = IsManagerOrCreater(ip, folder + "/" + fi.Name),
                });
            }

            String[] folders = Directory.GetDirectories(path);
            foreach (String item in folders)
            {
                DirectoryInfo di = new DirectoryInfo(item);
                result.Add(new FileDetail()
                {
                    Name = di.Name,
                    IsFolder = true,
                    Size = 0,
                    LastModifyTime = di.LastWriteTime,
                    IsCreater = IsManagerOrCreater(ip, folder + "/" + di.Name),
                });
            }

            result = result.OrderBy(a => a.Name).ToList();

            //往缓存中添加数据
            FileCacheBLL.Add(folder, result);

            return result;
        }

        /// <summary>
        /// 是否为管理员或创建者
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="path">子目录名或文件相对路径名</param>
        /// <returns></returns>
        private static Boolean IsManagerOrCreater(String ip, String path)
        {
            //是否为管理员
            String managers = ManagerConfigBLL.GetConfig("managers");
            String[] managerArray = managers.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (managerArray.Contains(ip))
                return true;

            //是否为创建者
            return LogCacheBLL.IsExists(ip, path);
        }

        public static String GetFullFileName(String path)
        {
            String fullPath = Path.Combine(rootPath, Common.UploadFolder, path);
            if (!File.Exists(fullPath))
                return String.Empty;
            else
                return fullPath;
        }

        /// <summary>
        /// 删除文件或目录
        /// </summary>
        /// <param name="path">文件或目录地址</param>
        /// <param name="isFolder">是否为目录</param>
        public static String Delete(String path, Boolean isFolder, String ip)
        {
            try
            {
                if (!IsManagerOrCreater(ip, path))
                {
                    LogUtil.Error($"{ip} 试图删除 {path} 时出错：无相应权限");
                    return "access denied";
                }

                String fullPath = Path.Combine(rootPath, Common.UploadFolder, path);
                if (isFolder)
                {
                    //删除目录
                    Directory.Delete(fullPath, true);
                    String parent = GetParentFolder(path);
                    FileCacheBLL.Remove(parent);
                }
                else
                {
                    //删除文件
                    String folder = Path.GetDirectoryName(path);
                    File.Delete(fullPath);
                    FileCacheBLL.Remove(folder);
                }

                UploadBLL.WriteLog(ip, "Delete", path);
                LogCacheBLL.Remove(ip, path);

                return String.Empty;
            }
            catch (Exception e)
            {
                LogUtil.Error(e.ToString());
                return e.ToString();
            }
        }

        /// <summary>
        /// 获取目录的父目录
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static String GetParentFolder(String folder)
        {
            folder = folder.Replace("\\", "/");
            Int32 index = folder.LastIndexOf("/");
            if (index < 0)
                index = 0;
            return folder.Substring(0, index);
        }
    }
}