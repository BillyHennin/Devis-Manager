// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;
using System.Collections.Generic;

namespace SLAM3.Classes
{
    public class Devis
    {
        private readonly DateTime date;
        private readonly List<Marchandise> listMarchandise;

        public Devis(List<Marchandise> list)
        {
            listMarchandise = list;
            TotalPrix = 0;
            date = DateTime.Now;
        }

        public double TotalPrix { get; set; }
        public Client client { get; set; }

        public DateTime getDate { get { return date; } }
        public List<Marchandise> getList { get { return listMarchandise; } }
        public Marchandise this[int i] { get { return getList[i]; } }
    }
}