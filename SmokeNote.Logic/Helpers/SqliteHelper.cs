using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Reflection;

namespace SmokeNote.Logic.Helpers
{
    public sealed class SqliteHelper
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        static string ConnectionString
        {
            get;
            set;
        }
        static SqliteHelper()
        {
            //数据库文件路径
            var filePath = System.Environment.CurrentDirectory + @"\SmokeNote.db";
            var connStr = string.Format("Data Source={0};Pooling=true;FailIfMissing=false", filePath);
            ConnectionString = connStr;
        }

        #region 参数
        /// <summary>
        /// 创建一个传入参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <returns></returns>
        public static SQLiteParameter CreateInParameter(string name, object value, DbType type = DbType.String, int size = 50)
        {
            var para = new SQLiteParameter(name, type, size);
            para.Direction = ParameterDirection.Input;
            para.Value = value;
            return para;
        }
        /// <summary>
        /// 创建一个传出参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <returns></returns>
        public static SQLiteParameter CreateOutParameter(string name, DbType type = DbType.String, int size = 50)
        {
            var para = new SQLiteParameter(name, type, size);
            para.Direction = ParameterDirection.Output;
            return para;
        }
        #endregion

        #region Connection
        static SQLiteConnection CreateConnection()
        {
            var conn = new SQLiteConnection(ConnectionString);
            return conn;
        }
        static SQLiteCommand CreateCommand(string commandText, CommandType commandType, SQLiteConnection connection, params SQLiteParameter[] paramList)
        {
            var cmd = new SQLiteCommand(commandText, connection);
            cmd.CommandType = commandType;
            if (paramList != null)
            {
                foreach (var para in paramList)
                {
                    cmd.Parameters.Add(para);
                }
            }
            return cmd;
        }
        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(string commandText, params SQLiteParameter[] paramList)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = CreateCommand(commandText, CommandType.Text, conn, paramList))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Query
        /// <summary>
        /// 执行SQL语句,得到DataSet
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string commandText, params SQLiteParameter[] paramList)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = CreateCommand(commandText, CommandType.Text, conn, paramList))
                {
                    var ada = new SQLiteDataAdapter(cmd);
                    var ds = new DataSet();
                    ada.Fill(ds);
                    return ds;
                }
            }
        }
        /// <summary>
        /// 执行SQL语句,得到第一个DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string commandText, params SQLiteParameter[] paramList)
        {
            var ds = ExecuteDataSet(commandText, paramList);
            return ds.Tables[0];
        }
        /// <summary>
        /// 执行SQL语句,返回数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static List<T> ExecuteList<T>(string commandText, params SQLiteParameter[] paramList)
        {
            var dt = ExecuteDataTable(commandText, paramList);
            return CollectionHelper.ConvertTo<T>(dt);
        }
        /// <summary>
        /// 执行SQL语句,返回第一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="paraList"></param>
        /// <returns></returns>
        public static T ExecuteItem<T>(string commandText, params SQLiteParameter[] paraList)
        {
            var dt = ExecuteDataTable(commandText, paraList);
            if (dt.Rows.Count == 0) return default(T);
            var dr = dt.Rows[0];
            return CollectionHelper.CreateItem<T>(dr);
        }
        #endregion

        #region ExecuteScalar
        public static object ExecuteScalar(string commandText, params SQLiteParameter[] paramList)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = CreateCommand(commandText, CommandType.Text, conn, paramList))
                {
                    return cmd.ExecuteScalar();
                }
            }
        }
        #endregion
    }
}
