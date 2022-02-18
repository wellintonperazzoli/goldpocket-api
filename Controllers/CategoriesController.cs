using Microsoft.AspNetCore.Mvc;
using GoldPocket.Models;
using GoldPocket.Services.ModelServices;
using Microsoft.AspNetCore.Authorization;
using GoldPocket.Enums;

namespace GoldPocket.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoriesController : BaseController
    {
        private readonly CategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(CategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _categoryService.ToList());


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int? id) => Ok(await _categoryService.Get(id));


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            _logger.LogInformation("Trying to create new category: " + category.Name);
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.Create(category);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState.Values);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Category category)
        {
            _logger.LogInformation($"Trying to update category {category.Id} ");

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.Update(category);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            return BadRequest(ModelState.Values);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                await _categoryService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
