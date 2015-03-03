// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Data;
using System.Data.SqlClient;
using MANAGER.Properties;

namespace MANAGER.Connection
{
    internal class ConnectionSql
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
            return new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                Connection = GetConnection(),
                CommandText = query
            };
        }

        private static SqlConnection GetConnection()
        {
            if (!ConnectionIsStarted)
            {
                new ConnectionSql();
            }
            return Connection;
        }
    }
}