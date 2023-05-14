using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebAppUsers.Data
{
    public abstract class DataAccessRepository
    {
        private string _connString = ConfigurationManager.ConnectionStrings["UsersConnectionString"].ConnectionString;

        protected string ResponeDB(int rowsAffected)
        {
            return rowsAffected > 0 ? "Ningun elemento fue afectado" : "";
        }

        public DataTable SpGetDataFromBD(string query, List<KeyValuePair<string, string>> parameters = null)
        {
            DataTable result = new DataTable();
            parameters = parameters ?? new List<KeyValuePair<string, string>>();

            using (SqlConnection cnn = new SqlConnection(_connString))
            {
                cnn.Open();

                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    foreach (KeyValuePair<string, string> param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        da.Fill(result);
                    }
                }
            }

            return result;
        }

        public int SpSaveDataToBD(string query, List<KeyValuePair<string, string>> parameters)
        {
            int result = 0;

            using (SqlConnection cnn = new SqlConnection(_connString))
            {
                cnn.Open();

                using (SqlCommand cmd = new SqlCommand(query, cnn))
                {
                    foreach (KeyValuePair<string, string> param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    cmd.CommandType = CommandType.StoredProcedure;

                    result = cmd.ExecuteNonQuery();
                }
            }

            return result;
        }
    }
}