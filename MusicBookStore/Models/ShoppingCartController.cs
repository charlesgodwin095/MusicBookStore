using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicBookStore.Data;

namespace MusicBookStore.Models
{
    // Controllers/ShoppingCartController.cs
    [Authorize] // Optional: remove if you want anonymous shopping
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _cartService; // Fixed variable name
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ShoppingCartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cartId = _cartService.GetCartId();
            var cartItems = await _context.CartItems
                .Where(c => c.CartId == cartId)
                .Include(c => c.Product)
                .ToListAsync();

            var viewModel = new ShoppingCartViewModel
            {
                cartItems = cartItems,
                CartTotal = cartItems.Sum(cartItem => cartItem.Product.Price * cartItem.Quantity) // Fixed variable name
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            await _cartService.AddToCartAsync(productId, quantity);

            TempData["SuccessMessage"] = $"{product.Name} added to cart!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> RemoveFromCart(int productId)
        {
            var cartId = _cartService.GetCartId();
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.CartId == cartId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem); // Fixed variable name
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return await RemoveFromCart(productId);
            }

            var cartId = _cartService.GetCartId(); // Fixed method name
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.CartId == cartId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearCart()
        {
            var cartId = _cartService.GetCartId();
            var cartItems = await _context.CartItems
                .Where(c => c.CartId == cartId)
                .ToListAsync(); // Fixed missing method call

            _context.CartItems.RemoveRange(cartItems); // Fixed variable name
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
    


        
