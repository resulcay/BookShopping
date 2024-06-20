using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> AddItem(int bookId, int quantity = 1, int redirect = 0)
        {
            var cartCount = await _cartRepository.AddItem(bookId, quantity);

            if (redirect == 0)
            {
                return Ok(cartCount);
            }

            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int bookId)
        {
            await _cartRepository.RemoveItem(bookId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItemCount = await _cartRepository.GetCartItemCount();
            return Ok(cartItemCount);
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckOutModel checkOutModel)
        {
            if (!ModelState.IsValid)
            {
                return View(checkOutModel);
            }

            bool isCheckOutSuccess = await _cartRepository.DoCheckOut(checkOutModel);

            if (!isCheckOutSuccess)
            {
                return RedirectToAction(nameof(OrderFailure));
            }

            return RedirectToAction(nameof(OrderSuccess));
        }

        public IActionResult OrderFailure()
        {
            return View();
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
