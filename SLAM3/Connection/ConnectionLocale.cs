using System;
using System.Data.SqlServerCe;

namespace SLAM3.Connection
{
    class ConnectionLocale
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
