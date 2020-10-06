// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

namespace MANAGER.Classes.Table
{
    public class SQL_Estimate
    {
        static SQL_Estimate()
        {
            TableName = "DEVIS";
            Day = "JOUR";
            NumberDevis = "NUMERODEVIS";
            PriceMerchandise = "PRIXMARCHANDISE";
            Quantity = "QUANTITE";
            Client = "ID_CLIENT";
        }

        public static string TableName { get; set; }
        public static string Quantity { get; set; }
        public static string Day { get; set; }
        public static string PriceMerchandise { get; set; }
        public static string NumberDevis { get; set; }
        public static string Client { get; set; }

        public void Construction(string tableName, string day, string numberDevis, string priceMerchandise, string quantity)
        {
            TableName = tableName;
            Day = day;
            NumberDevis = numberDevis;
            PriceMerchandise = priceMerchandise;
            Quantity = quantity;
        }
    }
}