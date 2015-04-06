// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;

namespace MANAGER.Table
{
    public class Merchandise
    {
        public static string ID { get; set; }
        public static string TableName { get; set; }
        public static string Price { get; set; }
        public static string Name { get; set; }
        public static string Quantity { get; set; }
        public static string OnSale { get; set; }

        static Merchandise()
        {
            TableName = "MARCHANDISE";
            ID = String.Format("ID_{0}", TableName);
            Name = "NOM";
            OnSale = "ENVENTE";
            Price = "PRIX";
            Quantity = "QUANTITE";
        }

        public void Construction(string TableName, string Name, string OnSale, string Price, string Quantity)
        {
            Merchandise.TableName = TableName;
            Merchandise.ID = String.Format("ID_{0}", Merchandise.TableName);
            Merchandise.Name = Name;
            Merchandise.OnSale = OnSale;
            Merchandise.Price = Price;
            Merchandise.Quantity = Quantity;
        }
    }
}