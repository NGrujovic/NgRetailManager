using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgRMDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        //Load data function for getting data with need to pass in parameter(Example: Getting item with specific id)
        public List<T> LoadData<T,U>(string storedProcedure,U parameters, string connectionStringName)
        {
            string connString = GetConnectionString(connectionStringName);

            using (IDbConnection cnn = new SqlConnection(connString))
            {
                List<T> rows = cnn.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        //Load data function for getting data without need to pass parameter(Example: Getting All data from one table without condition)
        public List<T> LoadData<T>(string storedProcedure, string connectionStringName)
        {
            string connString = GetConnectionString(connectionStringName);

            using (IDbConnection cnn = new SqlConnection(connString))
            {
                List<T> rows = cnn.Query<T>(storedProcedure, commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connString = GetConnectionString(connectionStringName);

            using (IDbConnection cnn = new SqlConnection(connString))
            {
                cnn.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                
            }
        }


    }
}
