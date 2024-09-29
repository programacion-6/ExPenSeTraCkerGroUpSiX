using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RestApi.Mappers.Concretes;
using RestApi.Services.Concretes;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/v1/expenses")]
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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? date = null, 
                                                [FromQuery] string? category = null)
        {
            var expenses = await _expenseService.GetExpensesAsync(date, category);
            return Ok(expenses);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExpenseRequest request)
        {
            var expense = request.ToDomain();
            try
            {
                var result = await _expenseService.CreateAsync(expense);
                return Ok(CreatedAtAction(nameof(Get), new { id = result.Id }, result).Value);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message 
                });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid Argument",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message
                });
            }
                        
        }      

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CreateExpenseRequest request)
        {
            var updatedExpense = request.ToDomain();
            var result = await _expenseService.UpdateAsync(id, updatedExpense);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _expenseService.DeleteAsync(id);
            return result ? Ok() : NotFound();
        }
    }
}
