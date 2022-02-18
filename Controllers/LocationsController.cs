using Microsoft.AspNetCore.Mvc;
using GoldPocket.Models;
using GoldPocket.Services.ModelServices;
using Microsoft.AspNetCore.Authorization;

namespace GoldPocket.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class LocationsController : BaseController
    {
        private readonly LocationService _locationService;

        public LocationsController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _locationService.ToList());

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int? id) => Ok(await _locationService.Get(id));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Location location)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _locationService.Update(location);
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
