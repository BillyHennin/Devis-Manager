

namespace MANAGER.Classes
{
    internal class ComboboxItemMarchandise
    {
        public string Text { get; set; }
        public Marchandise Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
