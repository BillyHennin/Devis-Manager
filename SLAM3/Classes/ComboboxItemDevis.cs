// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

namespace SLAM3.Classes
{
    internal class ComboboxItemDevis
    {
        public string Text { get; set; }
        public Devis Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}