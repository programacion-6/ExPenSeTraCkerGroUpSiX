using Microsoft.AspNetCore.Mvc;
using RestApi.Domain;
using RestApi.Mappers.Concretes;
using RestApi.Services.Concretes;

[ApiController]
[Route("api/v1/[controller]")]
public class BudgetController : ControllerBase
{
    private readonly ExpenseService _expenseService;

    public BudgetController(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet("month-with-most-expenses")]
    public async Task<IActionResult> GetMonthWithMostExpenses()
    {
        var result = await _expenseService.GetMonthWithMostExpensesAsync();
        return Ok(result);
    }

    [HttpGet("month-with-most-category-expenses")]
    public async Task<IActionResult> GetMonthWithMostExpensesByCategory()
    {
        var result = await _expenseService.GetCategoryWithMostExpensesAsync();
        return Ok(result);
    }
}