using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GoldPocket.Data;
using Microsoft.EntityFrameworkCore;
using GoldPocket.Services;
using GoldPocket.Services.ModelServices;

namespace GoldPocket.Controllers.API
{
    [ApiController]
    [Route("/")]
    public class PublicController : BaseController
    {
        [HttpGet]
        [HttpPost]
        [HttpPut]
        [HttpDelete]
        [HttpHead]
        public IActionResult Get() => Ok("Hello");

    }

}
