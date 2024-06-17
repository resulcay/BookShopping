namespace BookShoppingUI.Repositories
{
    public interface ICartRepository
    {
        Task<bool> RemoveItem(int bookId);
        Task<bool> AddItem(int bookId, int quantity);
        Task<IEnumerable<Cart>> GetUserCart();
    }
}