using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GoldPocket.Data;
using Microsoft.EntityFrameworkCore;
using GoldPocket.Services;
using GoldPocket.Models;
using GoldPocket.Enums;
using GoldPocket.Models.DB;
using GoldPocket.Services.ModelServices;

namespace GoldPocket.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class ChartController : BaseController
    {
        private readonly ChartService _chartService;

        public ChartController(ChartService chartService)
        {
            _chartService = chartService;
        }

        [HttpGet]
        [Route("expenseCategory/{months?}")]
        public async Task<IActionResult> expenseCategory(int months = 5)
            => Ok(await _chartService.expenseCategory(months));

        [HttpGet]
        [Route("itemCategory/{months?}")]
        public async Task<IActionResult> itemCategory(int months = 5)
            => Ok(await _chartService.itemCategory(months));

        [HttpGet]
        [Route("expenseCategoryDoughnut/")]
        public async Task<IActionResult> expenseCategoryDoughnut()
            => Ok(await _chartService.expenseCategoryDoughnut());

        [HttpGet]
        [Route("itemCategoryDoughnut/")]
        public async Task<IActionResult> itemCategoryDoughnut()
            => Ok(await _chartService.itemCategoryDoughnut());
    }
}
