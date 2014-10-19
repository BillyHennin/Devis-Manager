// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

namespace SLAM3.Classes
{
    internal class Produit
    {
        private readonly string nom;
        private readonly double prix;

        public Produit(double prix, string nom)
        {
            this.prix = prix;
            this.nom = nom;
        }

        public string getNom { get { return nom; } }
        public double getPrix { get { return prix; } }
    }
}