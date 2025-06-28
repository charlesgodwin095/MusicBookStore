using Microsoft.AspNetCore.Mvc;

namespace MusicBookStore.Models
{
    public class CartController : Controller
    {
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            // Your cart logic here
            return RedirectToAction("Index", "Products");
        }
    }
}
