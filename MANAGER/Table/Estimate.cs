// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

namespace MANAGER.Table
{
    public class Estimate
    {
        static Estimate()
        {
            TableName = "DEVIS";
            Day = "JOUR";
            NumberDevis = "NUMERODEVIS";
            PriceMerchandise = "PRIXMARCHANDISE";
            Quantity = "QUANTITE";
        }

        public static string TableName { get; set; }
        public static string Quantity { get; set; }
        public static string Day { get; set; }
        public static string PriceMerchandise { get; set; }
        public static string NumberDevis { get; set; }

        public void Construction(string TableName, string Day, string NumberDevis, string PriceMerchandise, string Quantity)
        {
            Estimate.TableName = TableName;
            Estimate.Day = Day;
            Estimate.NumberDevis = NumberDevis;
            Estimate.PriceMerchandise = PriceMerchandise;
            Estimate.Quantity = Quantity;
        }
    }
}