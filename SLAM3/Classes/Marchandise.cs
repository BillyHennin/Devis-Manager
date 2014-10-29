// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System.Windows.Controls;

namespace SLAM3.Classes
{
    public class Marchandise
    {
        private readonly string nom;
        private readonly double prix;
        private readonly int quantite;

        public Marchandise(string nom, int quantite, double prix)
        {
            this.nom = nom;
            this.quantite = quantite;
            this.prix = prix;
        }

        public Border Bordure { get; set; }

        public string getNom
        {
            get { return nom; }
        }

        public double getPrix
        {
            get { return prix; }
        }

        public double getQTE
        {
            get { return quantite; }
        }

        public double setBordureWidth
        {
            set { Bordure.Width = value; }
        }
    }
}