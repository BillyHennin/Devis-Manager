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
        //From sql : 0,1,3,2,5
        public Merchandise(int id, string name, int quantity, double price, int categoryId)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Price = price;
            CategoryId = categoryId;
        }

        public Border Border { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public bool OnSale { get; set; }

        public override string ToString()
        {
            return Id.ToString(CultureInfo.InvariantCulture);
        }
    }
}