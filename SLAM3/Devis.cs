// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System.Collections.Generic;

namespace SLAM3
{
    internal class Devis
    {
        private readonly List<Marchandise> listMarchandise;
        private Client client;

        public Devis(List<Marchandise> list)
        {
            listMarchandise = list;
            client = new Client();
            TotalPrix = 0;
        }

        public List<Marchandise> list { get { return listMarchandise; } }

        public Marchandise this[int i]{ get { return list[i]; }}

        public double TotalPrix { get; set; }

        public Client setClient{ set { client = value; } }
    }
}