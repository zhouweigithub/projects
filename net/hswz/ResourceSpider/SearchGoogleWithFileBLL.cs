using System;
using System.Collections.Generic;
using System.IO;
using Hswz.Common;

namespace ResourceSpider
{
    public class SearchGoogleWithFileBLL : SearchGoogleBase
    {
        private const String dataUrlFileName = @"data\urls.txt";
        private const String dataDomainFileName = @"data\domains.txt";

        protected override List<String> GetSavedUrls()
        {
            try
            {
                String filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dataUrlFileName);
                if (!FileHelper.IsExistFile(filePath))
                {
                    return new List<String>();
                }
                else
                {
                    List<String> datas = new List<String>();
                    String[] lines = File.ReadAllLines(filePath);
                    foreach (String item in lines)
                    {
                        String s = item.Trim();
                        if (!String.IsNullOrWhiteSpace(s))
                        {
                            datas.Add(s);
                        }
                    }
                    return datas;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("读取已有数据出现异常");
            }

            return new List<String>();
        }

        protected override Boolean SaveData(String url)
        {
            //保存URL
            SaveData(url, dataUrlFileName);

            //保存域名
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                SaveData(uri.Host, dataDomainFileName);
            }

            return true;
        }

        private Boolean SaveData(String url, String fileName)
        {
            try
            {
                String filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                String folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                FileHelper.CreateFile(filePath);
                FileHelper.AppendText(filePath, url + "\r\n");
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("保存数据出现异常");
                return false;
            }
        }
    }
}
