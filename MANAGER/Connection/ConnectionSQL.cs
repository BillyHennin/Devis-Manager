// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Data;
using System.Data.SqlClient;

namespace MANAGER.Connection
{
    public class ConnectionSql : Connection
    {
        private static SqlConnection Connection;
        private static Boolean ConnectionIsStarted;

        private ConnectionSql()
        {
            Connection = new SqlConnection(Properties.Connection.Default.DatabaseConnectionString);
            Connection.Open();
            ConnectionIsStarted = true;
        }

        private static SqlConnection GetConnection()
        {
            if(!ConnectionIsStarted)
            {
                new ConnectionSql();
            }
            return Connection;
        }

        public static IDbCommand Command(string query)
        {
            return new SqlCommand {Connection = GetConnection(), CommandText = query};
        }

        public static IDbCommand CommandStored(string query)
        {
            return new SqlCommand {CommandType = CommandType.StoredProcedure, Connection = GetConnection(), CommandText = query};
        }
    }
}