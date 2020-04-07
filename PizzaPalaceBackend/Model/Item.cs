namespace PizzaPalace.Model
{
    public class Item
    {
        public int ItemID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string ImageURL { get; set; }
    }
}
