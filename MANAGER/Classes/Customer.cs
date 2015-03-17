using System.Collections.Generic;

namespace MANAGER.Classes
{
    public class Customer
    {
        public Customer(int id, string denomination, string telephone, string email)
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
