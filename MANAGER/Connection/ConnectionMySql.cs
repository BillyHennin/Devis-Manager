// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Data;

using MySql.Data.MySqlClient;

namespace MANAGER.Connection
{
    public class ConnectionMySql : Connection
    {
        private static MySqlConnection Connection;
        private static Boolean ConnectionIsStarted;

        private ConnectionMySql()
        {
            Connection = new MySqlConnection(Properties.Connection.Default.DatabaseConnectionString);
            Connection.Open();
            ConnectionIsStarted = true;
        }

        private static MySqlConnection GetConnection()
        {
            if(!ConnectionIsStarted)
            {
                new ConnectionMySql();
            }
            return Connection;
        }

        public new static IDbCommand Command(string query)
        {
            return new MySqlCommand {Connection = GetConnection(), CommandText = query};
        }

        public new static IDbCommand CommandStored(string query)
        {
            return new MySqlCommand {CommandType = CommandType.StoredProcedure, Connection = GetConnection(), CommandText = query};
        }
    }
}