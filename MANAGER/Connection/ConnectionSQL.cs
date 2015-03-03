using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MANAGER.Properties;
using System.Data.SqlClient;

namespace MANAGER.Connection
{
    class ConnectionSql
    {
        private static SqlConnection Connection;
        private static Boolean ConnectionIsStarted;

        private ConnectionSql()
        {
            Connection = new SqlConnection(Settings.Default.DatabaseConnectionString);
            Connection.Open();
            ConnectionIsStarted = true;
        }

        public static SqlCommand SqlCommand(string query)
        {
            return new SqlCommand {Connection = GetConnection(), CommandText = query};
        }

        public static SqlCommand SqlCommandStored(string query)
        {
            return new SqlCommand {CommandType = CommandType.StoredProcedure, Connection = GetConnection(), CommandText = query};
        }

        private static SqlConnection GetConnection()
        {
            if(!ConnectionIsStarted)
            {
                new ConnectionSql();
            }
            return Connection;
        }
    }
}
