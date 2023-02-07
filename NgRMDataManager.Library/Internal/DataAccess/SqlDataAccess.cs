using Dapper;
using Microsoft.Extensions.Configuration;
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
    internal class SqlDataAccess : IDisposable
    {
        private readonly IConfiguration _config;
        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }
        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
            
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
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public void StartTransaction(string connectionStringName)
        {
            string connString = GetConnectionString(connectionStringName);
            _connection = new SqlConnection(connString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            isClosed = false;
        }

        private bool isClosed = false;
        

        public void CommitTransaction()
        {

            _transaction?.Commit();
            _connection?.Close();
            isClosed = true;
        }
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
            isClosed = true;
        }

        public void Dispose()
        {
            if(isClosed == false)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {

                    // TODO - log this issue
                }
            }
            _transaction = null;
            _connection = null;
            
        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            
                List<T> rows = _connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure,transaction:_transaction).ToList();

                return rows;
            
        }
    }
}
