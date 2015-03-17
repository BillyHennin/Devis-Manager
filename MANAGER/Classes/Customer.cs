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
