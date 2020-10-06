// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;

namespace MANAGER.Table
{
    public class SQL_Customer
    {
        static SQL_Customer()
        {
            TableName = "CLIENT";
            ID = $"ID_{TableName}";
            Email = "EMAIL";
            Name = "DENOMINATION";
            Phone = "TELEPHONE";
        }

        public static string ID { get; set; }
        public static string TableName { get; set; }
        public static string Email { get; set; }
        public static string Name { get; set; }
        public static string Phone { get; set; }

        public void Construction(string tableName, string email, string name, string phone)
        {
            TableName = tableName;
            ID = $"ID_{tableName}";
            Email = email;
            Name = name;
            Phone = phone;
        }
    }
}