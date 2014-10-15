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

        public Devis(List<Marchandise> listMarchandise)
        {
            this.listMarchandise = listMarchandise;
        }

        public List<Marchandise> getList()
        {
            return listMarchandise;
        }
    }
}