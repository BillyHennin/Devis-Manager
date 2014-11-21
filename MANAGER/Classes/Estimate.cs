// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Collections.Generic;

namespace MANAGER.Classes
{
    internal class Estimate
    {
        private readonly DateTime _date;
        private readonly List<Merchandise> _listMerchandise;

        public Estimate(List<Merchandise> list)
        {
            _listMerchandise = list;
            TotalPrix = 0;
            _date = DateTime.Now;
        }

        public double TotalPrix { get; set; }

        public Client Client { get; set; }

        public DateTime GetDate { get { return _date; } }

        public List<Merchandise> GetList { get { return _listMerchandise; } }

        public Merchandise this[int i] { get { return GetList[i]; } }
    }
}