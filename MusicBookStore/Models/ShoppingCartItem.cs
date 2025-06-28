namespace MusicBookStore.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Products Products { get; set; }
        public int Qauntity { get; set; }
    }
}