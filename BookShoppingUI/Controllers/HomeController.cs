using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookShoppingUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string searchTerm = "", int genreId = 0)
        {
            BookDisplayModel model = new()
            {
                Books = await _homeRepository.DisplayBooks(searchTerm, genreId),
                Genres = await _homeRepository.DisplayGenres(),
                SearchTerm = searchTerm,
                GenreId = genreId
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
