﻿// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System.Data;
using MySql.Data.MySqlClient;

namespace MANAGER.Classes.Connection
{
    public class ConnectionMySql : Classes.Connection.Connection
    {
        private static MySqlConnection _connection;
        private static bool _connectionIsStarted;

        private ConnectionMySql()
        {
            _connection = new MySqlConnection(Properties.Connection.Default.DatabaseConnectionString);
            _connection.Open();
            _connectionIsStarted = true;
        }

        private static MySqlConnection GetConnection()
        {
            if(!_connectionIsStarted)
            {
                new ConnectionMySql();
            }
            return _connection;
        }

        public new static IDbCommand Command(string query) => new MySqlCommand {Connection = GetConnection(), CommandText = query};

        public new static IDbCommand CommandStored(string query) => new MySqlCommand {CommandType = CommandType.StoredProcedure, Connection = GetConnection(), CommandText = query};
    }
}