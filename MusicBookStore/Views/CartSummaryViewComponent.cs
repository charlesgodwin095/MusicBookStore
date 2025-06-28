using MusicBookStore.Models;

namespace MusicBookStore.Views
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly ShoppingCartService _cartService;

        public CartSummaryViewComponent(ShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IviewComponentResult> InvokeAsync()
        {
            var cartId = _cartService.GetCartId();
            var count = await _cartService.GetCartItemCountAsync();
            return View(count);
        }


    }
}
