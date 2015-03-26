using System;
using System.Data;
using System.Windows;

using Oracle.ManagedDataAccess.Client;

namespace MANAGER.Connection
{
    public class Connection
    {
        public static string Database { private get; set; }

        private enum OracleDbType
        {
            // ReSharper disable UnusedMember.Local
            Bfile,
            Blob,
            Byte,
            Char,
            Clob,
            Date,
            Decimal,
            Double,
            Int16,
            Int32,
            Int64,
            IntevalDS,
            IntervalYM,
            Long, 
            Longraw,
            Nchar,
            Nclob,
            NVarchar2,
            raw,
            RefCursor,
            Single,
            TimeStamp,
            TimeStampLTZ,
            TimeStampTZ,
            Varchar2,
            XmlType
        }

        public static IDbCommand Command(string query)
        {
            IDbCommand command;
            switch(Database)
            {
                case("Oracle"):
                    command = ConnectionOracle.Command(query);
                    break;
                case("SQL"):
                    command = ConnectionSql.Command(query);
                    break;
                default:
                    command = ConnectionOracle.Command(query);
                    break;
            }
            return command;
        }

        public static IDbCommand CommandStored(string query)
        {
            IDbCommand command;
            switch(Database)
            {
                case("Oracle"):
                    command = ConnectionOracle.CommandStored(query);
                    break;
                case("SQL"):
                    command = ConnectionSql.CommandStored(query);
                    break;
                default:
                    command = ConnectionOracle.CommandStored(query);
                    break;
            }
            return command;
        }

        public static IDbCommand GetAll(string table)
        {
            return Command(String.Format("SELECT * FROM {0}", table));
        }

        public static void parameterAdd(string param, string type, string value, IDbCommand command)
        {
            switch (Database)
            {
                case ("Oracle"):
                    command.Parameters.Add(new OracleParameter(param, ParseEnum<OracleDbType>(type)) { Value = value });
                    break;
                case ("SQL"):
                    //command = ConnectionSql.SqlCommand(query);
                    break;
                default:
                    command.Parameters.Add(new OracleParameter(param, ParseEnum<OracleDbType>(type)) { Value = value });
                    break;
            }
        }

        private static OracleDbType ParseEnum<T>(string value)
        {
            return (OracleDbType)Enum.Parse(typeof(T), value, true);
        }
    }
}
