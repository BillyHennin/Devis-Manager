
namespace SLAM3
{
    public class Client
    {
        private readonly string telephone;
        private readonly string denomination;
        private readonly string email;

        public Client()
        {
            id = 0;
            denomination = "";
            telephone = "";
            email = "";
        }

        public Client(int id, string denomination, string telephone, string email)
        {
            this.id = id;
            this.denomination = denomination;
            this.telephone = telephone;
            this.email = email;
        }

        public int id { get; set; }

        public string getDenomination { get { return denomination; } }
        public string getTelephone { get { return telephone; } }
        public string getEmail { get { return email; } }
    }
}
