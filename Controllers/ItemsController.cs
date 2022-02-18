using Microsoft.AspNetCore.Mvc;
using GoldPocket.Models;
using GoldPocket.Services.ModelServices;
using Microsoft.AspNetCore.Authorization;

namespace GoldPocket.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class ItemsController : BaseController
    {
        private readonly ItemService _itemService;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(ItemService itemService, ILogger<ItemsController> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _itemService.ToList());

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int? id) => Ok(await _itemService.Get(id));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Item itemDetail)
        {
            _logger.LogInformation($"Trying to update item {itemDetail.Id}");
            if (ModelState.IsValid)
            {
                try
                {
                    await _itemService.Update(itemDetail);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            return BadRequest(ModelState.Values);
        }
    }
}
