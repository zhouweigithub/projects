using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CreateDBmodels.Models;
using Util.Log;

namespace CreateDBmodels.BLL
{
    public class BLL
    {

        public static void CreateTableModelFiles()
        {
            List<ColumeModel> columnModels = DAL.TableInfoDAL.GetTableColumeInfo();
            List<TableModel> tableModels = DAL.TableInfoDAL.GetTableInfo();
            List<PrimaryKeyModel> primaryKeyModels = DAL.TableInfoDAL.GetPrimaryKeyInfo();
            CreateTableModelFiles(tableModels, columnModels);
            CreateTableDalFiles(tableModels, primaryKeyModels);
        }

        private static void CreateTableModelFiles(List<TableModel> tableModels, List<ColumeModel> columnModels)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("开始创建模型文件...");

            String modelTemplete = GetTemplete("model.txt");
            String columnTempleteString = GetTemplete("model_field.txt");

            if (String.IsNullOrWhiteSpace(modelTemplete) || String.IsNullOrWhiteSpace(columnTempleteString))
            {
                LogUtil.Write($"数据库表模型文件为空", LogType.Error);
                return;
            }

            List<String> tableNames = tableModels.Select(b => b.TABLE_NAME).ToList();
            foreach (String tableName in tableNames)
            {
                try
                {
                    StringBuilder sb = new StringBuilder(16);
                    TableModel tableInfo = tableModels.FirstOrDefault(a => a.TABLE_NAME == tableName);
                    List<ColumeModel> columns = columnModels.Where(a => a.TABLE_NAME == tableName).ToList();
                    foreach (ColumeModel column in columns)
                    {
                        sb.Append(columnTempleteString.Replace("#field_comment#", column.COLUMN_COMMENT).Replace("#field_type#", GetDataType(column.DATA_TYPE)).Replace("#field_name#", column.COLUMN_NAME));
                    }

                    //获取驼峰命名
                    String newTableName = GetCamelName(tableName);

                    String content = modelTemplete.Replace("#table_comment#", $"{tableInfo?.TABLE_COMMENT}({tableInfo?.TABLE_TYPE})").Replace("#table_name#", newTableName).Replace("#colume_items#", sb.ToString());

                    String folderPath = AppDomain.CurrentDomain.BaseDirectory + "Results\\Models";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    File.WriteAllText($"{folderPath}\\{newTableName}.cs", content);
                }
                catch (Exception e)
                {
                    LogUtil.Write($"生成数据库表模型文件出错：tableName: {tableName}" + e, LogType.Error);
                }
            }
        }

        private static void CreateTableDalFiles(List<TableModel> tableModels, List<PrimaryKeyModel> primaryKeyModels)
        {
            Console.WriteLine();
            Console.WriteLine("开始创建DAL文件...");

            String dalTemplete = GetTemplete("dal.txt");

            if (String.IsNullOrWhiteSpace(dalTemplete))
            {
                LogUtil.Write($"数据库表模型文件为空", LogType.Error);
                return;
            }

            List<String> tableNames = tableModels.Select(b => b.TABLE_NAME).ToList();
            foreach (String tableName in tableNames)
            {
                try
                {
                    TableModel tableInfo = tableModels.FirstOrDefault(a => a.TABLE_NAME == tableName);
                    List<PrimaryKeyModel> primaryKeys = primaryKeyModels.Where(a => a.TABLE_NAME == tableName).ToList();

                    var firstPrimaryKeyModel = primaryKeys.FirstOrDefault();

                    String primaryKey = firstPrimaryKeyModel != null ? firstPrimaryKeyModel.COLUMN_NAME : "id";

                    //获取驼峰命名
                    String dalName = GetCamelName(tableName) + "DAL";

                    String content = dalTemplete.Replace("#table_comment#", $"{tableInfo?.TABLE_COMMENT}").Replace("#table_name#", tableName).Replace("#dal_name#", dalName).Replace("#primary_key#", primaryKey);

                    String folderPath = AppDomain.CurrentDomain.BaseDirectory + "Results\\Dals";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    File.WriteAllText($"{folderPath}\\{dalName}.cs", content);
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
        private static String GetTemplete(String fileName)
        {
            try
            {
                String filePath = AppDomain.CurrentDomain.BaseDirectory + "\\Templete\\" + fileName;

                if (!File.Exists(filePath))
                {
                    return String.Empty;
                }

                return File.ReadAllText(filePath, Encoding.GetEncoding("gb2312"));
            }
            catch (Exception e)
            {
                LogUtil.Write("读取模板文件出错：" + e, LogType.Error);
            }

            return String.Empty;
        }

        /// <summary>
        /// 根据mysql的类型生成.net的类型
        /// </summary>
        /// <param name="mysqlType">mysql的类型</param>
        /// <returns></returns>
        private static String GetDataType(String mysqlType)
        {
            String result;

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

        /// <summary>
        /// 获取驼峰命名
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        private static String GetCamelName(String name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            String[] nameArray = name.Split(new Char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

            String result = String.Empty;

            foreach (var item in nameArray)
            {
                result += item.Substring(0, 1).ToUpper() + item.Substring(1);
            }

            return result;
        }
    }
}
