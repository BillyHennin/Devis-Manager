// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

namespace MANAGER.Classes
{
    public class Client
    {
        private readonly string _denomination;
        private readonly string _email;
        private readonly int _id;
        private readonly string _telephone;

        public Client(int id, string denomination, string telephone, string email)
        {
            _id = id;
            _denomination = denomination;
            _telephone = telephone;
            _email = email;
        }

        public int GetId { get { return _id; } }

        public string GetDenomination { get { return _denomination; } }

        public string GetTelephone { get { return _telephone; } }

        public string GetEmail { get { return _email; } }
    }
}