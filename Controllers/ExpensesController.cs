using Microsoft.AspNetCore.Mvc;
using GoldPocket.Models;
using GoldPocket.Services.ModelServices;
using Microsoft.AspNetCore.Authorization;

namespace GoldPocket.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class ExpensesController : BaseController
    {
        private readonly ExpenseService _expenseService;

        public ExpensesController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _expenseService.ToList());


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int? id) => Ok(await _expenseService.Get(id));


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Expense expenseDetail)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _expenseService.Create(expenseDetail);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            return BadRequest(ModelState.Values);
        }


        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Expense expenseDetail)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _expenseService.Update(expenseDetail);
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
                await _expenseService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
