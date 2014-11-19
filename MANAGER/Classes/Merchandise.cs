﻿// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System.Windows.Controls;

namespace MANAGER.Classes
{
    public class Merchandise
    {
        private readonly int _id;
        private readonly string _nom;
        private readonly double _prix;
        private readonly int _quantite;

        public Merchandise(int id, string nom, int quantite, double prix)
        {
            _id = id;
            _nom = nom;
            _quantite = quantite;
            _prix = prix;
        }

        public Border Border { get; set; }

        public string GetNom { get { return _nom; } }

        public double GetPrix { get { return _prix; } }

        public int GetQte { get { return _quantite; } }

        public int GetId { get { return _id; } }
        /* for future use
        public double SetBordureWidth { set { Bordure.Width = value; } }*/
    }
}