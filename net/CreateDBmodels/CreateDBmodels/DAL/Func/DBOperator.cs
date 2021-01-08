using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using MySql.Data.MySqlClient;

namespace CreateDBmodels.DAL
{
    /// <summary>
    /// 数据库操作基类
    /// 文件功能描述：模块类，SqlServer数据库操作类，在这里实现了该数据库的相关特性
    /// 依赖说明：无依赖，不要直接实例化，通过DBHelper来调用具体的实例。
    /// 异常处理：捕获但不处理异常。
    /// </summary>
    public abstract class DBOperator
    {
        /// <summary>
        /// 命令执行时长（秒，默认为30）
        /// </summary>
        public Int32 CommandTimeout = 30;

        /// <summary>
        /// SQL命令
        /// </summary>
        protected DbCommand comm;

        #region 打开/关闭数据库连接

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public abstract void Close();

        #endregion

        #region 事务操作

        /// <summary>
        /// 开始事务
        /// </summary>
        public abstract void BeginTrans();

        /// <summary>
        /// 执行事务
        /// </summary>
        public abstract void CommitTrans();

        /// <summary>
        /// 事务回滚
        /// </summary>
        public abstract void RollbackTrans();

        #endregion

        /// <summary>
        /// 创建一个命令
        /// </summary>
        /// <param name="command">SQL语句或命令</param>
        /// <returns></returns>
        protected abstract DbCommand CreateCommand(String command);

        /// <summary>
        /// 构造包含参数的SQL语句或命令
        /// </summary>
        /// <param name="command">SQL语句或命令</param>
        /// <param name="value">参数值列表</param>
        protected abstract DbCommand CreateCommand(String command, params MySqlParameter[] commandParameters);

        #region 无参数调用

        /// <summary>
        /// 执行SQL语句，返回受影响记录条数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        public Int32 ExecuteNonQuery(String sql)
        {
            DbCommand comm = CreateCommand(sql);

            return comm.ExecuteNonQuery();
        }

        /// <summary>
        /// A2返回一个DataSet
        /// </summary>
        /// <param name="sql">SQL语句或命令</param>
        /// <returns>DataSet</returns>
        public abstract DataSet ExecuteDataSet(String sql);

        /// <summary>
        /// A3执行一条SQL语句，返回一个object
        /// </summary>
        /// <param name="sql">SQL语句或命令</param>
        /// <returns>object</returns>
        public Object ExecuteScalar(String sql)
        {
            DbCommand comm = CreateCommand(sql);

            return comm.ExecuteScalar();
        }

        /// <summary>
        /// 执行一条SQL语句，返回一个DbDataReader
        /// </summary>
        /// <param name="sql">SQL语句或命令</param>
        /// <returns>DbDataReader</returns>
        public DbDataReader ExecuteReader(String sql)
        {
            DbCommand comm = CreateCommand(sql);

            return comm.ExecuteReader();
        }

        /// <summary>
        /// 执行SQL语句，返回DataTable
        /// </summary>
        /// <param name="sql">SQL语句（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(String sql)
        {
            DataSet ds = ExecuteDataSet(sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region 带参数查询
        /// <summary>
        /// B1执行一个语句不返回任何值
        /// </summary>
        /// <param name="sql">SQL语句或命令</param>
        /// <param name="value">参数值列表</param>
        /// <returns>受影响行数</returns>
        public Int32 ExecuteNonQueryParams(String sql, params MySqlParameter[] commandParameters)
        {
            DbCommand comm = CreateCommand(sql, commandParameters);

            return comm.ExecuteNonQuery();
        }

        /// <summary>
        /// B2返回一个DataSet
        /// </summary>
        /// <param name="sql">SQL语句或命令</param>
        /// <param name="value">参数值列表</param>
        /// <returns>DataSet</returns>
        public abstract DataSet ExecuteDataSetParams(String sql, params MySqlParameter[] commandParameters);

        /// <summary>
        /// B3执行一条SQL语句，返回一个object
        /// </summary>
        /// <param name="sql">SQL语句或命令</param>
        /// <param name="value">参数值列表</param>
        /// <returns>object</returns>
        public Object ExecuteScalarParams(String sql, params MySqlParameter[] commandParameters)
        {
            DbCommand comm = CreateCommand(sql, commandParameters);
            comm.CommandTimeout = 0;

            return comm.ExecuteScalar();
        }

        /// <summary>
        /// B4执行一条SQL语句，返回一个DbDataReader
        /// </summary>
        /// <param name="sql">SQL语句或命令</param>
        /// <param name="value">参数值列表</param>
        /// <returns>DbDataReader</returns>
        public DbDataReader ExecuteReaderParams(String sql, params MySqlParameter[] commandParameters)
        {
            DbCommand comm = CreateCommand(sql, commandParameters);

            return comm.ExecuteReader();
        }

        /// <summary>
        /// 执行SQL语句，返回DataTable
        /// </summary>
        /// <param name="sql">SQL语句或命令（为了Oracle上能够使用，表的别名前不要加AS）</param>
        /// <param name="value">参数值列表</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTableParams(String sql, params MySqlParameter[] commandParameters)
        {
            DataSet ds = ExecuteDataSetParams(sql, commandParameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
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
        public abstract DataSet ExecuteDataSetPage(String sql, Int32 pageSize, Int32 pageIndex);

        /// <summary>
        /// 分页查询，返回DataSet
        /// </summary>
        /// <param name="sql">除去分页之外的SQL语句</param>
        /// <param name="pageSize">页面大小（单页记录条数）</param>
        /// <param name="pageIndex">当前页码（页号从1开始）</param>
        /// <param name="value">参数列表</param>
        /// <returns></returns>
        public abstract DataSet ExecuteDataSetPageParams(String sql, Int32 pageSize, Int32 pageIndex, params MySqlParameter[] commandParameters);

        /// <summary>
        /// 分页查询，返回DataTable
        /// </summary>
        /// <param name="sql">除去分页之外的SQL语句</param>
        /// <param name="pageSize">页面大小（单页记录条数）</param>
        /// <param name="pageIndex">当前页码（页号从1开始）</param>
        /// <returns></returns>
        public DataTable ExecuteDataTablePage(String sql, Int32 pageSize, Int32 pageIndex)
        {
            DataSet ds = ExecuteDataSetPage(sql, pageSize, pageIndex);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
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
        public DataTable ExecuteDataTablePageParams(String sql, Int32 pageSize, Int32 pageIndex, params MySqlParameter[] commandParameters)
        {
            DataSet ds = ExecuteDataSetPageParams(sql, pageSize, pageIndex, commandParameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 分页查询，返回DataTable，该方法已经过时（为了照顾SQLServer2005以下版本）
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
        public abstract DataTable ExecuteDataTablePageParams(String tableName, String fields, String keyField, String groupBy, String orderBy, Int32 pageSize, Int32 pageIndex, String condition, params MySqlParameter[] commandParameters);

        #endregion

        #region 导入txt到数据库

        /// <summary>
        /// 从本地文件导入数据到数据库表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <returns></returns>
        public abstract Int32 LoadDataInLocalFile(String tableName, String fileName);

        /// <summary>
        /// 从本地文件导入数据到数据库表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fieldsTerminated">字段列表</param>
        /// <returns></returns>
        public abstract Int32 LoadDataInLocalFile(String tableName, String fileName, List<String> fields);

        /// <summary>
        /// 从本地文件导入数据到数据库表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段分隔符</param>
        /// <returns></returns>
        public abstract Int32 LoadDataInLocalFile(String tableName, String fileName, String fieldsTerminated);

        /// <summary>
        /// 从本地文件导入数据到数据库表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="fieldsTerminated">字段分隔符</param>
        /// <returns></returns>
        public abstract Int32 LoadDataInLocalFile(String tableName, String fileName, List<String> fields, String fieldsTerminated);

        /// <summary>
        /// 从本地文件导入数据到数据库表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="fieldsTerminated">字段分隔符</param>
        /// <param name="linesTerminated">记录分隔符</param>
        /// <returns></returns>
        public abstract Int32 LoadDataInLocalFile(String tableName, String fileName, List<String> fields, String fieldsTerminated, String linesTerminated);

        #endregion

        #region 获取表的一些信息

        /// <summary>
        /// 根据表名，获取字段列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表字段列表</returns>
        public abstract List<String> GetFieldsList(String tableName);

        #endregion

        #region 显示参数列表(多数情况下用于调试输出)

        /// <summary>
        /// 显示参数列表
        /// </summary>
        /// <param name="paramsList"></param>
        /// <returns></returns>
        public String ShowParamsList(params MySqlParameter[] commandParameters)
        {
            StringBuilder sb = new StringBuilder();

            foreach (MySqlParameter para in commandParameters)
            {
                sb.AppendFormat("{0}={1}\r\n", para.ParameterName, para.Value);
            }

            return sb.ToString();
        }

        #endregion
    }
}
