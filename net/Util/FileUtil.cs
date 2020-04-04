// ****************************************
// FileName:FileUtil.cs
// Description:文件助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2014-06-13
// Revision History:
// ****************************************

using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Util
{
    using Ionic.Zip;

    /// <summary>
    /// 文件助手类
    /// </summary>
    public static class FileUtil
    {
        /// <summary>
        /// 获取指定目录下文件行数
        /// </summary>
        /// <param name="path">目录</param>
        /// <param name="suffix">文件后缀，用于过滤文件</param>
        /// <returns>文件行数</returns>
        public static Int32 GetFileLineCount(String path, String suffix)
        {
            Int32 count = 0;

            List<String> fileList = GetFileList(path, suffix);
            foreach (String fileName in fileList)
            {
                count += File.ReadAllLines(fileName).Count();
            }

            return count;
        }

        /// <summary>
        /// 获取指定目录下的所有文件、包括子文件夹里面的文件
        /// </summary>
        /// <param name="path">目录</param>
        /// <param name="suffix">文件后缀，用于过滤文件</param>
        /// <returns>文件的全路径列表</returns>
        public static List<String> GetFileList(String path, String suffix)
        {
            //获取路径对象
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            return GetFileList(dirInfo, suffix);
        }

        /// <summary>
        /// 获取指定目录对象下的所有文件、包括子文件夹里面的文件
        /// </summary>
        /// <param name="dirInfo">目录信息对象</param>
        /// <param name="suffix">文件后缀，用于过滤文件</param>
        /// <returns>文件的全路径列表</returns>
        public static List<String> GetFileList(DirectoryInfo dirInfo, String suffix)
        {
            List<String> fileNameList = new List<String>();

            //获取目录下的文件列表
            FileInfo[] fileInfos = dirInfo.GetFiles();
            foreach (FileInfo item in fileInfos)
            {
                if (!String.IsNullOrEmpty(suffix) && !item.Extension.Equals(suffix, StringComparison.Ordinal))
                {
                    continue;
                }

                fileNameList.Add(item.FullName);
            }

            //获取目录下的子目录
            DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
            foreach (DirectoryInfo item in dirInfos)
            {
                fileNameList.AddRange(GetFileList(item, suffix));
            }

            return fileNameList;
        }

        /// <summary>
        /// 获取指定url文件的内容
        /// </summary>
        /// <param name="url">文件的url地址</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.Net.WebException">System.Net.WebException</exception>
        /// <exception cref="System.NotSupportedException">System.NotSupportedException</exception>
        /// <returns>文件的内容</returns>
        public static Byte[] GetFileContent(String url)
        {
            if (String.IsNullOrWhiteSpace(url)) throw new ArgumentNullException("url", "url can't be empty.");

            try
            {
                return new WebClient().DownloadData(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 向指定文件里面写入信息
        /// </summary>
        /// <param name="filePath">文件夹名称</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="append">若要追加数据到该文件中，则为 true；若要覆盖该文件，则为 false。 如果指定的文件不存在，该参数无效，且构造函数将创建一个新文件。</param>
        /// <param name="msgs">信息列表</param>
        public static void WriteFile(String filePath, String fileName, Boolean append, params String[] msgs)
        {
            if (String.IsNullOrEmpty(filePath)) throw new ArgumentNullException("FilePath", "FilePath not exists.Please set the file path.");

            try
            {
                //如果目录不存在则创建
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                //获取文件对象
                FileInfo file = new FileInfo(Path.Combine(filePath, fileName));

                //文件不存在就创建，true表示追加
                using (StreamWriter writer = new StreamWriter(file.FullName, append))
                {
                    foreach (String msg in msgs)
                    {
                        writer.WriteLine(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取文件名称列表
        /// </summary>
        /// <param name="filePath">文件夹名称</param>
        /// <returns>文件名称列表</returns>
        public static String[] GetFileNameList(String filePath)
        {
            if (String.IsNullOrEmpty(filePath)) throw new ArgumentNullException("FilePath", "FilePath not exists.Please set the file path.");

            try
            {
                //如果目录不存在则直接返回
                if (!Directory.Exists(filePath))
                {
                    return null;
                }

                return Directory.GetFiles(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读取文件中的内容
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>文件中的内容列表</returns>
        public static String ReadFile(String fileName)
        {
            if (String.IsNullOrEmpty(fileName)) throw new ArgumentNullException("FilePath", "File not exists.Please set the file path.");

            try
            {
                //获取文件对象
                FileInfo file = new FileInfo(fileName);
                if (!file.Exists)
                {
                    return String.Empty;
                }

                //按行读取文件里面的内容
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        public static void DeleteFile(String fileName)
        {
            if (String.IsNullOrEmpty(fileName)) throw new ArgumentNullException("FilePath", "File not exists.Please set the file path.");

            //删除文件
            try
            {
                //获取文件对象
                FileInfo file = new FileInfo(fileName);
                if (!file.Exists)
                {
                    return;
                }

                //删除文件
                File.Delete(file.FullName);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 压缩文件或者文件夹
        /// </summary>
        /// <param name="files">要压缩的文件列表</param>
        /// <param name="folders">要压缩的文件夹列表</param>
        /// <param name="savePath">保存的路径</param>
        public static void CreateZip(IEnumerable<String> files, IEnumerable<String> folders, String savePath)
        {
            if ((files == null || files.Count() == 0) && (folders == null || folders.Count() == 0))
            {
                throw new ArgumentNullException("files，folders不能全为空。");
            }

            String parentFolder = Path.GetDirectoryName(savePath);
            if (Directory.Exists(parentFolder) == false)
            {
                Directory.CreateDirectory(parentFolder);
            }

            using (ZipFile zip = new ZipFile(Encoding.UTF8))
            {
                if (files != null)
                {
                    zip.AddFiles(files, "");
                }

                if (folders != null)
                {
                    foreach (var folder in folders)
                    {
                        zip.AddDirectory(folder, Path.GetFileNameWithoutExtension(folder));
                    }
                }

                //保存
                zip.Save(savePath);
            }
        }

        /// <summary>
        /// 解压zip文件
        /// </summary>
        /// <param name="zipPath">zip文件路径</param>
        /// <param name="targetPath">解压后的目标目录</param>
        public static void ExtractZip(String zipPath, String targetPath)
        {
            if (!File.Exists(zipPath))
            {
                throw new FileNotFoundException(String.Format("找不到待解压缩文件：{0}", zipPath));
            }

            if (new FileInfo(zipPath).Extension != ".zip")
            {
                throw new FormatException(String.Format("待解压缩文件：{0}不是zip文件", zipPath));
            }

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            using (ZipFile zip = new ZipFile(zipPath, Encoding.UTF8))
            {
                zip.ExtractAll(targetPath);
            }
        }
    }
}