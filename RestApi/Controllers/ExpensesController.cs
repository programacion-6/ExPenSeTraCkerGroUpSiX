using Microsoft.AspNetCore.Mvc;
using RestApi.Mappers;
using RestApi.Services;
using RestApi.Domain;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseService _expenseService;

        public ExpenseController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var expense = await _expenseService.ReadAsync(id);
            return expense == null ? NotFound() : Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Expense expense)
        {
            var result = await _expenseService.CreateAsync(expense);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _expenseService.ReadAsync();
            return Ok(expenses);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] Expense updatedExpense)
        {
            var result = await _expenseService.UpdateAsync(id, updatedExpense);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _expenseService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
