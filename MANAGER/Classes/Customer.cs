// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System.Collections.Generic;

namespace MANAGER.Classes
{
    public class Customer
    {
        public Customer(int id, string name, string cellphone, string email)
        {
            this.id = id;
            this.name = name;
            this.cellphone = cellphone;
            this.email = email;
        }

        public int id { get; set; }
        public string name { get; set; }
        public string cellphone { get; set; }
        public string email { get; set; }
        public List<Estimate> listEstimate { get; set; }
    }
}