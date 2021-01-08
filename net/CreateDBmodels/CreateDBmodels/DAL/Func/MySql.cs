using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace CreateDBmodels.DAL
{
    /// <summary>
    /// Mysql数据库实例
    /// 文件功能描述：模块类，SqlServer数据库操作类，在这里实现了该数据库的相关特性
    /// 依赖说明：无依赖，不要直接实例化，通过DBHelper来调用。
    /// 异常处理：捕获但不处理异常。
    /// </summary>
    public class Mysql : DBOperator
    {
        private String _connString;
        private MySqlConnection _conn;
        private MySqlTransaction _trans;
        /// <summary>
        /// 当前是否在存储过程中
        /// </summary>
        protected Boolean _isInTransaction = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strConnection"></param>
        public Mysql(String strConnection)
        {
            _connString = strConnection;
            _conn = new MySqlConnection(strConnection);
        }

        #region 打开/关闭数据库连接
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public override void Open()
        {
            if (_conn != null && _conn.State != ConnectionState.Open)
            {
                try
                {
                    _conn = new MySqlConnection(_connString);
                    this._conn.Open();
                }
                catch (Exception ee)
                {
                    throw new Exception("打开数据库连接失败。\r\n" + ee.ToString());
                }
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public override void Close()
        {
            if (_conn.State != ConnectionState.Closed)
            {
                try
                {
                    this._conn.Close();
                }
                catch (Exception ee)
                {
                    throw new Exception("关闭数据库连接失败。\r\n" + ee.ToString());
                }
            }
        }
        #endregion

        #region 事务操作
        /// <summary>
        /// 开始事务
        /// </summary>
        public override void BeginTrans()
        {
            if (_isInTransaction)
            {
                throw new Exception("当前事务尚未提交!");
            }

            _trans = _conn.BeginTransaction();
            _isInTransaction = true;
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        public override void CommitTrans()
        {
            _trans.Commit();
            _isInTransaction = false;
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public override void RollbackTrans()
        {
            _trans.Rollback();
            _isInTransaction = false;
        }

        #endregion

        /// <summary>
        /// 创建一个命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected override DbCommand CreateCommand(String command)
        {
            MySqlCommand comm = new MySqlCommand
            {
                Connection = _conn,
                CommandText = command
            };

            return comm;
        }

        /// <summary>
        /// 构造包含参数的SQL语句或命令
        /// </summary>
        /// <param name="command">SQL语句或命令</param>
        /// <param name="value">参数值列表</param>
        protected override DbCommand CreateCommand(String command, params MySqlParameter[] commandParameters)
        {
            MySqlCommand comm = new MySqlCommand
            {
                Connection = _conn,
                CommandText = command
            };

            if (commandParameters != null && commandParameters.Length > 0)
            {
                comm.Parameters.AddRange(commandParameters);
            }

            return comm;
        }

        #region 无参数调用
        /// <summary>
        /// A2返回一个DataSet
        /// </summary>
        /// <param name="sql">SQL语句或命令</param>
        /// <returns>DataSet</returns>
        public override DataSet ExecuteDataSet(String sql)
        {
            MySqlCommand comm = (MySqlCommand)CreateCommand(sql);

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataSet ds = new DataSet();
            adapter.SelectCommand = comm;

            adapter.Fill(ds);
            return ds;
        }

        #endregion

        #region 带参数查询
        /// <summary>
        /// B2返回一个DataSet
        /// </summary>
        /// <param name="sql">SQL语句或命令</param>
        /// <param name="value">参数值列表</param>
        /// <returns>DataSet</returns>
        public override DataSet ExecuteDataSetParams(String sql, params MySqlParameter[] commandParameters)
        {
            MySqlCommand comm = (MySqlCommand)CreateCommand(sql, commandParameters);

            DataSet ds = new DataSet();
            MySqlDataAdapter adapter = new MySqlDataAdapter
            {
                SelectCommand = comm
            };
            adapter.Fill(ds);
            return ds;
        }

        #endregion

        #region 分页查询

        /// <summary>
        /// 分页查询，返回DataTable
        /// </summary>
        /// <param name="sql">除去分页之外的SQL语句</param>
        /// <param name="pageSize">页面大小（单页记录条数）</param>
        /// <param name="pageIndex">当前页码（页号从1开始）</param>
        /// <returns></returns>
        public override DataSet ExecuteDataSetPage(String sql, Int32 pageSize, Int32 pageIndex)
        {
            String s = String.Format("SELECT * FROM ( {0} )t LIMIT {1} OFFSET {2}", sql, pageSize, (pageIndex - 1) * pageSize);
            return ExecuteDataSet(s);
        }

        /// <summary>
        /// 分页查询，返回DataTable
        /// </summary>
        /// <param name="sql">除去分页之外的SQL语句</param>
        /// <param name="pageSize">页面大小（单页记录条数）</param>
        /// <param name="pageIndex">当前页码（页号从1开始）</param>
        /// <param name="value">参数列表</param>
        /// <returns></returns>
        public override DataSet ExecuteDataSetPageParams(String sql, Int32 pageSize, Int32 pageIndex, params MySqlParameter[] commandParameters)
        {
            String s = String.Format("SELECT * FROM ( {0} )t LIMIT {1} OFFSET {2}", sql, pageSize, (pageIndex - 1) * pageSize);
            return ExecuteDataSetParams(s, commandParameters);
        }

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
        public override DataTable ExecuteDataTablePageParams(String tableName, String fields, String keyField, String groupBy, String orderBy, Int32 pageSize, Int32 pageIndex, String condition, params MySqlParameter[] commandParameters)
        {
            if (pageIndex > 0)
            {
                pageIndex = pageIndex - 1;
            }

            Int64 offset = pageIndex * pageSize;
            if (!String.IsNullOrEmpty(condition))
            {
                condition = " WHERE " + condition + " ";
            }
            if (!String.IsNullOrEmpty(groupBy))
            {
                groupBy = " GROUP BY " + groupBy + " ";
            }
            if (!String.IsNullOrEmpty(orderBy))
            {
                orderBy = " ORDER BY " + orderBy + " ";
            }

            String sql = String.Format("SELECT * FROM ( SELECT {0} FROM {1} {2} {3} {4} )t LIMIT {5} OFFSET {6}", fields, tableName, condition, groupBy, orderBy, pageSize, offset);

            return ExecuteDataTableParams(sql, commandParameters);
        }

        #endregion

        #region 导入txt到数据库

        /// <summary>
        /// 从本地文件导入数据到数据库表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <returns></returns>
        public override Int32 LoadDataInLocalFile(String tableName, String fileName)
        {
            String sql = String.Format("LOAD DATA LOCAL INFILE \"{0}\" INTO TABLE {1}", fileName.Replace('\\', '/'), tableName);
            return ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fieldsTerminated">字段列表</param>
        /// <returns></returns>
        public override Int32 LoadDataInLocalFile(String tableName, String fileName, List<String> fields)
        {
            String sql = String.Format("LOAD DATA LOCAL INFILE \"{0}\" INTO TABLE {1} ({2})", fileName.Replace('\\', '/'), tableName, String.Join(",", fields));
            return ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段分隔符</param>
        /// <returns></returns>
        public override Int32 LoadDataInLocalFile(String tableName, String fileName, String fieldsTerminated)
        {
            String sql = String.Format("LOAD DATA LOCAL INFILE \"{0}\" INTO TABLE {1} FIELDS TERMINATED BY \"{2}\"", fileName.Replace('\\', '/'), tableName, fieldsTerminated);
            return ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="fieldsTerminated">字段分隔符</param>
        /// <returns></returns>
        public override Int32 LoadDataInLocalFile(String tableName, String fileName, List<String> fields, String fieldsTerminated)
        {
            String sql = String.Format("LOAD DATA LOCAL INFILE \"{0}\" INTO TABLE {1} FIELDS TERMINATED BY \"{2}\" ({3})", fileName.Replace('\\', '/'), tableName, fieldsTerminated, String.Join(",", fields));
            return ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 从本地文件导入数据到数据库表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fileName">本地文件名</param>
        /// <param name="fields">字段列表</param>
        /// <param name="fieldsTerminated">字段分隔符</param>
        /// <param name="linesTerminated">记录分隔符</param>
        /// <returns></returns>
        public override Int32 LoadDataInLocalFile(String tableName, String fileName, List<String> fields, String fieldsTerminated, String linesTerminated)
        {
            String sql = String.Format("LOAD DATA LOCAL INFILE \"{0}\" INTO TABLE {1} FIELDS TERMINATED BY \"{2}\" LINES TERMINATED BY \"{3}\"({4})", fileName.Replace('\\', '/'), tableName, fieldsTerminated, linesTerminated, String.Join(",", fields));
            return ExecuteNonQuery(sql);
        }

        #endregion

        #region 获取表的一些信息

        /// <summary>
        /// 根据表名，获取字段列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>表字段列表</returns>
        public override List<String> GetFieldsList(String tableName)
        {
            List<String> list = new List<String>();

            String sql = "SELECT * FROM " + tableName + " WHERE 1=0";
            DataTable dt = ExecuteDataTable(sql);
            if (dt != null)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    list.Add(col.ColumnName);
                }
            }

            return list;
        }

        #endregion
    }
}
