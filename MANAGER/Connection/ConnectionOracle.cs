// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Data;

using Oracle.ManagedDataAccess.Client;

namespace MANAGER.Connection
{
    public class ConnectionOracle : Connection
    {
        private static OracleConnection Connection;
        private static Boolean ConnectionIsStarted;

        private ConnectionOracle()
        {
            Connection = new OracleConnection(Properties.Connection.Default.DatabaseConnectionString);
            Connection.Open();
            ConnectionIsStarted = true;
        }

        private static OracleConnection GetConnection()
        {
            if(!ConnectionIsStarted)
            {
                new ConnectionOracle();
            }
            return Connection;
        }

        public static IDbCommand Command(string query)
        {
            return new OracleCommand {Connection = GetConnection(), CommandText = query};
        }

        public static IDbCommand CommandStored(string query)
        {
            return new OracleCommand {CommandType = CommandType.StoredProcedure, Connection = GetConnection(), CommandText = query};
        }
    }
}