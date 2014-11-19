using System.Windows.Controls;

namespace MANAGER.Classes
{
    public class Merchandise
    {
        private readonly string _nom;
        private readonly double _prix;
        private readonly int _quantite;
        private readonly int _id;

        public Merchandise(int id, string nom, int quantite, double prix)
        {
            _id = id;
            _nom = nom;
            _quantite = quantite;
            _prix = prix;
        }



        public Border Border { get; set; }

        public string GetNom { get { return _nom; } }

        public double GetPrix { get { return _prix; } }

        public int GetQte { get { return _quantite; } }

        public int GetId { get { return _id; } }
        /* for future use
        public double SetBordureWidth { set { Bordure.Width = value; } }*/
    }
}
