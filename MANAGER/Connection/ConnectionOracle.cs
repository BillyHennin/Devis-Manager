// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using Oracle.DataAccess.Client; 

namespace MANAGER.Connection
{
    internal class ConnectionOracle
    {
        public OracleConnection OracleDatabase(String DatabaseConnectionString)

        {
            return new OracleConnection(DatabaseConnectionString);
        }

        public OracleCommand OracleCommand(OracleConnection db, string query)
        {
            return new OracleCommand {Connection = db, CommandText = query};
        }
    }
}