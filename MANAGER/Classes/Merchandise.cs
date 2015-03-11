// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

using System.Globalization;
using System.Windows.Controls;

#endregion

namespace MANAGER.Classes
{
    public class Merchandise
    {
        public Merchandise(int id, string nom, int quantite, double price, int categoryID)
        {
            this.id = id;
            this.nom = nom;
            this.quantite = quantite;
            this.price = price;
            this.categoryID = categoryID;
        }

        public Border Border { get; set; }
        public string nom { get; set; }
        public double price { get; set; }
        public int quantite { get; set; }
        public int id { get; set; }
        public int categoryID { get; set; }

        public override string ToString()
        {
            return id.ToString(CultureInfo.InvariantCulture);
        }
    }
}