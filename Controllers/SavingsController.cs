using GoldPocket.Models;
using GoldPocket.Services.ModelServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldPocket.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class SavingsController : BaseController
    {
        private readonly SavingService _service;
        private readonly ILogger<SavingService> _logger;

        public SavingsController(SavingService service, ILogger<SavingService> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.ToList());


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int? id) => Ok(await _service.Get(id));


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Saving data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Create(data);
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
        public async Task<IActionResult> Edit([FromBody] Saving data)
        {
            _logger.LogInformation($"Trying to update category {data.Id}");

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Update(data);
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
                await _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
