// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

namespace MANAGER.Classes.Table
{
    public class SQL_Merchandise
    {
        static SQL_Merchandise()
        {
            TableName = "MARCHANDISE";
            ID = $"ID_{TableName}";
            Name = "NOM";
            OnSale = "ENVENTE";
            Price = "PRIX";
            Quantity = "QUANTITE";
        }

        public static string ID { get; set; }
        public static string TableName { get; set; }
        public static string Price { get; set; }
        public static string Name { get; set; }
        public static string Quantity { get; set; }
        public static string OnSale { get; set; }

        public void Construction(string tableName, string name, string onSale, string price, string quantity)
        {
            TableName = tableName;
            ID = $"ID_{tableName}";
            Name = name;
            OnSale = onSale;
            Price = price;
            Quantity = quantity;
        }
    }
}