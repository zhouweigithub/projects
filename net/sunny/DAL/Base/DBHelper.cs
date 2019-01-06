using Sunny.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Sunny.DAL
{
    /// <summary>
    /// 数据库操作类，支持多数据库
    /// 文件功能描述：公共类，数据库操作类，通过本类可以快速方便地进行常用数据库操作，而不必关心数据库特性
    /// 依赖说明：通过Config获取数据库配置信息，配置文件中需要配置：DataBaseType和ConnString两个配置项
    /// 异常处理：捕获但不会处理异常。
    /// </summary>
    public class DBHelper : IDisposable
    {
        private string _dbType;
        private string _connStr;
        private DBOperator _db;
        /// <summary>
        /// 配置文件中数据库类型定义的键名
        /// </summary>
        protected string ConfigKeyForDataBaseType = "DataBaseType";

        /// <summary>
        /// 配置文件中数据库连接字符串的键名
        /// </summary>
        protected string ConfigKeyForConnString = "ConnString";

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get { return _dbType; } }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnStr { get { return _connStr; } }

        /// <summary>
        /// 构造函数,数据库类型及连接字符串会读取默认配置项DataBaseType、ConnString
        /// </summary>
        public DBHelper()
        {
            _dbType = Config.GetConfigToString(ConfigKeyForDataBaseType);
            _connStr = Config.GetConfigToString(ConfigKeyForConnString);
            _db = GetDBOperator(_dbType, _connStr);
            _db.Open();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbType">数据库类型（MYSQL/ORACLE/SQLSERVER/POSTGRESQL）</param>
        /// <param name="connStr">连接字符串</param>
        public DBHelper(string dbType, string connStr)
        {
            _dbType = dbType;
            _connStr = connStr;
            _db = GetDBOperator(dbType, connStr);
            _db.Open();
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~DBHelper()
        {
            _db.Close();
        }

        /// <summary>
        /// 显示关闭连接
        /// </summary>
        public void Dispose()
        {
            _db.Close();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            _db.Close();
        }

        #region 轻工厂制造数据库实例

        /// <summary>
        /// 创建数据库工厂实例
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connStr">连接字符串</param>
        /// <returns>数据库操作实例</returns>
        private static DBOperator GetDBOperator(string dbType, string connStr)
        {
            switch (dbType.ToUpper())
            {
                case "MYSQL":
                    return new Providers.Mysql(connStr);
                //case "ORACLE":
                //    return new Providers.Oracle(connStr);
                //case "SQLSERVER":
                //    return new Providers.SqlServer(connStr);
                //case "POSTGRESQL":
                //    return new Providers.Postgresql(connStr);
                default:
                    throw new Exception("未知的数据库类型：" + dbType);
            }
        }
        #endregion

        #region 事务操作
        /// <summary>
        /// 开始事务操作
        /// </summary>
        public void BeginTrans()
        {
            _db.BeginTrans();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTrans()
        {
            _db.CommitTrans();
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void RollbackTrans()
        {
            _db.RollbackTrans();
        }

        #endregion


        #region ExecuteNonQuery

        /// <summary>
        /// 执行SQL语句，返回受影响记录条数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        public int ExecuteNonQuery(string sql)
        {
            try
            {
                return _db.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\nSQL语句为：{0}。\r\n{1}", sql, ex.ToString()));
            }
        }

        /// <summary>
        /// 执行SQL语句，返回受影响记录条数
        /// </summary>
        /// <param name="sql">SQL语句或命令（参数用问号?占位。为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <param name="value">参数值列表</param>
        /// <returns>受影响记录条数</returns>
        public int ExecuteNonQueryParams(string sql, params MySqlParameter[] commandParameters)
        {
            try
            {
                return _db.ExecuteNonQueryParams(sql, commandParameters);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\n数据库：{0}。\r\nSQL语句：{1}。\r\n参数列表{2}。\r\n{3}", DbType, sql, _db.ShowParamsList(commandParameters), ex.ToString()));
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行一条SQL语句，返回第一行第一列object
        /// </summary>
        /// <param name="sql">SQL语句（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string sql)
        {
            try
            {
                return _db.ExecuteScalar(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\nSQL语句为：{0}。\r\n{1}", sql, ex.ToString()));
            }
        }

        /// <summary>
        /// 执行一条SQL语句，返回第一行第一列int
        /// </summary>
        /// <param name="sql">SQL语句（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <returns>int</returns>
        public int ExecuteScalarInt(string sql)
        {
            object obj = ExecuteScalar(sql);
            if (obj is DBNull)
            {
                return 0;
            }
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 执行一条SQL语句，返回第一行第一列string
        /// </summary>
        /// <param name="sql">SQL语句（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <returns>string</returns>
        public string ExecuteScalarString(string sql)
        {
            object obj = ExecuteScalar(sql);
            if (obj is DBNull)
            {
                return string.Empty;
            }
            try
            {
                return Convert.ToString(obj);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 执行一条SQL语句，返回一个object
        /// </summary>
        /// <param name="sql">SQL语句或命令（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <param name="value">参数值列表</param>
        /// <returns>object</returns>
        public object ExecuteScalarParams(string sql, params MySqlParameter[] commandParameters)
        {
            try
            {
                return _db.ExecuteScalarParams(sql, commandParameters);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\n数据库：{0}。\r\nSQL语句：{1}。\r\n参数列表{2}。\r\n{3}", DbType, sql, _db.ShowParamsList(commandParameters), ex.ToString()));
            }
        }

        /// <summary>
        /// 执行一条SQL语句，返回一个int
        /// </summary>
        /// <param name="sql">SQL语句或命令（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <param name="value">参数值列表</param>
        /// <returns>int</returns>
        public int ExecuteScalarIntParams(string sql, params MySqlParameter[] commandParameters)
        {
            object obj = ExecuteScalarParams(sql, commandParameters);
            if (obj is DBNull)
            {
                return 0;
            }
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 执行一条SQL语句，返回一个string
        /// </summary>
        /// <param name="sql">SQL语句或命令（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <param name="value">参数值列表</param>
        /// <returns>string</returns>
        public string ExecuteScalarStringParams(string sql, params MySqlParameter[] commandParameters)
        {
            object obj = ExecuteScalarParams(sql, commandParameters);
            if (obj is DBNull)
            {
                return string.Empty;
            }
            try
            {
                return Convert.ToString(obj);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region ExecuteDataRow

        /// <summary>
        /// 执行SQL语句，返回DataRow
        /// </summary>
        /// <param name="sql">SQL语句（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <returns>DataRow</returns>
        public DataRow ExecuteDataRow(string sql)
        {
            try
            {
                DataTable dt = ExecuteDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\nSQL语句为：{0}。\r\n{1}", sql, ex.ToString()));
            }
        }

        /// <summary>
        /// 执行SQL语句，返回DataRow
        /// </summary>
        /// <param name="sql">SQL语句或命令（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <param name="value">参数值列表</param>
        /// <returns>DataRow</returns>
        public DataRow ExecuteDataRowParams(string sql, params MySqlParameter[] commandParameters)
        {
            try
            {
                DataTable dt = ExecuteDataTableParams(sql, commandParameters);
                if (dt != null && dt.Rows.Count != 0)
                {
                    return dt.Rows[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\n数据库：{0}。\r\nSQL语句：{1}。\r\n参数列表{2}。\r\n{3}", DbType, sql, commandParameters, ex.ToString()));
            }
        }

        #endregion

        #region ExecuteDataTable

        /// <summary>
        /// 执行SQL语句，返回DataTable
        /// </summary>
        /// <param name="sql">SQL语句（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql)
        {
            try
            {
                return _db.ExecuteDataTable(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\nSQL语句为：{0}。\r\n{1}", sql, ex.ToString()));
            }
        }

        /// <summary>
        /// 执行SQL语句，返回DataTable
        /// </summary>
        /// <param name="sql">SQL语句或命令（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <param name="value">参数值列表</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTableParams(string sql, params MySqlParameter[] commandParameters)
        {
            try
            {
                return _db.ExecuteDataTableParams(sql, commandParameters);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\n数据库：{0}。\r\nSQL语句：{1}。\r\n参数列表{2}。\r\n{3}", DbType, sql, _db.ShowParamsList(commandParameters), ex.ToString()));
            }
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 执行SQL语句，返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <returns>DataTable</returns>
        public DataSet ExecuteDataSet(string sql)
        {
            try
            {
                return _db.ExecuteDataSet(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\nSQL语句为：{0}。\r\n{1}", sql, ex.ToString()));
            }
        }

        /// <summary>
        /// 执行SQL语句，返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句或命令（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <param name="value">参数值列表</param>
        /// <returns>DataTable</returns>
        public DataSet ExecuteDataSetParams(string sql, params MySqlParameter[] commandParameters)
        {
            try
            {
                return _db.ExecuteDataSetParams(sql, commandParameters);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\n数据库：{0}。\r\nSQL语句：{1}。\r\n参数列表{2}。\r\n{3}", DbType, sql, _db.ShowParamsList(commandParameters), ex.ToString()));
            }
        }

        #endregion

        #region 分页查询
        /// <summary>
        /// 分页查询，返回DataSet
        /// </summary>
        /// <param name="sql">除去分页之外的SQL语句</param>
        /// <param name="pageSize">页面大小（单页记录条数）</param>
        /// <param name="pageIndex">当前页码（页号从1开始）</param>
        /// <returns></returns>
        public DataSet ExecuteDataSetPage(string sql, int pageSize, int pageIndex)
        {
            try
            {
                return _db.ExecuteDataSetPage(sql, pageSize, pageIndex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\n数据库：{0}。\r\nSQL语句：{1}。\r\n{2}", DbType, sql, ex.ToString()));
            }
        }

        /// <summary>
        /// 分页查询，返回DataSet
        /// </summary>
        /// <param name="sql">除去分页之外的SQL语句</param>
        /// <param name="pageSize">页面大小（单页记录条数）</param>
        /// <param name="pageIndex">当前页码（页号从1开始）</param>
        /// <param name="value">参数列表</param>
        /// <returns></returns>
        public DataSet ExecuteDataSetPageParams(string sql, int pageSize, int pageIndex, params MySqlParameter[] commandParameters)
        {
            try
            {
                return _db.ExecuteDataSetPageParams(sql, pageSize, pageIndex, commandParameters);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\n数据库：{0}。\r\nSQL语句：{1}。\r\n参数列表{2}。\r\n{3}", DbType, sql, _db.ShowParamsList(commandParameters), ex.ToString()));
            }
        }

        /// <summary>
        /// 分页查询，返回DataTable
        /// </summary>
        /// <param name="sql">除去分页之外的SQL语句</param>
        /// <param name="pageSize">页面大小（单页记录条数）</param>
        /// <param name="pageIndex">当前页码（页号从1开始）</param>
        /// <returns></returns>
        public DataTable ExecuteDataTablePage(string sql, int pageSize, int pageIndex)
        {
            try
            {
                return _db.ExecuteDataTablePage(sql, pageSize, pageIndex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\n数据库：{0}。\r\nSQL语句：{1}。\r\n{2}", DbType, sql, ex.ToString()));
            }
        }

        /// <summary>
        /// 分页查询，返回DataTable
        /// </summary>
        /// <param name="sql">除去分页之外的SQL语句</param>
        /// <param name="pageSize">页面大小（单页记录条数）</param>
        /// <param name="pageIndex">当前页码（页号从1开始）</param>
        /// <param name="value">参数列表</param>
        /// <returns></returns>
        public DataTable ExecuteDataTablePageParams(string sql, int pageSize, int pageIndex, params MySqlParameter[] commandParameters)
        {
            try
            {
                return _db.ExecuteDataTablePageParams(sql, pageSize, pageIndex, commandParameters);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("查询失败！\r\n数据库：{0}。\r\nSQL语句：{1}。\r\n参数列表{2}。\r\n{3}", DbType, sql, _db.ShowParamsList(commandParameters), ex.ToString()));
            }
        }
        #endregion

        #region ExecuteDataTablePage


        /// <summary>
        /// 分页查询，返回DataTable，该方法已经过时（为了照顾SQLServer2005以下版本）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fields">要返回的字段列表，逗号分隔</param>
        /// <param name="keyField">主键字段名，如果是Oracle或Mysql可以为空（""）</param>
        /// <param name="orderBy">排序字段，可以为空，多个字段需用逗号分隔</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页（页号从1开始）</param>
        /// <returns>DataTable</returns>
        //public DataTable ExecuteDataTablePage(string tableName, string fields, string keyField, string orderBy, int pageSize, int pageIndex)
        //{
        //    string gropBy = string.Empty;
        //    string condition = string.Empty;
        //    return _db.ExecuteDataTablePageParams(tableName, fields, keyField, gropBy, orderBy, pageSize, pageIndex, condition);
        //}

        #endregion

        #region ExecuteDataTablePageParams

        /// <summary>
        /// 分页查询，返回DataTable
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fields">要返回的字段列表，逗号分隔</param>
        /// <param name="keyField">主键字段名，如果是Oracle或Mysql可以为空（""）</param>
        /// <param name="groupBy">GROUP BY子句,不包含GROUP BY关键字</param>
        /// <param name="orderBy">排序字段，可以为空，多个字段需用逗号分隔</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页（页号从1开始）</param>
        /// <param name="condition">筛选条件，可以为空，不带"WHERE"关键字</param>
        /// <param name="value">参数列表</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTablePageParams(string tableName, string fields, string keyField, string groupBy, string orderBy, int pageSize, int pageIndex, string condition, params MySqlParameter[] commandParameters)
        {
            return _db.ExecuteDataTablePageParams(tableName, fields, keyField, groupBy, orderBy, pageSize, pageIndex, condition, commandParameters);
        }

        #endregion

        #region 导入txt到数据库
        /// <summary>
        /// 从本地文件导入数据到数据库表中，默认：文件为UTF-8编码，导入所有字段，字段分隔符为\t，记录分隔符为\n
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <returns>导入记录条数</returns>
        public int LoadDataInLocalFile(string tableName, string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                throw new Exception("文件" + fileName + "不存在，无法导入表" + tableName);
            }
            return _db.LoadDataInLocalFile(tableName, fileName);
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中，默认：文件为UTF-8编码，字段分隔符为\t，记录分隔符为\n
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fieldsTerminated">字段列表</param>
        /// <returns>导入记录条数</returns>
        public int LoadDataInLocalFile(string tableName, string fileName, List<string> fields)
        {
            if (!System.IO.File.Exists(fileName))
            {
                throw new Exception("文件" + fileName + "不存在，无法导入表" + tableName);
            }
            return _db.LoadDataInLocalFile(tableName, fileName, fields);
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中，默认：文件为UTF-8编码，导入所有字段，记录分隔符为\n
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段分隔符</param>
        /// <returns>导入记录条数</returns>
        public int LoadDataInLocalFile(string tableName, string fileName, string fieldsTerminated)
        {
            if (!System.IO.File.Exists(fileName))
            {
                throw new Exception("文件" + fileName + "不存在，无法导入表" + tableName);
            }
            return _db.LoadDataInLocalFile(tableName, fileName, fieldsTerminated);
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中，默认：文件为UTF-8编码，记录分隔符为\n
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="fieldsTerminated">字段分隔符</param>
        /// <returns>导入记录条数</returns>
        public int LoadDataInLocalFile(string tableName, string fileName, List<string> fields, string fieldsTerminated)
        {
            if (!System.IO.File.Exists(fileName))
            {
                throw new Exception("文件" + fileName + "不存在，无法导入表" + tableName);
            }
            return _db.LoadDataInLocalFile(tableName, fileName, fields, fieldsTerminated);
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中，默认：文件为UTF-8编码
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="fieldsTerminated">字段分隔符</param>
        /// <param name="linesTerminated">记录分隔符</param>
        /// <returns>导入记录条数</returns>
        public int LoadDataInLocalFile(string tableName, string fileName, List<string> fields, string fieldsTerminated, string linesTerminated)
        {
            if (!System.IO.File.Exists(fileName))
            {
                throw new Exception("文件" + fileName + "不存在，无法导入表" + tableName);
            }
            return _db.LoadDataInLocalFile(tableName, fileName, fields, fieldsTerminated, linesTerminated);
        }

        #endregion

        #region 从内存导入数据到数据库

        /// <summary>
        /// 从DataTable导入数据到数据库表（适用于小批量数据导入）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="dt">数据表（字段名通过ColumnName来指定）</param>
        /// <returns></returns>
        public int LoadDataInDataTable(string tableName, DataTable dt)
        {
            if (dt == null || dt.Rows.Count < 1)
            {
                return 0;
            }

            List<string> filed = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                filed.Add(col.ColumnName);
            }
            int rowIndex = 0;
            int colcount = dt.Columns.Count;
            MySqlParameter[] parList = new MySqlParameter[dt.Rows.Count];

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO " + tableName + "({0}) VALUES ", string.Join(",", filed));


            foreach (DataRow dr in dt.Rows)
            {
                if (rowIndex != 0)
                {
                    sb.Append(",");
                }
                sb.Append("(");

                for (int columnIndex = 0; columnIndex < colcount; columnIndex++)
                {
                    string tmpPara = string.Format("@{0}{1}", filed[columnIndex], columnIndex);
                    parList[rowIndex] = new MySqlParameter(tmpPara, dr[columnIndex]);
                    if (columnIndex != 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(tmpPara);
                }
                sb.AppendLine(")");

                rowIndex++;
            }

            //执行SQL语句
            return ExecuteNonQueryParams(sb.ToString(), parList);
        }

        /// <summary>
        /// 从List导入数据到数据库表（适用于小批量数据导入）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="list">数据列表（每条记录为一个字典，字典的键为字段名，值为字段值</param>
        /// <returns>导入数据的条数</returns>
        public int LoadDataInList(string tableName, List<Dictionary<string, object>> list)
        {
            if (list == null || list.Count < 1 || list[0] == null)
            {
                return 0;
            }
            int rowIndex = 0;
            int colcount = list[0].Count;

            MySqlParameter[] parList = new MySqlParameter[list.Count];

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO " + tableName + "({0}) VALUES ", string.Join(",", list[0].Keys));


            foreach (Dictionary<string, object> dic in list)
            {
                if (rowIndex != 0)
                {
                    sb.Append(",");
                }
                sb.Append("(");

                int columnIndex = 0;
                foreach (var kvp in dic)
                {
                    string tmpPara = string.Format("@{0}{1}", kvp.Key, columnIndex);
                    parList[rowIndex] = new MySqlParameter(tmpPara, kvp.Value);

                    if (columnIndex != 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(tmpPara);
                    columnIndex++;
                }
                sb.AppendLine(")");

                rowIndex++;
            }


            //执行SQL语句
            return ExecuteNonQueryParams(sb.ToString(), parList);
        }

        #endregion

        #region 获取表信息
        /// <summary>
        /// 根据表名，获取字段列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表字段列表</returns>
        public List<string> GetFieldsList(string tableName)
        {
            try
            {
                return _db.GetFieldsList(tableName);
            }
            catch (Exception ex)
            {
                throw new Exception("获取表" + tableName + "的字段列表出错!" + ex.Message);
            }
        }

        /// <summary>
        /// 判断表某字段值是否重复
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">字段值</param>
        /// <param name="keyField">主键字段名</param>
        /// <param name="keyValue">主键值</param>
        /// <returns>bool</returns>
        public bool IsDuplicate(string tableName, string fieldName, string value, string keyField, string keyValue)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} != @{1} AND {2} = @{2}", tableName, keyField, fieldName);
            MySqlParameter[] parList = new MySqlParameter[] {
                new MySqlParameter("@" + keyField, keyValue),
                new MySqlParameter("@" + fieldName, value)
            };
            byte ret = Convert.ToByte(ExecuteScalarParams(sql, parList));
            return ret > 0 ? true : false;
        }

        /// <summary>
        /// 判断表某字段是否重复
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="oldValue">如果是修改，则为旧值；如果是添加，则为string.Empty</param>
        /// <param name="newValue">新值</param>
        /// <returns>bool</returns>
        public bool IsDuplicate(string tableName, string fieldName, string oldValue, string newValue)
        {
            if (oldValue == newValue)
            {
                return false;
            }
            string sql = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = @{1}", tableName, fieldName);
            MySqlParameter[] parList = new MySqlParameter[] { new MySqlParameter("@" + fieldName, newValue) };
            byte ret = Convert.ToByte(ExecuteScalarParams(sql, parList));
            return ret > 0 ? true : false;
        }

        /// <summary>
        /// 判断表某字段是否重复
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="oldValue">如果是修改，则为旧值；如果是添加，则为string.Empty</param>
        /// <param name="newValue">新值</param>
        /// <param name="condition">附加判断条件（不带WHERE，不带AND）</param>
        /// <returns>bool</returns>
        public bool IsDuplicate(string tableName, string fieldName, string oldValue, string newValue, string[] conditions)
        {
            if (oldValue == newValue)
            {
                return false;
            }
            string conondition = string.Empty;
            foreach (string c in conditions)
            {
                conondition += " AND " + c;
            }

            string sql = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = @{1} {2}", tableName, fieldName, conondition);
            MySqlParameter[] parList = new MySqlParameter[] {
                new MySqlParameter("@" + fieldName, newValue)
            };
            byte ret = Convert.ToByte(ExecuteScalarParams(sql, parList));
            return ret > 0 ? true : false;
        }

        /// <summary>
        /// 判断表某字段是否重复
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="condition">判断条件（不带关键词WHERE）</param>
        /// <returns>bool</returns>
        public bool IsDuplicate(string tableName, string condition)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0} WHERE {1}", tableName, condition);
            byte ret = Convert.ToByte(ExecuteScalar(sql));
            return ret > 0 ? true : false;
        }

        #endregion

        #region 获取当前连接串中的数据库名称


        /// <summary>
        /// 获取数据库连接串中涉及的数据库名,通过database关键字获取
        /// </summary>
        /// <returns></returns>
        public string GetDBName()
        {
            try
            {
                var ary = ConnStr.ToLower().Trim().Split(';');

                foreach (var item in ary)
                {
                    if (item.Contains("database"))
                    {
                        if (item.IndexOf('=') > -1)
                        {
                            return item.Split('=')[1];
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
