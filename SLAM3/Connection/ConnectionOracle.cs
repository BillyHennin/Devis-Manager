using System;
using System.Data.OracleClient;

#pragma warning disable 618

namespace SLAM3.Connection
{
    class ConnectionOracle
    {
        public OracleConnection OracleDatabase(String DatabaseConnectionString)

        {
            return new OracleConnection(DatabaseConnectionString);
        }

        public OracleCommand OracleCommand(OracleConnection db, string query)
        {
            return new OracleCommand { Connection = db, CommandText = query };
        }
    }
}