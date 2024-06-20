using Microsoft.AspNetCore.Identity;

namespace BookShoppingUI.Repositories
{
    public abstract class UserTracker
    {
        protected readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected UserTracker(
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            this.context = context;
        }

        public string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext!.User;
            string userId = _userManager.GetUserId(principal)!;

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User not found");
            }

            return userId;
        }
    }
}
