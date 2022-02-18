using GoldPocket.Data;
using GoldPocket.Models;
using GoldPocket.Models.DB;
using Microsoft.AspNetCore.Identity;

namespace GoldPocket.Services
{
    public class UserService
    {
        protected IHttpContextAccessor _httpContextAccessor;
        protected readonly UserManager<appUser> _userManager;
        public appUser currentUser;

        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<appUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }


        public async Task<appUser> getCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext?.User;
            if (userId == null) return null;
            return await _userManager.GetUserAsync(userId);
        }
    }
}
