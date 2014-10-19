// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System.Windows.Controls;

namespace SLAM3.Classes
{
    internal class Marchandise
    {
        private readonly Border bordure;
        private readonly double prix;
        private int id;
        private string nom;
        private int quantite;

        public Marchandise(int id, string nom, int quantite, double prix, Border bordure)
        {
            this.id = id;
            this.nom = nom;
            this.quantite = quantite;
            this.prix = prix;
            this.bordure = bordure;
        }

        public double getPrix { get { return prix; } }

        public double setBordureWidth { set { bordure.Width = value; } }

        public Border getBordure { get { return bordure; } }
    }
}