using MANAGER.Classes;

namespace MANAGER.ComboBox
{
    internal class ComboboxItemCustomer
    {
        public string Text { private get; set; }
        public Customer Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
