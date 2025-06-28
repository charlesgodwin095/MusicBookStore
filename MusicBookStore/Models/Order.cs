namespace MusicBookStore.Models
{
    public class Order
    {
        public int id { get; set; }
        public string UserId { get; set; } // Link to I dentity User
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderItems> OrderItems { get; set; }
    }

    public class OrderItems 
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
        public decimal Price { get; set; }
    }
}
