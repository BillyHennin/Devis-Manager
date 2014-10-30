// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

namespace SLAM3.Classes
{
    internal class Produit
    {
        private readonly string _nom;
        private readonly double _prix;

        public Produit(double prix, string nom)
        {
            _prix = prix;
            _nom = nom;
        }

        public string GetNom
        {
            get { return _nom; }
        }

        public double GetPrix
        {
            get { return _prix; }
        }
    }
}