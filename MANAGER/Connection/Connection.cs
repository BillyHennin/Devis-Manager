﻿// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

//using System.Collections.Specialized;

//using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace MANAGER.Connection
{
    public class Connection
    {
        public static string Database { private get; set; }

        public static IDbCommand Command(string query)
        {
            IDbCommand command;
            switch(Database)
            {
                case ("Oracle"):
                    command = ConnectionOracle.Command(query);
                    break;
                case ("SqlServer"):
                    command = ConnectionSqlServer.Command(query);
                    break;
                case ("MySql"):
                    command = ConnectionMySql.Command(query);
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
                case ("Oracle"):
                    command = ConnectionOracle.CommandStored(query);
                    break;
                case ("SqlServer"):
                    command = ConnectionSqlServer.CommandStored(query);
                    break;
                case ("MySql"):
                    command = ConnectionMySql.CommandStored(query);
                    break;
                default:
                    command = ConnectionOracle.CommandStored(query);
                    break;
            }
            return command;
        }

        public static IDbCommand GetAll(string tableQuery)
        {
            return Command(String.Format("SELECT * FROM {0}", tableQuery));
        }

        public static void Insert(string tableQuery, params Object[] value)
        {
            var query = String.Empty;
            foreach(var field in value)
            {
                query += "'" + field + "',";
            }
            query = query.Substring(0, query.Length - 1);
            var queryInsert = String.Format("INSERT INTO {0} VALUES ({1})", tableQuery, query);
            var Command = Connection.Command(queryInsert);
            Command.ExecuteNonQuery();
        }

        public static void Delete(string tableQuery, object ID, string param = null)
        {
            var Id_Table = String.Empty;
            Id_Table = param ?? tableQuery;
            var query = String.Format("DELETE FROM {0} WHERE ID_{1} = {2}", tableQuery, Id_Table, ID);
            var Command = Connection.Command(query);
            Command.ExecuteNonQuery();
        }

        public static void Update(string tableQuery, int ID, String[,] value)
        {
            var set = String.Empty;
            for(var i = 0; i < value.Length / 2; i++)
            {
                set += String.Format("{0}='{1}' ,", value[i, 0], value[i, 1]);
            }
            set = set.Substring(0, set.Length - 1);
            var query = String.Format("UPDATE {0} SET {2} WHERE ID_{0} = {1}", tableQuery, ID, set);
            var Command = Connection.Command(query);
            Command.ExecuteNonQuery();
        }

        //Make this works one day.
        /*
        public static void parameterAdd(string param, string type, Object value, IDbCommand command)
        {
            switch (Database)
            {
                case ("Oracle"):
                    command.Parameters.Add(new OracleParameter(param, OracleDbType.Int32) { Value = value });
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
            return (OracleDbType) Enum.Parse(typeof(T), value, true);
        }

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
        }*/
    }
}