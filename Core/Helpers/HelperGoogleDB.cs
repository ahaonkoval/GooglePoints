using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public class HelperGoogleDB
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetRestReuest()
        {
            int limit = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["DbConnectString"]))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"SELECT isnull(sum([limit]), -1) limit FROM [dbo].[google_request_limit] a 
                    WHERE a.[date_request] = cast(getdate() as date)";
                cmd.CommandType = CommandType.Text;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    limit = Convert.ToInt32(reader[0]);
                }
                connection.Close();
            }
            return limit;
        }
    }
}
