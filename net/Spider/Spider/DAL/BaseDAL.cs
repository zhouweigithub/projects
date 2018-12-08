
using System;
using System.Data;
using System.Linq;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using Spider.Common;

namespace Spider.DAL
{
    /// <summary>
    /// 数据处理的基类
    /// </summary>
    public class BaseDAL
    {
        /// <summary>
        /// 运营数据库连接字符串
        /// </summary>
        public static String OperationDBConnection { get; set; }

        /// <summary>
        /// 执行无返回值的数据库操作，仅返回受影响的行数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>受影响的行数</returns>
        public static Int32 ExecuteNonQuery(String connectionString, String commandText, params MySqlParameter[] commandParameters)
        {
            try
            {
                return MySqlHelper.ExecuteNonQuery(connectionString, commandText, commandParameters);
            }
            catch (Exception ex)
            {
                throw new Exception(JoinErrorMessage(ex, commandText, commandParameters));
            }
        }

        /// <summary>
        /// 执行返回单个值的数据库操作
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>返回单个值</returns>
        public static Object ExecuteScalar(String connectionString, String commandText, params MySqlParameter[] commandParameters)
        {
            try
            {
                return MySqlHelper.ExecuteScalar(connectionString, commandText, commandParameters);
            }
            catch (Exception ex)
            {
                LogUtil.Write(connectionString, LogType.Error);
                throw new Exception(JoinErrorMessage(ex, commandText, commandParameters));
            }
        }

        /// <summary>
        /// 执行数据库操作，返回一个数据表
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>包含结果的数据表</returns>
        public static DataTable ExecuteDataTable(String connectionString, String commandText, params MySqlParameter[] commandParameters)
        {
            try
            {
                DataSet ds = MySqlHelper.ExecuteDataset(connectionString, commandText, commandParameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }

                return null;
            }
            catch (Exception ex)
            {
                LogUtil.Write(connectionString, LogType.Error);

                throw new Exception(JoinErrorMessage(ex, commandText, commandParameters));
            }
        }

        /// <summary>
        /// 拼接错误信息
        /// </summary>
        /// <param name="ex">异常欣喜</param>
        /// <param name="commandText">执行的sql语句</param>
        /// <param name="commandParameters">参数信息</param>
        /// <returns></returns>
        private static String JoinErrorMessage(Exception ex, String commandText, MySqlParameter[] commandParameters)
        {
            String stackTraceMessage = ex.StackTrace ?? String.Empty;
            String exMessage = ex.Message ?? String.Empty;
            String paramMessage = String.Empty;
            if (commandParameters != null && commandParameters.Length > 0)
            {
                paramMessage = String.Join(", ", commandParameters.Select(item => String.Format("{0} = {1}", item.ParameterName, item.Value.ToString())));
            }

            return String.Format("Message:{1} {0} CommandText:{2} {0} ParamInfo:{3}{0} StackTrace:{4} {0} CallStackTrace:{5}", Environment.NewLine, exMessage, commandText, paramMessage, stackTraceMessage, new StackTrace().ToString());
        }
    }
}