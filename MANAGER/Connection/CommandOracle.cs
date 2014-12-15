using MANAGER.Classes;
using MANAGER.Properties;

using Oracle.ManagedDataAccess.Client;

namespace MANAGER.Connection
{
    internal static class CommandOracle
    {
        public static void Insert(int idClient, Merchandise merchandise, int idEstimate, int numberEstimate, string date, int i)
        {
            var Insert = ConnectionOracle.OracleCommandStored(ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString), "INSERTDEVIS");

            Insert.Parameters.Add(new OracleParameter(":1", OracleDbType.Int32) { Value = idClient });
            Insert.Parameters.Add(new OracleParameter(":2", OracleDbType.Int32) { Value = merchandise.id });
            Insert.Parameters.Add(new OracleParameter(":3", OracleDbType.Int32) { Value = ((idEstimate) + i + 1) });
            Insert.Parameters.Add(new OracleParameter(":4", OracleDbType.Int32) { Value = merchandise.quantite });
            Insert.Parameters.Add(new OracleParameter(":5", OracleDbType.Varchar2) { Value = date });
            Insert.Parameters.Add(new OracleParameter(":6", OracleDbType.Varchar2) { Value = merchandise.prix });
            Insert.Parameters.Add(new OracleParameter(":7", OracleDbType.Varchar2) { Value = ((numberEstimate) + 1) });

            Insert.ExecuteNonQuery();
        }
    }
}
