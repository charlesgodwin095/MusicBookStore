namespace MusicBookStore.Models
{
    //ViewModels/ShoppingCartViewModel.cs
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public decimal Shippingfee { get; set; } = 5.99m; // Default shipping fee
    }
}
