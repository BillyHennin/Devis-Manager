// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System.Data;

using Oracle.ManagedDataAccess.Client;

namespace MANAGER.Connection
{
    public class ConnectionOracle : ConnectionFactory
    {
        public override IDbConnection CreateConnection()
        {
            return new OracleConnection(connectionString);
        }

        public override IDbCommand CreateCommand()
        {
            return new OracleCommand();
        }

        public override IDbConnection CreateOpenConnection()
        {
            var connection = (OracleConnection)CreateConnection();
            connection.Open();

            return connection;
        }

        public override IDbCommand CreateCommand(string commandText, IDbConnection connection)
        {
            var command = (OracleCommand)CreateCommand();

            command.CommandText = commandText;
            command.Connection = (OracleConnection)connection;
            command.CommandType = CommandType.Text;

            return command;
        }

        public override IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection)
        {
            var command = (OracleCommand)CreateCommand();

            command.CommandText = procName;
            command.Connection = (OracleConnection)connection;
            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        public override IDataParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new OracleParameter(parameterName, parameterValue);
        }
    }
}