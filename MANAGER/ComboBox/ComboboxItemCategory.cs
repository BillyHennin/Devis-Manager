
using MANAGER.Classes;

namespace MANAGER.ComboBox
{
    internal class ComboboxItemCategory
    {
        public string Text { private get; set; }
        public Category Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
