using BookShoppingUI.Models.DTOs;
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

        public async Task ChangeOrderStatus(UpdateOrderStatusModel updateOrderStatusModel)
        {
            var order = await context.Orders.FindAsync(updateOrderStatusModel.OrderId)
                        ?? throw new Exception("Order not found");

            order.OrderStatusId = updateOrderStatusModel.OrderStatusId;
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrder(bool getAll = false)
        {
            var orders = context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetails)!
                .ThenInclude(od => od.Book)
                .ThenInclude(b => b!.Genre)
                .AsQueryable();

            if (!getAll)
            {
                var userId = GetUserId();
                orders = orders.Where(o => o.UserId == userId);

                return await orders.ToListAsync();
            }

            return await orders.ToListAsync();
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            return await context.Orders.FindAsync(orderId);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await context.OrderStatuses.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await context.Orders.FindAsync(orderId)
                        ?? throw new Exception("Order not found");

            order.IsPaid = !order.IsPaid;
            await context.SaveChangesAsync();
        }
    }
}
