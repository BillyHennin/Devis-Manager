// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Data.SqlServerCe;

namespace MANAGER.Connection
{
    internal class ConnectionLocale
    {
        public SqlCeConnection LocalDatabase(String DatabaseConnectionString)
        {
            return new SqlCeConnection(DatabaseConnectionString);
        }

        public SqlCeCommand LocalCommand(SqlCeConnection db, string query)
        {
            return new SqlCeCommand {Connection = db, CommandText = query};
        }
    }
}