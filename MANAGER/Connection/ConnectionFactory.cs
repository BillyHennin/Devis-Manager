using System.Data;

namespace MANAGER.Connection
{
    public abstract class ConnectionFactory
    {
        public string connectionString = Properties.Connection.Default.DatabaseConnectionString;

        #region Abstract Functions

        public abstract IDbConnection CreateConnection();
        public abstract IDbCommand CreateCommand();
        public abstract IDbConnection CreateOpenConnection();
        public abstract IDbCommand CreateCommand(string commandText, IDbConnection connection);
        public abstract IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection);
        public abstract IDataParameter CreateParameter(string parameterName, object parameterValue);

        #endregion
        
    }
}
