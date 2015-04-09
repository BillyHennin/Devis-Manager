// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;

namespace MANAGER.Table
{
    public class Customer
    {
        public static string ID { get; set; }
        public static string TableName { get; set; }
        public static string Email { get; set; }
        public static string Name { get; set; }
        public static string Phone { get; set; }

        static Customer()
        {
            TableName = "CLIENT";
            ID = String.Format("ID_{0}", TableName);
            Email = "EMAIL";
            Name = "DENOMINATION";
            Phone = "TELEPHONE";
        }

        public void Construction(string TableName, string Email, string Name, string Phone)
        {
            Customer.TableName = TableName;
            ID = String.Format("ID_{0}", TableName);
            Customer.Email = Email;
            Customer.Name = Name;
            Customer.Phone = Phone;
        }
    }
}