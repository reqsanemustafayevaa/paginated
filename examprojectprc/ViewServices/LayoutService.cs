using examprojectpr.Core.Models;
using examprojectpr.Data.DAL;
using Microsoft.AspNet.Identity;

namespace examprojectprc.ViewServices
{
    public class LayoutService
    {
        private readonly AppDbcontext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LayoutService(AppDbcontext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Setting>> GetBook()
        {
            var settings = _context.Settings.ToList();
            return settings;
        }

        public async Task<AppUser> GetAppUser()
        {
            AppUser user = null;
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                string username = _httpContextAccessor.HttpContext.User.Identity.Name;
                user = await _userManager.FindByNameAsync(username);
            }

            return user;
        }

    
    }
}
