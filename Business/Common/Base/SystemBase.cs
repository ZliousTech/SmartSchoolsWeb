using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Common.Base
{
    public class SystemBase
    {
        public SystemBase() 
        { 

        }
        
        public static DataTable GetDataTble(string cmdStr)
        {
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (Conn.State == ConnectionState.Closed) Conn.Open();
            SqlCommand cmd = new SqlCommand(cmdStr, Conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            da.Fill(ds);
            dt = ds.Tables[0];
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
                cmd.Cancel();
                da.Dispose();
                ds.Dispose();
            }
            return dt;
        }
    }
}
