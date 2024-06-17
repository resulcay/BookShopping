using Microsoft.EntityFrameworkCore;

namespace BookShoppingUI.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;
        
        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> DisplayBooks(string searchTerm = "", int genreId = 0)
        {
            searchTerm = searchTerm.Trim().ToLower();

            var books = await (from book in _context.Books
                         join genre in _context.Genres
                         on book.GenreId equals genre.Id
                         where string.IsNullOrEmpty(searchTerm) || (book != null && book.BookName!.ToLower().StartsWith(searchTerm))
                         select new Book
                         {
                             Id = book.Id,
                             AuthorName = book.AuthorName,
                             BookName = book.BookName,
                             Image = book.Image,
                             GenreId = book.GenreId,
                             GenreName = genre.GenreName,
                             Price = book.Price,
                         }
                         ).ToListAsync();

            if (genreId > 0)
            {
                books = books.Where(b => b.GenreId == genreId).ToList();
            }

            return books;
        } 

        public async Task<IEnumerable<Genre>> DisplayGenres()
        {
            return await _context.Genres.ToListAsync();
        }
    }
}
