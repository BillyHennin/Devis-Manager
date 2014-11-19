using MANAGER.Classes;

namespace MANAGER.ComboBox
{
    internal class ComboboxItemMerchandise
    {
        public string Text { private get; set; }
        public Merchandise Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
