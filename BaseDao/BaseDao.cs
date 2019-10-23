using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using Tools;
namespace dao
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseDao
    {
        #region Version Info
        //=====================================================================
        // Project Name        :    BaseDao  
        // Project Description : 
        // Class Name          :    Class1
        // File Name           :    Class1
        // Namespace           :    BaseDao 
        // Class Version       :    v1.0.0.0
        // Class Description   : 
        // CLR                 :    4.0.30319.42000  
        // Author              :    董   魁  (ccie20079@126.com)
        // Addr                :    中国  陕西 咸阳    
        // Create Time         :    2019-10-22 14:57:19
        // Modifier:     
        // Update Time         :    2019-10-22 14:57:19
        //======================================================================
        // Copyright © DGCZ  2019 . All rights reserved.
        // =====================================================================
        #endregion
        //私有构造器
        private BaseDao()
        {
        }
        //单例模式
        private static BaseDao instance;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static BaseDao getBaseDao() {
            if (instance == null) {
                instance = new BaseDao();
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
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strSQL;
            using (SqlConnection conn = ConnHelper.getConn())
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
                    ConnHelper.closeConn(conn);
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
        public int ExecuteNonQuery(string procName, SqlParameter[] cmdParms)
        {
            int val = 0;
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = ConnHelper.getConn())
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
                    ConnHelper.closeConn(conn);
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
        public SqlDataReader ExecuteReader(string procName, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = ConnHelper.getConn();
            SqlDataReader dr = null;
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
                ConnHelper.closeConn(conn);
            }
            return dr;
        }
        /// <summary>
        /// 执行某个存储过程，不带参数？
        /// </summary>
        /// <param name="procName"></param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader ExecuteReader(string procName)
        {
            SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = ConnHelper.getConn();
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
                ConnHelper.closeConn(conn);
            }
            return dr;
        }
        /// <summary>
        /// 执行存储过程,带参数，返回DataSet
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string procName, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = ConnHelper.getConn();
            cmd.CommandTimeout = 0;
            DataSet ds = new DataSet();
            try
            {
                CreateCommand(cmd, conn, null, procName, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter();
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
                ConnHelper.closeConn(conn);
            }
            return ds;
        }
        /// <summary>
        /// 执行存储过程,带参数.返回DataTable
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string procName, SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = ConnHelper.getConn();
            cmd.CommandTimeout = 0;
            DataTable dt = new DataTable();
            try
            {
                CreateCommand(cmd, conn, null, procName, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter();
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
                ConnHelper.closeConn(conn);
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
            SqlConnection conn = ConnHelper.getConn();
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                ConnHelper.closeConn(conn);
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
            SqlConnection conn = ConnHelper.getConn();
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                ConnHelper.closeConn(conn);
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
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = ConnHelper.getConn();
            DataSet ds = new DataSet();
            try
            {
                CreateCommand(cmd, conn, null, procName, null);
                SqlDataAdapter da = new SqlDataAdapter();
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
                ConnHelper.closeConn(conn);
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
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = ConnHelper.getConn();
            DataTable dt = new DataTable();
            try
            {
                CreateCommand(cmd, conn, null, procName, null);
                SqlDataAdapter da = new SqlDataAdapter();
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
                ConnHelper.closeConn(conn);
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
        public void CreateCommand(SqlCommand cmd, SqlConnection con, SqlTransaction trans, string procName, SqlParameter[] cmdParms)
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
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}
