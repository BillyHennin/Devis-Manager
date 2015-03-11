
namespace MANAGER.Classes
{
    class Category
    {
        public Category(int ID, string Description)
        {
            this.ID = ID;
            this.Description = Description;
        }

        public string Description { get; set; }
        public int ID { get; set; }
    }
}
