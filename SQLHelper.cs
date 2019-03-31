using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace code
{
    /// <summary>
    /// 数据库连接帮助类
    /// </summary>
    public abstract class SQLHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static readonly string ConnectionString = "server=.;database=master;uid=sa;pwd=sa";

        #region 执行操作，返回表  +static DataTable ExcuteTable(string sql, CommandType type, params SqlParameter[] ps)
        /// <summary>
        /// 执行操作，返回表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable ExcuteTable(string sql, CommandType type, params SqlParameter[] ps)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, ConnectionString);
            da.SelectCommand.CommandType = type;
            da.SelectCommand.Parameters.AddRange(ps);
            da.Fill(dt);
            return dt;
        }
        #endregion

        #region 执行操作，返回DataSet表集合  +static DataSet ExcuteTable(string sql, CommandType type, params SqlParameter[] ps)
        /// <summary>
        /// 执行操作，返回表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet ExcuteDataSet(string sql, CommandType type, params SqlParameter[] ps)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(sql, ConnectionString);
            da.SelectCommand.CommandType = type;
            da.SelectCommand.Parameters.AddRange(ps);
            da.Fill(ds);
            return ds;
        }
        #endregion

        #region 返回单个值的泛型版本  -static T ExcuteScalar<T>(string sql, params SqlParameter[] ps)
        /// <summary>
        /// 返回单个值的泛型版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static T ExcuteScalar<T>(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(sql, conn);
                comm.Parameters.AddRange(ps);
                object obj = comm.ExecuteScalar(); //标识列返回的值的类型不是int而是decimal
                return default(T);
            }
        }
        #endregion

        #region 返回泛型集合  + static List<T> GetList<T>(DataTable dt)
        /// <summary>
        /// 返回泛型集合
        /// </summary>
        /// <typeparam name="T">类型占位，并不是一个真正存在的类型，只在运行的时候才能确定它的类型是什么</typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(DataTable dt)
        {
            //int num = dt.Columns.Count;
            List<T> lists = new List<T>();
            //获取当前T所指定的类型
            Type t = typeof(T);
            //通过反射的方式得到类型的属性
            PropertyInfo[] ps = t.GetProperties();
            foreach (DataRow row in dt.Rows)
            {
                //每一行对应着一个对象，通过反射的方式创建出对象
                T obj = (T)Activator.CreateInstance(t);
                //不能通过对象.属性的方式来赋值，因为属性是什么都不知道
                //通过反射的方式为对象的属性赋值
                foreach (PropertyInfo p in ps)
                {
                    string name = p.Name;
                    //表的字段名称就是类的属性名称
                    p.SetValue(obj, row[name], null);
                }
                lists.Add(obj);
            }
            return lists;
        }
        #endregion

        #region 用提供的参数，在连接字符串所指定的数据库中执行SQL语句（非查询）
        /// <summary>
        /// 用提供的参数，在连接字符串所指定的数据库中执行SQL语句（非查询）
        /// </summary>
        /// <remarks>
        /// 使用示例:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型（存储过程、文本等）</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">用于执行命令的参数数组</param>
        /// <returns>受命令所影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, commandType, commandText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }
        #endregion

        #region 用提供的参数和存在的数据库连接对象，执行SQL语句（非查询）
        /// <summary>
        /// 用提供的参数和存在的数据库连接对象，执行SQL语句（非查询）
        /// </summary>
        /// <remarks>
        /// 使用示例:  
        ///  int result = ExecuteNonQuery(connection, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">存在的数据库连接对象</param>
        /// <param name="commandType">命令类型（存储过程、文本等）</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">用于执行命令的参数数组</param>
        /// <returns>受命令所影响的行数</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        #endregion

        #region 用提供的参数和存在的事务对象，执行SQL语句（非查询）
        /// <summary>
        /// 用提供的参数和存在的事务对象，执行SQL语句（非查询）
        /// </summary>
        /// <remarks>
        /// 使用示例:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">存在的事务对象</param>
        /// <param name="commandType">命令类型（存储过程、文本等）</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">用于执行命令的参数数组</param>
        /// <returns>受命令所影响的行数</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, commandType, commandText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        #endregion

        #region 用提供的参数，在连接字符串所指定的数据库中执行SQL查询，并返回结果集（SqlDataReader）
        /// <summary>
        /// 用提供的参数，在连接字符串所指定的数据库中执行SQL查询，并返回结果集（SqlDataReader）
        /// </summary>
        /// <remarks>
        /// 使用示例:  
        ///  SqlDataReader r = ExecuteReader(connectionString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型（存储过程、文本等）</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">用于执行命令的参数数组</param>
        /// <returns>一个包含结果的SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // 之所以这里用 try/catch，是因为：
            // 如果方法抛出异常时，我们希望关闭连接并抛出异常
            // 因为此时不会返回 DataReader，故 commandBehaviour.CloseConnection 也不起作用
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        #endregion

        #region 用提供的参数，在连接字符串所指定的数据库中执行SQL查询，并返回查询结果的第一行第一列的值
        /// <summary>
        /// 用提供的参数，在连接字符串所指定的数据库中执行SQL查询，并返回查询结果的第一行第一列的值
        /// </summary>
        /// <remarks>
        /// 使用示例:  
        ///  Object obj = ExecuteScalar(connectionString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">命令类型（存储过程、文本等）</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">用于执行命令的参数数组</param>
        /// <returns>一个 object 对象，可用 Convert.To{Type} 转换为所需类型</returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }
        #endregion

        #region 用提供的参数和存在的数据库连接对象，执行SQL查询，并返回查询结果的第一行第一列的值
        /// <summary>
        /// 用提供的参数和存在的数据库连接对象，执行SQL查询，并返回查询结果的第一行第一列的值
        /// </summary>
        /// <remarks>
        /// 使用示例:  
        ///  Object obj = ExecuteScalar(connection, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">存在的数据库连接对象</param>
        /// <param name="commandType">命令类型（存储过程、文本等）</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">用于执行命令的参数数组</param>
        /// <returns>一个 object 对象，可用 Convert.To{Type} 转换为所需类型</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }
        #endregion

        #region 用提供的参数和存在的数据库连接对象，执行SQL查询，并返回查询结果的DatSet结果集
        /// <summary>
        /// 用提供的参数和存在的数据库连接对象，执行SQL查询，并返回查询结果的DatSet结果集
        /// </summary>
        /// <param name="connectionString">链接语句</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandType">查询模式</param>
        /// <param name="commandParameters">Parameter参数</param>
        /// <returns></returns>
        static public DataSet ExecuteDataSet(string connectionString, string commandText, CommandType commandType, params SqlParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, con, null, commandType, commandText, commandParameters);
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
            }
            return ds;
        }
        #endregion

        #region 构建一个用于执行的命令对象
        /// <summary>
        /// 构建一个用于执行的命令对象
        /// </summary>
        /// <param name="cmd">SqlCommand 对象</param>
        /// <param name="conn">SqlConnection 对象</param>
        /// <param name="trans">SqlTransaction 对象</param>
        /// <param name="cmdType">命令类型（存储过程、文本等）</param>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="cmdParms">用于执行命令的参数数组</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        #endregion
    }
}
