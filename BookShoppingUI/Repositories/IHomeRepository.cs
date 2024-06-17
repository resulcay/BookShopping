namespace BookShoppingUI.Repositories
{
    public interface IHomeRepository
    {
         Task<IEnumerable<Book>> DisplayBooks(string searchTerm = "", int genreId = 0);
         Task<IEnumerable<Genre>> DisplayGenres();
    }
}