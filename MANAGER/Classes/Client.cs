// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

using System.Collections.Generic;

#endregion

namespace MANAGER.Classes
{
    public class Client
    {
        public Client(int id, string denomination, string telephone, string email)
        {
            this.id = id;
            this.denomination = denomination;
            this.telephone = telephone;
            this.email = email;
        }

        public int id { get; set; }

        public string denomination { get; set; }

        public string telephone { get; set; }

        public string email { get; set; }

        public List<Estimate> listEstimate { get; set; }
    }
}