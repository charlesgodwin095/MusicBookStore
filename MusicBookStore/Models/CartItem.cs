namespace MusicBookStore.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Products Products { get; set; }
        public int Qauntity { get; set; }
        public string ShoppingCartId { get; set; } // Session-based or UserId if authenticated
    }

    //Models/Product.cs
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        // Add other product properties
    }

}
