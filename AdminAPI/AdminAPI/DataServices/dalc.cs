using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using AdminAPI.Utility;

namespace AdminAPI.DataServices
{
    public class dalc
    {
        SqlConnection conn = new SqlConnection();
        // SqlConnection conn;

        public dalc()
        {
            conn.ConnectionString = @"Data Source=DESKTOP-OK7SFIP;Initial Catalog=AdminDemo;Integrated Security=True;";
            // conn.ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        }


        public async Task<List<T>> selectbyqueryList<T>(string str)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = str.ToString();
            SqlDataReader dr;
            try
            {
                await conn.OpenAsync();
                dr = await cmd.ExecuteReaderAsync();
                List<T> lst = new List<T>();
                while (await dr.ReadAsync())
                {
                    T item = Common.GetItem<T>(dr);
                    lst.Add(item);
                }
                return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                // conn.Dispose();
            }
        }




        public async Task<List<T>> GetList<T>(string Query, SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(para);
            cmd.CommandText = Query.ToString();
            SqlDataReader dr;
            try
            {
                await conn.OpenAsync();
                dr = await cmd.ExecuteReaderAsync();
                List<T> lst = new List<T>();
                while (await dr.ReadAsync())
                {
                    T item = Common.GetItem<T>(dr);
                    lst.Add(item);
                }
                return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                // conn.Dispose();
            }
        }

        public DataTable GetDataTable_Text(string Query, SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(para);
            cmd.CommandText = Query.ToString();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                conn.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                // conn.Dispose();
            }
        }

        public async Task<List<T>> GetList_Text<T>(string Query, SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(para);
            cmd.CommandText = Query.ToString();
            SqlDataReader dr;
            try
            {
                await conn.OpenAsync();
                dr = await cmd.ExecuteReaderAsync();
                List<T> lst = new List<T>();
                if (dr.HasRows)
                {
                    while (await dr.ReadAsync())
                    {
                        T item = Common.GetItem<T>(dr);
                        lst.Add(item);
                    }
                }
                return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                // conn.Dispose();
            }
        }

        public async Task<List<Dictionary<string, object>>> GetQryData(string qry)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = qry;
            SqlDataReader dr;
            try
            {
                await conn.OpenAsync();
                dr = await cmd.ExecuteReaderAsync();
                // conn.Close();
                List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();
                while (await dr.ReadAsync())
                {
                    Dictionary<string, object> obj = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        obj[dr.GetName(i)] = dr[i].GetType().Name == "DBNull" ? null : dr[i];
                    }
                    lst.Add(obj);
                }
                return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }

    }
}
