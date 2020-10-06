using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MANAGER.Classes.Connection
{
    public class ConnectionLocal : Classes.Connection.Connection
    {
        private static SqlConnection _connection;
        private static bool _connectionIsStarted;

        private ConnectionLocal()
        {
            _connection = new SqlConnection(Properties.Connection.Default.DatabaseConnectionString);
            _connection.Open();
            _connectionIsStarted = true;
        }

        private static SqlConnection GetConnection()
        {
            if (!_connectionIsStarted)
            {
                new ConnectionLocal();
            }
            return _connection;
        }

        public new static IDbCommand Command(string query) => new SqlCommand { Connection = GetConnection(), CommandText = query };

        public new static IDbCommand CommandStored(string query) => new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = GetConnection(), CommandText = query };

        public new static IDbCommand CommandStored(string query, Dictionary<string, object> param)
        {
           var cmd = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = GetConnection(), CommandText = query };
           foreach (var parameter in param)
           {
               cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
           }
           return cmd;
        }
    }
}