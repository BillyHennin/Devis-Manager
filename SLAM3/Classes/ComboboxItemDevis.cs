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