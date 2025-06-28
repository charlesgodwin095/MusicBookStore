using MusicBookStore.Data;

namespace MusicBookStore.Models
{
    public class ShoppingCart_Service
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContentAccessor;

        public ShoppingCart_Service(ApplicationDbContext context, IHttpContextAccessor httpContentAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContentAccessor;
        }

        public string GetCartId()
        {
            var session = _httpContentAccessor.HttpContext.Session;
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return cardId;
        }

        public async Task AddToCartAsync(int productsId, int quantity)
        {
            var cartId = GetCartId();
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(cartId => cartId.ProductId == productsId && cartId.CartId == cartId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productsId,
                    QuantityId = quantityId

                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();

        }

        // Add other methods like RemoveFromCart, GetCartItems, ClearCart, etc.
    }
}
