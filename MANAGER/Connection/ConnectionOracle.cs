// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

using System;
using System.Data;

using Oracle.ManagedDataAccess.Client;

#endregion

namespace MANAGER.Connection
{
    internal static class ConnectionOracle
    {
        public static OracleConnection OracleDatabase(String DatabaseConnectionString)
        {
            return new OracleConnection(DatabaseConnectionString);
        }

        public static OracleCommand OracleCommand(OracleConnection db, string query)
        {
            return new OracleCommand {Connection = db, CommandText = query};
        }

        public static OracleCommand OracleCommandStored(OracleConnection db, string query)
        {
            return new OracleCommand {CommandType = CommandType.StoredProcedure, Connection = db, CommandText = query};
        }
    }
}