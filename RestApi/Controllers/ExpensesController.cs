using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RestApi.Domain;
using RestApi.Mappers.Concretes;
using RestApi.Services.Concretes;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/v1/expenses")]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseService _expenseService;
        private readonly IValidator<Expense> _validator;

        public ExpenseController(ExpenseService expenseService, IValidator<Expense> validator)
        {
            _expenseService = expenseService;
            _validator = validator;
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
            try 
            {
                var expense = request.ToDomain();
                await _expenseService.ValidateCategory(expense, request.Category);

                var validationResult = await _validator.ValidateAsync(expense);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Validation Error",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = validationResult.Errors.FirstOrDefault().ErrorMessage
                    });
                }

                var result = await _expenseService.CreateAsync(expense);
                return Ok(CreatedAtAction(nameof(Get), new { id = result.Id }, result).Value);     
            } 
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message
                });
            }
            
        }      

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CreateExpenseRequest request)
        {
            try 
            {
                var updatedExpense = request.ToDomain();
                await _expenseService.ValidateCategory(updatedExpense, request.Category);

                var validationResult = await _validator.ValidateAsync(updatedExpense);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new ProblemDetails
                    {
                        Title = "Validation Error",
                        Status = StatusCodes.Status400BadRequest,
                        Detail = validationResult.Errors.FirstOrDefault().ErrorMessage
                    });
                }

                var result = await _expenseService.UpdateAsync(id, updatedExpense);
                return result ? Ok() : NotFound();
            } 
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message
                });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _expenseService.DeleteAsync(id);
            return result ? Ok() : NotFound();
        }
    }
}
