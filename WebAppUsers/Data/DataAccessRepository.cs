using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebAppUsers.Data
{
    /// <summary>
    /// Class to access to the data base
    /// </summary>
    public abstract class DataAccessRepository
    {
        private string _connString = ConfigurationManager.ConnectionStrings["UsersAppBD"].ConnectionString;

        /// <summary>
        /// Used to notice that the operation does not affected any row
        /// </summary>
        /// <param name="rowsAffected"></param>
        /// <returns></returns>
        protected string ResponeDB(int rowsAffected)
        {
            return rowsAffected > 0 ? "" : "Ningun elemento fue afectado";
        }

        /// <summary>
        /// Executes a stored procedure to retrieve data
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType">by default is stored procedure could by a text command</param>
        /// <returns></returns>
        public DataTable SpGetDataFromBD(
            string query,
            List<KeyValuePair<string, string>> parameters = null,
            CommandType commandType = CommandType.StoredProcedure
        )
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

                    cmd.CommandType = commandType;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(result);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Executes a stored procedure to affect the database 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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