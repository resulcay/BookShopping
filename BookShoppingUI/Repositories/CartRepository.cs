using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingUI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AddItem(int bookId, int quantity)
        {
            using var transaction =  _context.Database.BeginTransaction();

            try
            {
                string userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }

                var cart = await GetCart(userId);

                if (cart is null)
                {
                    cart = new Cart
                    {
                        UserId = userId
                    };

                    _context.Carts.Add(cart);
                }

                _context.SaveChanges();

                var cartItem = _context.CartDetails.FirstOrDefault(cd => cd.CartId == cart.Id && cd.BookId == bookId);

                if (cartItem is not null)
                {
                    cartItem.Quantity += quantity;
                }
                else
                {
                    cartItem = new CartDetail
                    {
                        CartId = cart.Id,
                        BookId = bookId,
                        Quantity = quantity
                    };

                    _context.CartDetails.Add(cartItem);
                }

                _context.SaveChanges();
                transaction.Commit();

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveItem(int bookId)
        {
            try
            {
                string userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }

                var cart = await GetCart(userId);

                if (cart is null)
                {
                    return false;
                }

                var cartItem = _context.CartDetails.FirstOrDefault(cd => cd.CartId == cart.Id && cd.BookId == bookId);

                if (cartItem is null)
                {
                    return false;
                }
                else if (cartItem.Quantity == 1)
                {
                    _context.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity--;
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Cart>> GetUserCart()
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId)) 
            {
                throw new Exception("User not found");
            }

            var cart = await _context.Carts.
                Include(c => c.CartDetails)!.
                ThenInclude(cd => cd.Book).
                ThenInclude(b => b!.Genre).
                Where(c => c.UserId == userId).
                ToListAsync();
        
            return cart;
        }

        private async Task<Cart?> GetCart(string userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            return cart;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext!.User;
            string userId = _userManager.GetUserId(principal)!;

            return userId;
        }
    }
}
