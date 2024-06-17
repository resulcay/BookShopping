namespace BookShoppingUI.Repositories
{
    public interface ICartRepository
    {
        Task<int> RemoveItem(int bookId);
        Task<int> AddItem(int bookId, int quantity);
        Task<IEnumerable<Cart>> GetUserCart();
        Task<int> GetCartItem(string userId = "");
    }
}