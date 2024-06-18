namespace BookShoppingUI.Repositories
{
    public interface ICartRepository
    {
        Task<int> RemoveItem(int bookId);
        Task<int> AddItem(int bookId, int quantity);
        Task<Cart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
    }
}