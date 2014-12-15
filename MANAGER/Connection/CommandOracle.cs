using MANAGER.Classes;
using MANAGER.Properties;

using Oracle.ManagedDataAccess.Client;

namespace MANAGER.Connection
{
    internal static class CommandOracle
    {
        public static void Insert(int idClient, Merchandise merchandise, int idEstimate, int numberEstimate, string date)
        {
            var Insert = ConnectionOracle.OracleCommandStored(ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString), "INSERTDEVIS");

            var paramIdClient = new OracleParameter(":1", OracleDbType.Int32) { Value = idClient };
            var paramIdMerchandise = new OracleParameter(":2", OracleDbType.Int32) { Value = merchandise.id };
            var paramIdEstimate = new OracleParameter(":3", OracleDbType.Int32) { Value = ((idEstimate) + 1) };
            var paramQTE = new OracleParameter(":4", OracleDbType.Int32) { Value = merchandise.quantite };
            var paramDate = new OracleParameter(":5", OracleDbType.Varchar2) { Value = date };
            var paramPrice = new OracleParameter(":6", OracleDbType.Varchar2) { Value = merchandise.prix };
            var paramNumEstimate = new OracleParameter(":7", OracleDbType.Varchar2) { Value = ((numberEstimate) + 1) };

            Insert.Parameters.Add(paramIdClient);
            Insert.Parameters.Add(paramIdMerchandise);
            Insert.Parameters.Add(paramIdEstimate);
            Insert.Parameters.Add(paramQTE);
            Insert.Parameters.Add(paramDate);
            Insert.Parameters.Add(paramPrice);
            Insert.Parameters.Add(paramNumEstimate);

            Insert.ExecuteNonQuery();
        }
    }
}
