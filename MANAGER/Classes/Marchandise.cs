// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System.Windows.Controls;

namespace MANAGER.Classes
{
    public class Marchandise
    {
        private readonly string _nom;
        private readonly double _prix;
        private readonly int _quantite;

        public Marchandise(string nom, int quantite, double prix)
        {
            _nom = nom;
            _quantite = quantite;
            _prix = prix;
        }

        public Border Bordure { get; set; }

        public string GetNom { get { return _nom; } }

        public double GetPrix { get { return _prix; } }

        public int GetQte { get { return _quantite; } }

        public double SetBordureWidth { set { Bordure.Width = value; } }
    }
}