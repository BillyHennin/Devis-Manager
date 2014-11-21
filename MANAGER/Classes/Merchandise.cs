// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System.Globalization;
using System.Windows.Controls;

namespace MANAGER.Classes
{
    public class Merchandise
    {
        private readonly int id;
        private readonly string nom;
        private readonly double prix;
        private readonly int quantite;

        public Merchandise(int id, string nom, int quantite, double prix)
        {
            this.id = id;
            this.nom = nom;
            this.quantite = quantite;
            this.prix = prix;
        }

        public Border Border { get; set; }

        public string GetNom { get { return nom; } }

        public double GetPrix { get { return prix; } }

        public int GetQte { get { return quantite; } }

        public int GetId { get { return id; } }

        public override string ToString()
        {
            return GetId.ToString(CultureInfo.InvariantCulture);
        }
    }
}