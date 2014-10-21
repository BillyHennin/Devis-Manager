// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System.Collections.Generic;

namespace SLAM3.Classes
{
    public class Client
    {
        private readonly string denomination;
        private readonly string email;
        private readonly string telephone;

        public Client(string denomination, string telephone, string email)
        {
            this.denomination = denomination;
            this.telephone = telephone;
            this.email = email;
        }

        public string getDenomination { get { return denomination; } }
        public string getTelephone { get { return telephone; } }
        public string getEmail { get { return email; } }
    }
}