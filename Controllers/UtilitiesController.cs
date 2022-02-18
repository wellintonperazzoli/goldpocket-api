using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GoldPocket.Data;
using Microsoft.EntityFrameworkCore;
using GoldPocket.Services;
using GoldPocket.Services.ModelServices;

namespace GoldPocket.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class UtilitiesController : BaseController
    {
        private readonly GoldPocketContext _context;
        private readonly UserService _userService;
        private readonly ExpenseService _expenseService;

        public UtilitiesController(GoldPocketContext context, UserService userService, ExpenseService expenseService)
        {
            _context = context;
            _userService = userService;
            _expenseService = expenseService;
        }

        [HttpGet]
        [Route("item-type/")]
        public async Task<IActionResult> Item() =>
            Ok(await _context.Item.Where(l => l.appUserId == _userService.getCurrentUser().Result.Id).Select(i => new { i.Name, Type = i.measureType.ToString() }).ToListAsync());

        [HttpGet]
        [Route("current-month/")]
        public async Task<IActionResult> Index() => Ok(await _expenseService.CurrentMonth());
    }

}
