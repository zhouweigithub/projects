using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateImportExportMysqlDataScript
{
    public class BLL
    {
        private ServerInfo _exportServerInfo;
        private ServerInfo _importServerInfo;
        private static readonly string sqlFolder = AppDomain.CurrentDomain.BaseDirectory + "sql\\";
        private static readonly string configFileName = "config.txt";
        private static readonly string conditionFileName = "condition.txt";
        private static readonly string exportScript = "mysqldump.exe -h{0} -t -c --single-transaction --set-gtid-purged=OFF -u{1} -p{2} -P{3} {4} {5} {6} >{7}";
        private static readonly string importScript = "mysql.exe -h{0} -u{1} -p{2} -P{3} {4} <{5}";

        /// <summary>
        /// 生成导出与导入数据的.bat脚本文件
        /// </summary>
        public BLL()
        {
            _exportServerInfo = GetConfigInfo("[Export]");
            _importServerInfo = GetConfigInfo("[Import]");
        }

        /// <summary>
        /// 生成导出与导入数据的.bat脚本文件
        /// </summary>
        public void CreateScriptFiles()
        {
            List<condition> conditions = GetConditions();
            foreach (var item in conditions)
            {
                CreateExportMysqlDataScript(item.DataTable, item.Where);
                CreateInportMysqlDataScript(item.DataTable);
            }
        }

        /// <summary>
        /// 生成默认配置文件
        /// </summary>
        public void CreateConfigFile()
        {
            CreateServerConfigFile();
            CreateConditionFile();
        }



        /// <summary>
        /// 生成导出数据的脚本文件
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="where"></param>
        private void CreateExportMysqlDataScript(string dataTable, string where)
        {
            string scriptFolder = AppDomain.CurrentDomain.BaseDirectory + "export\\";
            string fileName = scriptFolder + "export_" + dataTable + ".bat";
            string sqlFileName = Path.Combine(sqlFolder, dataTable + ".sql");

            string whereScript = string.Empty;
            if (!string.IsNullOrWhiteSpace(where))
                whereScript = $"-w \"{where}\"";

            if (!Directory.Exists(scriptFolder))
                Directory.CreateDirectory(scriptFolder);
            if (!Directory.Exists(sqlFolder))
                Directory.CreateDirectory(sqlFolder);

            string content = $@"@echo off
@echo -----------------------------------------------------------------------------
@echo 导出数据表 {_exportServerInfo.Database}.{dataTable} 到 {sqlFileName}
@echo -----------------------------------------------------------------------------
";
            content += string.Format(exportScript, _exportServerInfo.IP, _exportServerInfo.UserName, _exportServerInfo.Password, _exportServerInfo.Port, _exportServerInfo.Database, dataTable, whereScript, sqlFileName);
            content += "\r\n@pause";
            File.WriteAllText(fileName, content, Encoding.GetEncoding("gb2312"));
        }

        /// <summary>
        /// 生成导入数据的脚本文件
        /// </summary>
        /// <param name="dataTable"></param>
        private void CreateInportMysqlDataScript(string dataTable)
        {
            string scriptFolder = AppDomain.CurrentDomain.BaseDirectory + "import\\";
            string fileName = scriptFolder + "import_" + dataTable + ".bat";
            string sqlFileName = Path.Combine(sqlFolder, dataTable + ".sql");

            if (!Directory.Exists(scriptFolder))
                Directory.CreateDirectory(scriptFolder);

            string content = $@"@echo off
@echo -----------------------------------------------------------------------------
@echo 导入数据到表 {_importServerInfo.Database}.{dataTable} 从 {sqlFileName}
@echo -----------------------------------------------------------------------------
";

            content += string.Format(importScript, _importServerInfo.IP, _importServerInfo.UserName, _importServerInfo.Password, _importServerInfo.Port, _importServerInfo.Database, Path.Combine(sqlFolder, dataTable + ".sql"));
            content += "\r\n@pause";

            File.WriteAllText(fileName, content, Encoding.GetEncoding("gb2312"));
        }

        /// <summary>
        /// 生成需要导出的表与过滤条件的配置文件
        /// </summary>
        /// <returns></returns>
        private List<condition> GetConditions()
        {
            if (File.Exists(conditionFileName))
            {
                List<condition> result = new List<condition>();
                string filePath = AppDomain.CurrentDomain.BaseDirectory + conditionFileName;
                string[] lines = File.ReadAllLines(filePath);
                foreach (var item in lines)
                {
                    if (item.Trim().Length == 0)
                        continue;

                    condition condition = new condition();
                    int whiteSpaceIndex = item.IndexOf(' ');
                    if (whiteSpaceIndex > -1)
                    {
                        condition.DataTable = item.Substring(0, whiteSpaceIndex).Trim();
                        condition.Where = item.Substring(whiteSpaceIndex + 1).Trim();
                    }
                    else
                    {
                        condition.DataTable = item.Trim();
                    }

                    result.Add(condition);
                }
                return result;
            }
            return new List<condition>();
        }

        /// <summary>
        /// 获取数据库服务器参数的相关全局配置
        /// </summary>
        /// <returns></returns>
        private ServerInfo GetConfigInfo(string startString)
        {
            if (File.Exists(configFileName))
            {
                ServerInfo result = new ServerInfo();
                string filePath = AppDomain.CurrentDomain.BaseDirectory + configFileName;
                var lines = File.ReadAllLines(filePath).ToList();

                int index = lines.IndexOf(startString);
                if (index < 0)
                    return null;

                for (int i = index + 1; i < index + 6; i++)
                {
                    var item = lines[i];
                    if (item.StartsWith("[IP]"))
                        result.IP = item.Substring(item.IndexOf("[IP]") + "[IP]".Length).Trim();
                    else if (item.StartsWith("[UserName]"))
                        result.UserName = item.Substring(item.IndexOf("[UserName]") + "[UserName]".Length).Trim();
                    else if (item.StartsWith("[Password]"))
                        result.Password = item.Substring(item.IndexOf("[Password]") + "[Password]".Length).Trim();
                    else if (item.StartsWith("[Port]"))
                        result.Port = item.Substring(item.IndexOf("[Port]") + "[Port]".Length).Trim();
                    else if (item.StartsWith("[Database]"))
                        result.Database = item.Substring(item.IndexOf("[Database]") + "[Database]".Length).Trim();
                }
                return result;
            }
            return null;
        }

        private void CreateServerConfigFile()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + configFileName;
            string content = @"[Export]
[IP]10.1.0.30
[UserName]root
[Password]moqikaka3311
[Port]3311
[Database]promotioncenter

[Import]
[IP]10.1.0.30
[UserName]root
[Password]moqikaka3311
[Port]3311
[Database]promotioncenter";
            File.WriteAllText(filePath, content);
        }

        private void CreateConditionFile()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + conditionFileName;
            string content = @"aso_ad id=1
appleid_appid";
            File.WriteAllText(filePath, content);
        }
    }

    public class ServerInfo
    {
        /// <summary>
        /// 数据库服务器IP
        /// </summary>
        public string IP;
        /// <summary>
        /// 数据库用户名
        /// </summary>
        public string UserName;
        /// <summary>
        /// 数据库密码
        /// </summary>
        public string Password;
        /// <summary>
        /// 数据库端口
        /// </summary>
        public string Port;
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Database;
        /// <summary>
        /// 导出的SQL文件存放的目录
        /// </summary>
        //public string SqlFileFolder;
    }

    public class condition
    {
        /// <summary>
        /// 数据表名
        /// </summary>
        public string DataTable;
        /// <summary>
        /// 过滤条件，可空
        /// </summary>
        public string Where;
    }
}
