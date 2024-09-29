
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RestApi.Mappers.Concretes;
using RestApi.Services.Concretes;

namespace RestApi.Controllers;

[ApiController]
[Route("api/v1/incomes")]
public class IncomeController : ControllerBase
{
    private readonly IncomeService _incomeService;
    public IncomeController(IncomeService incomeService)
    {
        _incomeService = incomeService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var incomes = await _incomeService.ReadAsync();
        var income = incomes.FirstOrDefault(i => i.Id == id);
        return income == null ? NotFound() : Ok(income);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll([FromQuery] DateTime? date = null)
    {
        var incomes = await _incomeService.GetIncomesAsync(date);
        return Ok(incomes);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIncomeRequest request)
    {
        var income = request.ToDomain();
        try
        {
            var result = await _incomeService.CreateAsync(income);
            return Ok(CreatedAtAction(nameof(Get), new { income.Id }, result).Value);
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
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CreateIncomeRequest request)
    {
        var income = request.ToDomain();
        var result = await _incomeService.UpdateAsync(id, income);
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _incomeService.DeleteAsync(id);
        return result ? Ok() : NotFound();
    }

    
}
