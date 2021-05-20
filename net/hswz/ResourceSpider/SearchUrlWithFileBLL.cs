using System;
using System.Collections.Generic;
using System.IO;
using Hswz.Common;

namespace ResourceSpider
{
    public class SearchUrlWithFileBLL : SearchDomainBase
    {
        private const String dataFileName = @"data\datas.txt";

        protected override List<String> GetSavedUrls()
        {
            try
            {
                String filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dataFileName);
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
            try
            {
                String filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dataFileName);
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
