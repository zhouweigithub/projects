//***********************************************************************************
//文件名称：BaseQuery.cs
//功能描述：表单查询基类（针对单表的增/删/改/查的操作）
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using Sunny.Model.Common;
using Sunny.Common;
using MySql.Data.MySqlClient;

namespace Sunny.DAL
{
    /// <summary>
    /// 表单查询基类（针对单表的增/删/改/查的操作）
    /// 文件功能描述：模块类，数据库常用操作的抽象类，包含了对于单表的常见增、删、改、查及缓存操作
    /// 依赖说明：Config、WriteLog、DBHelper、Cache
    /// 异常处理：捕获异常，当出现异常时，会通过WriteLog输出错误信息到日志文件。
    /// </summary>
    public class BaseQuery
    {

        #region fields

        /// <summary>
        /// 数据库中的表名
        /// </summary>
        protected string TableName = string.Empty;

        /// <summary>
        /// 数据库表名的后缀（适用于动态分表的情形。如果没有后缀留空；如果有后缀则在每一个具体操作之前先为后缀赋值）
        /// </summary>
        public string TableNameSuffix = string.Empty;

        /// <summary>
        /// 查询项目名称
        /// </summary>
        protected string ItemName = string.Empty;

        private bool _isAddIntoCache = false;

        /// <summary>
        /// 是否写入缓存（如果是动态表——表名带后缀，则始终不会写缓存）
        /// </summary>
        protected bool IsAddIntoCache
        {
            get { return _isAddIntoCache && string.IsNullOrWhiteSpace(TableNameSuffix); }
            set { _isAddIntoCache = value; }
        }

        /// <summary>
        /// 缓存键名
        /// </summary>
        protected string CacheKey
        {
            get
            {
                return "CacheKeyQuery" + TableName;
            }
        }

        /// <summary>
        /// 缓存有效时间（分钟）
        /// </summary>
        protected int CacheTimeOut = 30;

        /// <summary>
        /// 主键字段
        /// </summary>
        protected string KeyField = "ID";

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderbyFields = "ID ASC";

        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="keyField">主键字段（只针对单个主键有效，联合主键无效）</param>
        /// <param name="orderbyFields">排序字段（不能带order by）</param>
        /// <param name="isAddIntoCache">是否缓存数据</param>
        public BaseQuery(string tableName, string keyField, string orderbyFields, bool isAddIntoCache)
        {
            TableName = tableName;
            KeyField = keyField;
            OrderbyFields = orderbyFields;
            IsAddIntoCache = isAddIntoCache;
        }

        #region 添加

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="fieldList">字段列表</param>
        /// <param name="valueList">值列表</param>
        /// <returns></returns>
        public int Add(List<string> fieldList, List<object> valueList)
        {
            if (fieldList.Count < 1 || fieldList.Count != valueList.Count)
            {
                return 0;
            }

            StringBuilder sb = new StringBuilder();
            MySqlParameter[] commandParameters = new MySqlParameter[fieldList.Count];
            sb.Append("INSERT INTO " + TableName + TableNameSuffix + " (");
            sb.Append(string.Join(",", fieldList.ToArray()) + " ) VALUES (");
            for (int i = 0; i < fieldList.Count; i++)
            {
                sb.AppendFormat("@{0},", fieldList[i]);
                commandParameters[i] = new MySqlParameter("@" + fieldList[i], valueList[i]);
            }
            sb.Length--;
            sb.Append(")");

            int row = 0;
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    row = dbHelper.ExecuteNonQueryParams(sb.ToString(), commandParameters);
                    if (row > 0 && IsAddIntoCache)
                    {
                        Cache.Remove(CacheKey);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "添加记录到表" + ItemName + TableName + TableNameSuffix + "出错\t" + ex.Message);
            }

            return row;
        }


        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="fieldValue">字段值列表</param>
        /// <returns>返回成功条数</returns>
        public int Add(Dictionary<string, object> fieldValue)
        {
            if (fieldValue.Count == 0)
            {
                return 0;
            }
            else
            {
                List<string> fieldList = new List<string>();
                List<object> valueList = new List<object>();
                foreach (var item in fieldValue)
                {
                    fieldList.Add(item.Key);
                    valueList.Add(item.Value);
                }

                return Add(fieldList, valueList);
            }
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <typeparam name="T">待添加的模型类型  此类型变量必须和数据库字段一一对应</typeparam>
        /// <param name="item">模型实例</param>
        /// <returns>返回成功条数</returns>
        public int Add<T>(T item)
        {
            return Add(ToDictionary(item));
        }

        public int Add<T>(T item, List<string> excludeFields)
        {
            return Add(ToDictionary(item, excludeFields));
        }



        #endregion

        #region 修改

        /// <summary>
        /// 根据主键更新记录
        /// </summary>
        /// <param name="fieldList">要修改的字段列表</param>
        /// <param name="valueList">要修改的值列表</param>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int UpdateByKey(List<string> fieldList, List<object> valueList, object keyValue)
        {
            return Update(fieldList, valueList, KeyField + "=" + keyValue);
        }

        /// <summary>
        /// 根据主键更新记录
        /// </summary>
        /// <param name="fieldValue">要修改的字段值列表</param>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int UpdateByKey(Dictionary<string, object> fieldValue, object keyValue)
        {
            if (fieldValue.Count == 0)
            {
                return 0;
            }
            else
            {
                List<string> fieldList = new List<string>();
                List<object> valueList = new List<object>();
                foreach (var item in fieldValue)
                {
                    //忽略主键
                    if (item.Key.ToLower() == this.KeyField.ToLower())
                    {
                        continue;
                    }

                    fieldList.Add(item.Key);
                    valueList.Add(item.Value);
                }

                return UpdateByKey(fieldList, valueList, keyValue);
            }
        }


        /// <summary>
        /// 根据主键更新记录 此方法会修改模型对对应的所有字段,调用时请注意
        /// </summary>
        /// <typeparam name="T">要修改的类型 此类型变量必须和数据库字段一一对应</typeparam>
        /// <param name="item">要修改的类型 实例</param>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回成功条数</returns>
        public int UpdateByKey<T>(T item, object keyValue)
        {
            return UpdateByKey(ToDictionary(item), keyValue);
        }
        public int UpdateByKey<T>(T item, List<string> excludeFields, object keyValue)
        {
            return UpdateByKey(ToDictionary(item, excludeFields), keyValue);
        }

        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="fieldList">要修改的字段列表</param>
        /// <param name="valueList">要修改的值列表</param>
        /// <param name="where">where子句，不带参数</param>
        /// <param name="values">新值</param>
        /// <returns></returns>
        public int Update(List<string> fieldList, List<object> valueList, string where)
        {
            if (fieldList.Count < 1 || fieldList.Count != valueList.Count)
            {
                return 0;
            }

            MySqlParameter[] commandParameters = new MySqlParameter[] { };

            StringBuilder sb = new StringBuilder();
            commandParameters = new MySqlParameter[fieldList.Count];
            sb.Append("UPDATE " + TableName + TableNameSuffix + " SET ");
            for (int i = 0; i < fieldList.Count; i++)
            {
                sb.AppendFormat("{0}=@{0},", fieldList[i]);
                commandParameters[i] = new MySqlParameter("@" + fieldList[i], valueList[i]);
            }
            sb.Length--;

            if (!string.IsNullOrWhiteSpace(where))
            {
                sb.Append(" WHERE " + where);
            }

            int row = 0;
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    row = dbHelper.ExecuteNonQueryParams(sb.ToString(), commandParameters);
                    if (row > 0 && IsAddIntoCache)
                    {
                        Cache.Remove(CacheKey);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "修改表" + "添加记录到表" + ItemName + TableName + TableNameSuffix + "记录出错\t" + ex.Message);
            }

            return row;
        }


        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="fieldValue">要修改的字段值列表</param>
        /// <param name="where">where子句，不带参数</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public int Update(Dictionary<string, object> fieldValue, string where)
        {
            if (fieldValue.Count == 0)
            {
                return 0;
            }
            else
            {
                List<string> fieldList = new List<string>();
                List<object> valueList = new List<object>();
                foreach (var item in fieldValue)
                {
                    fieldList.Add(item.Key);
                    valueList.Add(item.Value);
                }

                return Update(fieldList, valueList, where);
            }
        }

        /// <summary>
        /// 修改记录
        /// </summary>
        /// <typeparam name="T">待修改模型</typeparam>
        /// <param name="item">待修改模型 实例</param>
        /// <param name="where">where子句，不带关键字，参数用问号占位</param>
        /// <param name="values">参数</param>
        /// <returns>返回成功条数</returns>
        public int Update<T>(T item, string where, params MySqlParameter[] commandParameters)
        {
            return Update(ToDictionary(item), where, commandParameters);
        }

        public int Update<T>(T item, string where, List<string> excludeFields)
        {
            return Update(ToDictionary(item, excludeFields), where);
        }


        #endregion

        #region 删除记录

        /// <summary>
        /// 根据主键删除记录
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public int DeleteByKey(object keyValue)
        {
            MySqlParameter[] commandParameters = new MySqlParameter[] {
                new MySqlParameter("@"+KeyField,keyValue)
            };
            return Delete(KeyField + "=@" + KeyField, commandParameters);
        }

        /// <summary>
        /// 根据限定条件删除记录
        /// </summary>
        /// <param name="where">where子句，不带关键字，参数用问号占位</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public int Delete(string where, params MySqlParameter[] commandParameters)
        {
            int row = 0;
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    row = dbHelper.ExecuteNonQueryParams("DELETE FROM " + TableName + TableNameSuffix + " WHERE " + where, commandParameters);
                    if (row > 0 && IsAddIntoCache)
                    {
                        Cache.Remove(CacheKey);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "删除表" + ItemName + TableName + TableNameSuffix + "记录出错\t" + ex.Message);
            }

            return row;
        }

        /// <summary>
        /// 删除表所有记录
        /// </summary>
        /// <returns></returns>
        public int DeleteAll()
        {
            int row = 0;
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    row = dbHelper.ExecuteNonQueryParams("DELETE FROM " + TableName + TableNameSuffix, null);
                    if (row > 0 && IsAddIntoCache)
                    {
                        Cache.Remove(CacheKey);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "删除表" + ItemName + TableName + TableNameSuffix + "所有记录出错\t" + ex.Message);
            }

            return row;
        }

        /// <summary>
        /// 清空表
        /// </summary>
        /// <returns></returns>
        public bool Truncate()
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    dbHelper.ExecuteNonQueryParams("TRUNCATE TABLE " + TableName + TableNameSuffix, null);
                    if (IsAddIntoCache)
                    {
                        Cache.Remove(CacheKey);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "清空表" + ItemName + TableName + TableNameSuffix + "出错\t" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除表，慎用！！！！
        /// </summary>
        /// <returns></returns>
        public bool Drop()
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    dbHelper.ExecuteNonQuery("DROP TABLE " + TableName + TableNameSuffix);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "删除表" + ItemName + TableName + TableNameSuffix + "出错\t" + ex.Message);

                return false;
            }

            return true;
        }

        #endregion

        #region 读数据

        /// <summary>
        /// 查询最大ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxID()
        {
            string sql = "SELECT MAX(" + KeyField + ") FROM " + TableName + TableNameSuffix;

            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    return Convert.ToInt32(dbHelper.ExecuteScalarInt(sql));
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "读取表" + ItemName + TableName + TableNameSuffix + "最大ID出错\t" + ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return GetCount("1=1", null);
        }

        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <param name="where">where子句，不可为空，不带关键字，参数用问号占位</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public int GetCount(string where, params MySqlParameter[] commandParameters)
        {
            string sql = "SELECT COUNT(*) FROM " + TableName + TableNameSuffix + " WHERE " + where;

            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    return Convert.ToInt32(dbHelper.ExecuteScalarIntParams(sql, commandParameters));
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "读取表" + ItemName + TableName + TableNameSuffix + "记录条数出错\t" + ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// 从缓存中获取数据
        /// </summary>
        /// <returns></returns>
        protected DataTable GetTableFromCache()
        {
            if (IsAddIntoCache)
            {
                object obj = Cache.Get(CacheKey);
                if (obj != null)
                {
                    return (DataTable)obj;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            DataTable dt = null;
            if (IsAddIntoCache)
            {
                dt = GetTableFromCache();
            }
            if (dt != null)
            {
                return dt;
            }
            dt = GetTable("1=1", null);
            if (dt != null && IsAddIntoCache)
            {
                Cache.Add(CacheKey, dt, CacheTimeOut);
            }

            return dt;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public IList<T> GetList<T>() where T : new()
        {
            return ToEntityList<T>(GetTable());
        }


        /// <summary>
        /// 获取列表 默认已主键OrderBy排序
        /// </summary>
        /// <param name="where">where子句，不可为空，不带关键字，参数用问号占位</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public DataTable GetTable(string where, params MySqlParameter[] commandParameters)
        {
            DataTable dt = null;
            string sql = "SELECT * FROM " + TableName + TableNameSuffix + " WHERE " + where + (string.IsNullOrWhiteSpace(OrderbyFields) ? "" : " ORDER BY " + OrderbyFields);

            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    dt = dbHelper.ExecuteDataTableParams(sql, commandParameters);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "读取表" + ItemName + TableName + TableNameSuffix + "数据出错\t" + ex.Message);
            }

            return dt;
        }

        /// <summary>
        /// 获取指定字段的DataTable
        /// </summary>
        /// <param name="fields">字段列表 多个使用逗号连接</param>
        /// <returns></returns>
        public DataTable GetTableFields(string fields)
        {
            DataTable dt = null;
            string sql = "SELECT " + fields + " FROM " + TableName + TableNameSuffix + (string.IsNullOrWhiteSpace(OrderbyFields) ? "" : " ORDER BY " + OrderbyFields);

            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    dt = dbHelper.ExecuteDataTable(sql);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "读取表" + ItemName + TableName + TableNameSuffix + "数据出错\t" + ex.Message);
            }

            return dt;
        }

        /// <summary>
        /// 获取指定字段的DataTable
        /// </summary>
        /// <param name="fields">字段列表 多个使用逗号连接</param>
        /// <param name="where">where子句，不可为空，不带关键字，参数用问号占位</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public DataTable GetTableFields(string fields, string where, params MySqlParameter[] commandParameters)
        {
            DataTable dt = null;
            string sql = "SELECT " + fields + " FROM " + TableName + TableNameSuffix + " WHERE " + where + (string.IsNullOrWhiteSpace(OrderbyFields) ? "" : " ORDER BY " + OrderbyFields);

            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    dt = dbHelper.ExecuteDataTableParams(sql, commandParameters);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "读取表" + ItemName + TableName + TableNameSuffix + "数据出错\t" + ex.Message);
            }

            return dt;
        }

        /// <summary>
        /// 分页查询获取指定字段的DataTable
        /// </summary>
        /// <param name="fields">字段列表 多个使用逗号连接</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public DataTable GetTableFieldsPage(string fields, int pageSize, int pageIndex)
        {
            DataTable dt = null;
            string sql = "SELECT " + fields + " FROM " + TableName + TableNameSuffix + (string.IsNullOrWhiteSpace(OrderbyFields) ? "" : " ORDER BY " + OrderbyFields);

            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    dt = dbHelper.ExecuteDataTablePage(sql, pageSize, pageIndex);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "分页读取表" + ItemName + TableName + TableNameSuffix + "数据出错\t" + ex.Message);
            }

            return dt;
        }

        /// <summary>
        /// 分页查询获取指定字段的DataTable
        /// </summary>
        /// <param name="fields">字段列表 多个使用逗号连接</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="where">where子句，不可为空，不带关键字，参数用问号占位</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public DataTable GetTableFieldsPage(string fields, int pageSize, int pageIndex, string where, params MySqlParameter[] commandParameters)
        {
            DataTable dt = null;
            string sql = "SELECT " + fields + " FROM " + TableName + TableNameSuffix + " WHERE " + where + (string.IsNullOrWhiteSpace(OrderbyFields) ? "" : " ORDER BY " + OrderbyFields);

            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    dt = dbHelper.ExecuteDataTablePageParams(sql, pageSize, pageIndex, commandParameters);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "分页读取表" + ItemName + TableName + TableNameSuffix + "数据出错\t" + ex.Message);
            }

            return dt;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="where">where子句，不可为空，不带关键字，参数用问号占位</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public IList<T> GetList<T>(string where, params MySqlParameter[] commandParameters) where T : new()
        {
            return ToEntityList<T>(GetTable(where, commandParameters));
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public DataTable GetTablePage(int pageSize, int pageIndex)
        {
            return GetTablePage(pageSize, pageIndex, "1=1", null);
        }

        /// <summary>
        /// 根据页面获取分页列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        public IList<T> GetListPage<T>(int pageSize, int pageIndex) where T : new()
        {
            return ToEntityList<T>(GetTablePage(pageSize, pageIndex));
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="where">where子句，不带关键字，参数用问号占位</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public DataTable GetTablePage(int pageSize, int pageIndex, string where, params MySqlParameter[] commandParameters)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    return dbHelper.ExecuteDataTablePageParams("SELECT * FROM " + TableName + TableNameSuffix + " WHERE " + where + (string.IsNullOrEmpty(OrderbyFields) ? "" : (" ORDER BY " + OrderbyFields)), pageSize, pageIndex, commandParameters);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "分页查询表" + ItemName + TableName + TableNameSuffix + "数据出错\t" + ex.Message);
            }

            return null;
        }

        /// <summary>
        /// 根据页面获取分页列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="where">where子句，不可为空，不带关键字，参数用问号占位</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public IList<T> GetListPage<T>(int pageSize, int pageIndex, string where, params MySqlParameter[] commandParameters) where T : new()
        {
            return ToEntityList<T>(GetTablePage(pageSize, pageIndex, where, commandParameters));
        }

        /// <summary>
        /// 根据限定条件查询一条记录
        /// </summary>
        /// <param name="where">where子句，不可为空，不带关键字，参数用问号占位</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public DataRow GetRow(string where, params MySqlParameter[] commandParameters)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    return dbHelper.ExecuteDataRowParams("SELECT * FROM " + TableName + TableNameSuffix + " WHERE " + where, commandParameters);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "读取表" + ItemName + TableName + TableNameSuffix + "记录出错\t" + ex.Message);
            }

            return null;
        }

        /// <summary>
        /// 根据限定条件查询一条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="where">where子句，不可为空，不带关键字，参数用问号占位</param>
        /// <param name="values">参数</param>
        /// <returns></returns>
        public T GetEntity<T>(string where, params MySqlParameter[] commandParameters) where T : new()
        {
            return ToEntity<T>(GetRow(where, commandParameters));
        }

        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataRow GetRowByKey(object keyValue)
        {
            string sql = "SELECT * FROM " + TableName + TableNameSuffix + " WHERE " + KeyField + "=@" + KeyField;
            try
            {
                MySqlParameter[] commandParameters = new MySqlParameter[] {
                    new MySqlParameter("@" + KeyField, keyValue)
                };

                using (DBHelper dbHelper = new DBHelper())
                {
                    return dbHelper.ExecuteDataRowParams(sql, commandParameters);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "读取表" + ItemName + TableName + TableNameSuffix + "键为[" + keyValue + "]行出错\t" + ex.Message);
            }

            return null;
        }

        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public T GetEntityByKey<T>(object keyValue) where T : new()
        {
            return ToEntity<T>(GetRowByKey(keyValue));
        }


        /// <summary>
        /// 根据键值获取记录某一字段值
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public object GetValueByKey(object keyValue, string fieldName)
        {
            DataRow dr = GetRowByKey(keyValue);
            if (dr != null)
            {
                return dr[fieldName];
            }

            return null;
        }

        /// <summary>
        /// 根据键值获取记录某一字段值
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fieldName">要获取的字段名</param>
        /// <returns></returns>
        public string GetStringValueByKey(object keyValue, string fieldName)
        {
            DataRow dr = GetRowByKey(keyValue);
            if (dr != null)
            {
                return Convert.ToString(dr[fieldName]);
            }

            return string.Empty;
        }

        /// <summary>
        /// 根据键值获取记录某一字段值
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fieldName">要获取的字段名</param>
        /// <returns></returns>
        public int GetIntValueByKey(object keyValue, string fieldName)
        {
            DataRow dr = GetRowByKey(keyValue);
            if (dr != null)
            {
                return Convert.ToInt32(dr[fieldName]);
            }

            return 0;
        }

        /// <summary>
        /// 根据键值获取记录某一字段值
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fieldName">要获取的字段名</param>
        /// <returns></returns>
        public long GetLongValueByKey(object keyValue, string fieldName)
        {
            DataRow dr = GetRowByKey(keyValue);
            if (dr != null)
            {
                return Convert.ToInt64(dr[fieldName]);
            }

            return 0;
        }

        /// <summary>
        /// 根据键值获取记录某一字段值
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fieldName">要获取的字段名</param>
        /// <returns></returns>
        public byte GetByteValueByKey(object keyValue, string fieldName)
        {
            DataRow dr = GetRowByKey(keyValue);
            if (dr != null)
            {
                return Convert.ToByte(dr[fieldName]);
            }

            return 0;
        }

        /// <summary>
        /// 根据键值获取记录某一字段值
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fieldName">要获取的字段名</param>
        /// <returns></returns>
        public bool GetBoolValueByKey(object keyValue, string fieldName)
        {
            DataRow dr = GetRowByKey(keyValue);
            if (dr != null)
            {
                return Convert.ToInt32(dr[fieldName]) != 0;
            }

            return false;
        }

        #endregion

        #region 验证数据唯一性

        /// <summary>
        /// 检测字段值是否重复
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fieldName">重复验证的字段名</param>
        /// <param name="value">重复验证的字段值</param>
        /// <returns></returns>
        public bool IsDuplicate(object keyValue, string fieldName, string value)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM " + TableName + TableNameSuffix + " WHERE " + KeyField + "<>@" + KeyField + " AND " + fieldName + "=@" + fieldName;

                using (DBHelper dbHelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@" + KeyField, keyValue),
                        new MySqlParameter("@" + fieldName, value)
                    };
                    return Convert.ToInt32(dbHelper.ExecuteScalarIntParams(sql, commandParameters)) > 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "验证表" + ItemName + TableName + TableNameSuffix + "值重复性出错\t" + ex.Message);
            }

            return true;
        }

        #endregion

        #region 类型转换
        /// <summary>
        /// 根据模型反射得到字典
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="item">模型实例</param>
        /// <returns>返回字典,Key:string Value:object</returns>
        public Dictionary<string, object> ToDictionary<T>(T item)
        {
            return ToDictionary(item, null);
        }

        /// <summary>
        /// 根据模型反射得到字典
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="item">模型实例</param>
        /// <param name="excludeFields">要忽略的字段列表</param>
        /// <returns>返回字典,Key:string Value:object</returns>
        public Dictionary<string, object> ToDictionary<T>(T item, List<string> excludeFields)
        {
            Dictionary<string, object> retdic = new Dictionary<string, object>();
            Dictionary<string, byte> dic = new Dictionary<string, byte>();


            if (excludeFields != null)
            {
                foreach (string f in excludeFields)
                {
                    if (!dic.ContainsKey(f.ToLower()))
                    {
                        dic.Add(f.ToLower(), 1);
                    }
                }
            }

            try
            {
                foreach (var p in item.GetType().GetProperties())
                {   //排除非表字段属性
                    if (!dic.ContainsKey(p.Name.ToLower()) && Attribute.IsDefined(p, typeof(TableFieldAttribute)))
                    {
                        retdic.Add(p.Name, p.GetValue(item, null));
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "将表" + ItemName + TableName + TableNameSuffix + "的Model转换为字典出错\t" + ex.Message);
            }

            return retdic;
        }

        /// <summary>
        /// 根据模型 反射得到字段,从DataTable里面转换得到ListModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public IList<T> ToEntityList<T>(DataTable dt) where T : new()
        {
            if (dt == null)
            {
                return null;
            }

            IList<T> list = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(ToEntity<T>(dr));
            }

            return list;
        }

        /// <summary>
        /// 根据模型 反射得到字段,从DataRow反射得到实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="dr">一行记录</param>
        /// <returns></returns>
        public T ToEntity<T>(DataRow dr) where T : new()
        {
            if (dr == null)
            {
                return default(T);
            }

            T t = new T();

            try
            {
                foreach (var pi in t.GetType().GetProperties())
                {
                    if (!pi.CanWrite)
                    {
                        continue;
                    }
                    if (!dr.Table.Columns.Contains(pi.Name))
                    {
                        continue;
                    }
                    object value = dr[pi.Name];
                    if (dr[pi.Name].GetType().ToString().Contains("DateTime"))
                    {
                        try
                        {
                            value = Convert.ToDateTime(dr[pi.Name]);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (value != DBNull.Value)
                    {

                        pi.SetValue(t, value, null);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "将表" + ItemName + TableName + TableNameSuffix + "DataRow转换为：Entity数据出错\t" + ex.Message);
            }

            return t;
        }

        #endregion

        #region 导入txt到数据库

        /// <summary>
        /// 从本地文件导入数据到数据库表中，默认：文件为UTF-8编码，导入到派生类对应的表，导入所有字段，字段分隔符为\t，记录分隔符为\n
        /// </summary>
        /// <param name="fileName">本地文件名</param>
        /// <returns>导入记录条数</returns>
        public int LoadDataInLocalFile(string fileName)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    return dbHelper.LoadDataInLocalFile(TableName + TableNameSuffix, fileName);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "从文件" + fileName + "导入表" + ItemName + TableName + TableNameSuffix + "记录出错\t" + ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中，默认：文件为UTF-8编码，导入到派生类对应的表，字段分隔符为\t，记录分隔符为\n
        /// </summary>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fieldsTerminated">字段列表</param>
        /// <returns>导入记录条数</returns>
        public int LoadDataInLocalFile(string fileName, List<string> fields)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    return dbHelper.LoadDataInLocalFile(TableName + TableNameSuffix, fileName, fields);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "从文件" + fileName + "导入表" + ItemName + TableName + TableNameSuffix + "记录出错\t" + ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中，默认：文件为UTF-8编码，导入到派生类对应的表，导入所有字段，记录分隔符为\n
        /// </summary>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段分隔符</param>
        /// <returns>导入记录条数</returns>
        public int LoadDataInLocalFile(string fileName, string fieldsTerminated)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    return dbHelper.LoadDataInLocalFile(TableName + TableNameSuffix, fileName, fieldsTerminated);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "从文件" + fileName + "导入表" + ItemName + TableName + TableNameSuffix + "记录出错\t" + ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中，默认：文件为UTF-8编码，导入到派生类对应的表，记录分隔符为\n
        /// </summary>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="fieldsTerminated">字段分隔符</param>
        /// <returns>导入记录条数</returns>
        public int LoadDataInLocalFile(string fileName, List<string> fields, string fieldsTerminated)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    return dbHelper.LoadDataInLocalFile(TableName + TableNameSuffix, fileName, fields, fieldsTerminated);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "从文件" + fileName + "导入表" + ItemName + TableName + TableNameSuffix + "记录出错\t" + ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中，默认：文件为UTF-8编码，导入到派生类对应的表
        /// </summary>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="fieldsTerminated">字段分隔符</param>
        /// <param name="linesTerminated">记录分隔符</param>
        /// <returns>导入记录条数</returns>
        public int LoadDataInLocalFile(string fileName, List<string> fields, string fieldsTerminated, string linesTerminated)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    return dbHelper.LoadDataInLocalFile(TableName + TableNameSuffix, fileName, fields, fieldsTerminated, linesTerminated);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "从文件" + fileName + "导入表" + ItemName + TableName + TableNameSuffix + "记录出错\t" + ex.Message);
            }

            return 0;
        }

        #endregion

        #region 从内存导入数据到数据库

        /// <summary>
        /// 从DataTable导入数据到数据库表（适用于小批量数据导入），默认导入到派生类对应的表
        /// </summary>
        /// <param name="dt">数据表（字段名通过ColumnName来指定）</param>
        /// <returns></returns>
        public int LoadDataInDataTable(DataTable dt)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    return dbHelper.LoadDataInDataTable(TableName + TableNameSuffix, dt);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "从DataTable导入表" + ItemName + TableName + TableNameSuffix + "记录出错\t" + ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// 从List导入数据到数据库表（适用于小批量数据导入），默认导入到派生类对应的表
        /// </summary>
        /// <param name="list">数据列表（每条记录为一个字典，字典的键为字段名，值为字段值</param>
        /// <returns>导入数据的条数</returns>
        public int LoadDataInList(List<Dictionary<string, object>> list)
        {
            try
            {
                using (DBHelper dbHelper = new DBHelper())
                {
                    return dbHelper.LoadDataInList(TableName + TableNameSuffix, list);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "从List导入表" + ItemName + TableName + TableNameSuffix + "记录出错\t" + ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// 从List导入数据到数据库表（适用于小批量数据导入），默认导入到派生类对应的表
        /// </summary>
        /// <param name="list">数据列表（每条记录为一个字典，字典的键为字段名，值为字段值</param>
        /// <returns>导入数据的条数</returns>
        public int LoadDataInList<T>(List<T> list)
        {
            return LoadDataInList(list, null);
        }

        /// <summary>
        /// 从List导入数据到数据库表（适用于小批量数据导入），默认导入到派生类对应的表
        /// </summary>
        /// <param name="list">数据列表（每条记录为一个字典，字典的键为字段名，值为字段值</param>
        /// <param name="excludeFields">要忽略的字段列表</param>
        /// <returns>导入数据的条数</returns>
        public int LoadDataInList<T>(List<T> list, List<string> excludeFields)
        {
            try
            {
                List<Dictionary<string, object>> l = new List<Dictionary<string, object>>();
                foreach (var item in list)
                {
                    l.Add(ToDictionary<T>(item, excludeFields));
                }

                //导入
                return LoadDataInList(l);
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "从List导入表" + ItemName + TableName + TableNameSuffix + "记录出错\t" + ex.Message);
            }

            return 0;
        }

        #endregion




    }
}
