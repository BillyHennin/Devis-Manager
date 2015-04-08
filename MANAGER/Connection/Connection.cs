// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Data;
using System.Linq;

namespace MANAGER.Connection
{
    public class Connection
    {
        public static String Database { private get; set; }

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
            command.Prepare();
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
            command.Prepare();
            return command;
        }

        public static IDbCommand GetAll(string tableQuery)
        {
            return Command(String.Format("SELECT * FROM {0}", tableQuery));
        }

        public static Object GetUniqueCell(string query)
        {
            var command = Command(query);
            return command.ExecuteScalar();
        }

        public static void Insert(string tableQuery, params Object[] value)
        {
            var query = value.Aggregate(String.Empty, (current, field) => current + ("'" + field + "',"));
            query = query.Substring(0, query.Length - 1);
            var queryInsert = String.Format("INSERT INTO {0} VALUES ({1})", tableQuery, query);
            var Command = Connection.Command(queryInsert);
            Command.ExecuteNonQuery();
        }

        public static void Delete(string tableQuery, object ID, string param = null)
        {
            var Id_Table = param ?? tableQuery;
            var query = String.Format("DELETE FROM {0} WHERE ID_{1} = {2}", tableQuery, Id_Table, ID);
            var Command = Connection.Command(query);
            Command.ExecuteNonQuery();
        }

        public static void Update(string tableQuery, int ID, String[,] value)
        {
            var query = String.Empty;
            var size = value.Length / 2;
            for(var i = 0; i < size; i++)
            {
                query += String.Format("{0} = '{1}' ,", value[i, 0], value[i, 1]);
            }
            query = String.Format("UPDATE {0} SET {2} WHERE ID_{0} = {1}", tableQuery, ID, query.Substring(0, query.Length - 1));
            var Command = Connection.Command(query);
            Command.ExecuteNonQuery();
        }
    }
}