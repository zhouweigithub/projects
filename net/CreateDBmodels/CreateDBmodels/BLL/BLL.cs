using CreateDBmodels.Models;
using Moqikaka.Util.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDBmodels.BLL
{
    public class BLL
    {
        private static readonly string columnTempleteString = @"
        /// <summary>
        /// {0}
        /// </summary>
        [TableField]
        public {1} {2} {{ get; set; }}";

        public static void CreateTableModelFiles()
        {
            List<ColumeModel> columnModels = DAL.TableInfoDAL.GetTableColumeInfo();
            List<TableModel> tableModels = DAL.TableInfoDAL.GetTableInfo();
            CreateTableModelFiles(tableModels, columnModels);
        }

        private static void CreateTableModelFiles(List<TableModel> tableModels, List<ColumeModel> columnModels)
        {
            string templete = GetTemplete();
            if (string.IsNullOrWhiteSpace(templete))
            {
                LogUtil.Write($"数据库表模型文件为空", LogType.Error);
                return;
            }

            List<string> tableNames = columnModels.GroupBy(a => new { a.TABLE_NAME }).Select(b => b.Key.TABLE_NAME).ToList();
            foreach (string tableName in tableNames)
            {
                try
                {
                    StringBuilder sb = new StringBuilder(16);
                    TableModel tableInfo = tableModels.FirstOrDefault(a => a.TABLE_NAME == tableName);
                    List<ColumeModel> columns = columnModels.Where(a => a.TABLE_NAME == tableName).ToList();
                    foreach (ColumeModel column in columns)
                    {
                        sb.AppendFormat(columnTempleteString, column.COLUMN_COMMENT, GetDataType(column.DATA_TYPE), column.COLUMN_NAME);
                    }
                    string content = templete.Replace("#tablecomment#", $"{tableInfo?.TABLE_COMMENT}({tableInfo?.TABLE_TYPE})").Replace("#tablename#", tableName).Replace("#columeitems#", sb.ToString());

                    string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Results";
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    File.WriteAllText($"{folderPath}\\{tableName}.cs", content);
                }
                catch (Exception e)
                {
                    LogUtil.Write($"生成数据库表模型文件出错：tableName: {tableName}" + e, LogType.Error);
                }
            }
        }



        /// <summary>
        /// 读取模板内容
        /// </summary>
        /// <returns></returns>
        private static string GetTemplete()
        {
            try
            {
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\Templete\\model.txt";

                if (!File.Exists(filePath))
                    return string.Empty;

                return File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                LogUtil.Write("读取模板文件出错：" + e, LogType.Error);
            }

            return string.Empty;
        }

        /// <summary>
        /// 根据mysql的类型生成.net的类型
        /// </summary>
        /// <param name="mysqlType">mysql的类型</param>
        /// <returns></returns>
        private static string GetDataType(string mysqlType)
        {
            string result = string.Empty;
            switch (mysqlType)
            {
                case "char":
                    result = "string";
                    break;
                case "varchar":
                    result = "string";
                    break;
                case "timestamp":
                    result = "DateTime";
                    break;
                case "datetime":
                    result = "DateTime";
                    break;
                case "date":
                    result = "DateTime";
                    break;
                case "time":
                    result = "string";
                    break;
                case "double":
                    result = "double";
                    break;
                case "float":
                    result = "float";
                    break;
                case "decimal":
                    result = "decimal";
                    break;
                case "int":
                    result = "int";
                    break;
                case "bigint":
                    result = "long";
                    break;
                case "smallint":
                    result = "int16";
                    break;
                case "tinyint":
                    result = "short";
                    break;
                case "text":
                    result = "string";
                    break;
                default:
                    LogUtil.Write("发现未定义的数据类型：" + mysqlType, LogType.Fatal);
                    result = "string";
                    break;
            }

            return result;
        }
    }
}
