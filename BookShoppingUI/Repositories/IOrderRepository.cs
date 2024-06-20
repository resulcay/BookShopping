namespace BookShoppingUI.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrder(bool getAll = false);
        Task ChangeOrderStatus(UpdateOrderStatusModel updateOrderStatusModel);
        Task TogglePaymentStatus(int orderId);
        Task<Order?> GetOrderById(int orderId);
        Task<IEnumerable<OrderStatus>> GetOrderStatuses();
    }
}
