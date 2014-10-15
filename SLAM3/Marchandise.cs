// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

namespace SLAM3
{
    internal class Marchandise
    {
        private int id;
        private string nom;
        private double prix;
        private int quantite;

        public Marchandise(int id, string nom, int quantite, double prix)
        {
            this.id = id;
            this.nom = nom;
            this.quantite = quantite;
            this.prix = prix;
        }
    }
}