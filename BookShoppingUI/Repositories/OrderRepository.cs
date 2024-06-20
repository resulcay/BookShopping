using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingUI.Repositories
{
    public class OrderRepository : UserTracker, IOrderRepository
    {
        public OrderRepository(UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context)
            : base(userManager, httpContextAccessor, context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrder()
        {
            var orders = await context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetails)!
                .ThenInclude(od => od.Book)
                .ThenInclude(b => b!.Genre)
                .Where(o => o.UserId == GetUserId())
                .ToListAsync();

            return orders;
        }
    }
}
