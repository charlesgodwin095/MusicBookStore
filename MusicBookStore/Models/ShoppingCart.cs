using MusicBookStore.Data;

namespace MusicBookStore.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<ShoppingCartItem> Items { get; set; }

        private readonly ApplicationDbContext _context;
        public string ShoppingCartId { get; set; }

        public ShoppingCart(ApplicationDbContext context)
        {
            _context = context;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetRequiredService<ApplicationDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Products product, int quantity)
        {
            var cartItem = _context.CartItems.SingleOrDefault(
                c => c.ProductId == product.Id &&
                c.ShoppingCartId == ShoppingCartId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    ShoppingCartId = ShoppingCartId
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }
            _context.SaveChanges();
        }

        public List<CartItem> GetCartItems()
        {
            return _context.CartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Include(c => c.Products)
                .ToList();
        }

        public decimal GetCartTotal()
        {
            return _context.CartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Products.Price * c.Quantity)
                .Sum();
        }

        public void ClearCart()
        {
            var cartItems = _context.CartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId);

            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }
    }

    public class ShoppingCartItem
    {
        private readonly ApplicationDbContext _context;
        public string ShoppingCartId { get; set; }

        public ShoppingCartItem(ApplicationDbContext context)
        {
            _context = context;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            var session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetRequiredService<ApplicationDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Products product, int quantity)
        {
            var cartItem = _context.CartItems.SingleOrDefault(
                c => c.ProductId == product.Id &&
                c.ShoppingCartId == ShoppingCartId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    ShoppingCartId = ShoppingCartId
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }
            _context.SaveChanges();
        }

        public List<CartItem> GetCartItems()
        {
            return _context.CartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Include(c => c.Products)
                .ToList();
        }

        public decimal GetCartTotal()
        {
            return _context.CartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Products.Price * c.Quantity)
                .Sum();
        }

        public void ClearCart()
        {
            var cartItems = _context.CartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId);

            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }
    }
}

