using GoldPocket.Data;
using GoldPocket.Models;

namespace GoldPocket.Services.ModelServices
{
    public class BaseService
    {
        protected readonly GoldPocketContext _context;
        protected readonly UserService _userService;


        public BaseService(GoldPocketContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }
    }
}
