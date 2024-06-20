using StatusAlias = BookShoppingUI.Constants; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingUI.Repositories
{
    public class CartRepository : UserTracker, ICartRepository
    {
        public CartRepository(UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context)
            : base(userManager, httpContextAccessor, context)
        {
        }

        public async Task<int> AddItem(int bookId, int quantity)
        {
            string userId = GetUserId();
            using var transaction =  context.Database.BeginTransaction();

            try
            {
                var cart = await GetCart(userId);

                if (cart is null)
                {
                    cart = new Cart
                    {
                        UserId = userId
                    };

                    context.Carts.Add(cart);
                }

                context.SaveChanges();

                var cartItem = context.CartDetails.FirstOrDefault(cd => cd.CartId == cart.Id && cd.BookId == bookId);

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
                        Quantity = quantity,
                        UnitPrice = context.Books.Find(bookId)!.Price
                    };

                    context.CartDetails.Add(cartItem);
                }

                context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                
            }

            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int bookId)
        {
            string userId = GetUserId();

            try
            {
                var cart = await GetCart(userId) ?? throw new Exception("Invalid Cart");

                var cartItem = context.CartDetails.FirstOrDefault(cd => cd.CartId == cart.Id && cd.BookId == bookId);

                if (cartItem is null)
                {
                    throw new Exception("Cart is empty");
                }
                else if (cartItem.Quantity == 1)
                {
                    context.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity--;
                }

                context.SaveChanges();
            }
            catch (Exception)
            {
               
            }

            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<Cart> GetUserCart()
        {
            var userId = GetUserId();

            var cart = await context.Carts.
                Include(c => c.CartDetails)!.
                ThenInclude(cd => cd.Book).
                ThenInclude(b => b!.Genre).
                Where(c => c.UserId == userId).
                FirstOrDefaultAsync();
        
            return cart!;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }

            var data = await (from cart in context.Carts
                              join cartDetail in context.CartDetails
                              on cart.Id equals cartDetail.CartId
                              where cart.UserId == userId
                              select cartDetail.Quantity).SumAsync();
            return data;
        }
         
        public async Task<bool> DoCheckOut()
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                var cart = await GetCart(userId) ?? throw new Exception("Invalid Cart");
                var cartDetails = context.CartDetails.Where(cd => cd.CartId == cart.Id).ToList();

                if (cartDetails.Count == 0)
                {
                    throw new Exception("Cart is empty");
                }

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    OrderStatusId = (int)StatusAlias.OrderStatus.Pending,
                };

                context.Orders.Add(order);
                context.SaveChanges();

                foreach (var item in cartDetails)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    context.OrderDetails.Add(orderDetail);
                }

                context.SaveChanges();
                context.CartDetails.RemoveRange(cartDetails);
                context.SaveChanges();
                transaction.Commit();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private async Task<Cart?> GetCart(string userId)
        {
            var cart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            return cart;
        }
  }
}
