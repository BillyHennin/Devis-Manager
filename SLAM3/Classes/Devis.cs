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
        private readonly DateTime _date;
        private readonly List<Marchandise> _listMarchandise;

        public Devis(List<Marchandise> list)
        {
            _listMarchandise = list;
            TotalPrix = 0;
            _date = DateTime.Now;
        }

        public double TotalPrix { get; set; }
        public Client Client { get; set; }

        public DateTime GetDate
        {
            get { return _date; }
        }

        public List<Marchandise> GetList
        {
            get { return _listMarchandise; }
        }

        public Marchandise this[int i]
        {
            get { return GetList[i]; }
        }
    }
}