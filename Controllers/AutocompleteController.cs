using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GoldPocket.Data;
using Microsoft.EntityFrameworkCore;
using GoldPocket.Services;
using GoldPocket.Enums;

namespace GoldPocket.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class AutocompleteController : BaseController
    {
        private readonly GoldPocketContext _context;
        private readonly UserService _userService;

        public AutocompleteController(GoldPocketContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        [Route("location/")]
        public async Task<IActionResult> Location() =>
            Ok(await _context.Location.Where(l => l.appUserId == _userService.getCurrentUser().Result.Id).Select(u => u.Name).ToListAsync());


        [HttpGet]
        [Route("category/{type}")]
        public async Task<IActionResult> Category(CategoryTypes type) =>
            Ok(await _context.Category.Where(l => l.appUserId == _userService.getCurrentUser().Result.Id && l.Type == type).Select(u => u.Name).ToListAsync());

        [HttpGet]
        [Route("item/")]
        public async Task<IActionResult> Item() =>
            Ok(await _context.Item.Where(l => l.appUserId == _userService.getCurrentUser().Result.Id).Select(u => u.Name).ToListAsync());

        [HttpGet]
        [Route("itemcategory/")]
        public async Task<IActionResult> ItemCategory() =>
            Ok(await _context.Category.Where(l => l.appUserId == _userService.getCurrentUser().Result.Id && l.Type == CategoryTypes.Item).Select(u => u.Name).ToListAsync());

        [HttpGet]
        [Route("expensecategory/")]
        public async Task<IActionResult> ExpenseCategory() =>
            Ok(await _context.Category.Where(l => l.appUserId == _userService.getCurrentUser().Result.Id && l.Type == CategoryTypes.Expense).Select(u => u.Name).ToListAsync());

        [HttpGet]
        [Route("categoryTypes/")]
        public IActionResult GetCategoryTypes()
        {
            return Ok(from CategoryTypes t in Enum.GetValues(typeof(CategoryTypes))
                      select new { value = (int)t, label = t.ToString() });
        }

        [HttpGet]
        [Route("measureTypes/")]
        public IActionResult GetMeasureTypes()
        {
            return Ok(from MeasureTypes t in Enum.GetValues(typeof(CategoryTypes))
                      select new { value = (int)t, label = t.ToString() });
        }

    }
}
