using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using Tools;
namespace dao
{
    public class MySqlBaseDao
    {
             //私有构造器
        private MySqlBaseDao()
        {
        }
        //单例模式
        private static MySqlBaseDao instance;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MySqlBaseDao getBaseDao()
        {
            if (instance == null) {
                instance = new MySqlBaseDao();
            }
            return instance;
        }      
        /// <summary>
        /// SQL语句执行方法。
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string strSQL)
        {
            int val = 0;
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strSQL;
            using (MySqlConnection conn = MySqlConnHelper.getConn())
            {
                cmd.Connection = conn;
                try
                {
                    val = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally {
                    cmd.Dispose();
                    MySqlConnHelper.closeConn(conn);
                }
                return val;
            }
        }
        /// <summary>
        /// 存储过程执行方法,传入参数集合。
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string procName, MySqlParameter[] cmdParms)
        {
            int val = 0;
            MySqlCommand cmd = new MySqlCommand();
            using (MySqlConnection conn = MySqlConnHelper.getConn())
            {
                
                CreateCommand(cmd, conn, null, procName, cmdParms);
                try {
                    val = cmd.ExecuteNonQuery();
                }catch(Exception ex){
                    MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally{
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    MySqlConnHelper.closeConn(conn);
                }
                return val;
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public MySqlDataReader ExecuteReader(string procName, MySqlParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = MySqlConnHelper.getConn();
            MySqlDataReader dr = null;
            try
            {
                CreateCommand(cmd, conn, null, procName, cmdParms);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                cmd.Dispose();
                MySqlConnHelper.closeConn(conn);
            }
            return dr;
        }
        /// <summary>
        /// 执行某个存储过程，不带参数？
        /// </summary>
        /// <param name="procName"></param>
        /// <returns>SqlDataReader</returns>
        public MySqlDataReader ExecuteReader(string procName)
        {
            MySqlDataReader dr = null;
            MySqlConnection conn = MySqlConnHelper.getConn();
            MySqlCommand cmd = new MySqlCommand();            
            try
            {
                CreateCommand(cmd, conn, null, procName, null);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示:", MessageBoxButtons.OK, MessageBoxIcon.Information);          
            }
            finally
            {
                cmd.Dispose();
                MySqlConnHelper.closeConn(conn);
            }
            return dr;
        }
        /// <summary>
        /// 执行存储过程,带参数，返回DataSet
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string procName, MySqlParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = MySqlConnHelper.getConn();
            cmd.CommandTimeout = 0;
            DataSet ds = new DataSet();
            try
            {
                CreateCommand(cmd, conn, null, procName, cmdParms);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                cmd.Dispose();
                MySqlConnHelper.closeConn(conn);
            }
            return ds;
        }
        /// <summary>
        /// 执行存储过程,带参数.返回DataTable
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string procName, MySqlParameter[] cmdParms)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = MySqlConnHelper.getConn();
            cmd.CommandTimeout = 0;
            DataTable dt = new DataTable();
            try
            {
                CreateCommand(cmd, conn, null, procName, cmdParms);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                cmd.Dispose();
                MySqlConnHelper.closeConn(conn);
            }
            return dt;
        }
        /// <summary>
        /// 执行SQL语句,返回DataTable
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string strSQL)
        {
            MySqlConnection conn = MySqlConnHelper.getConn();
            DataTable dt = new DataTable();
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(strSQL, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                MySqlConnHelper.closeConn(conn);
            }
            return dt;
        }
        /// <summary>
        /// 执行SQL语句，返回DataSet
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string strSQL)
        {
            MySqlConnection conn = MySqlConnHelper.getConn();
            DataSet ds = new DataSet();
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(strSQL, conn);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                MySqlConnHelper.closeConn(conn);
            }
            return ds;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSetByProcName(string procName)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = MySqlConnHelper.getConn();
            DataSet ds = new DataSet();
            try
            {
                CreateCommand(cmd, conn, null, procName, null);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                cmd.Dispose();
                MySqlConnHelper.closeConn(conn);
            }
            return ds;
        }
        /// <summary>
        /// 执行存储过程,返回DataTable
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTableByProcName(string procName)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = MySqlConnHelper.getConn();
            DataTable dt = new DataTable();
            try
            {
                CreateCommand(cmd, conn, null, procName, null);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                cmd.Dispose();
                MySqlConnHelper.closeConn(conn);
            }
            return dt;
        }
        /// <summary>
        /// 带事物处理的command
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="con"></param>
        /// <param name="trans"></param>
        /// <param name="procName"></param>
        /// <param name="cmdParms"></param>
        public void CreateCommand(MySqlCommand cmd, MySqlConnection con, MySqlTransaction trans, string procName, MySqlParameter[] cmdParms)
        {
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = procName;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            cmd.CommandType = CommandType.StoredProcedure;

            if (cmdParms != null)
            {
                foreach (MySqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}
