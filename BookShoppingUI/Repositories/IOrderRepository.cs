namespace BookShoppingUI.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrder();
    }
}
