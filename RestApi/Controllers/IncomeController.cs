
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RestApi.Domain;
using RestApi.Mappers.Concretes;
using RestApi.Services.Concretes;
using Microsoft.AspNetCore.Authorization;

namespace RestApi.Controllers;

[ApiController]
[Route("api/v1/incomes")]
[Authorize]
public class IncomeController : ControllerBase
{
    private readonly IncomeService _incomeService;
    private readonly IValidator<Income> _validator;

    public IncomeController(IncomeService incomeService, IValidator<Income> validator)
    {
        _incomeService = incomeService;
        _validator = validator;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var income = await _incomeService.ReadAsync(id);
        return income == null ? NotFound() : Ok(income);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DateTime? date = null)
    {
        var incomes = await _incomeService.GetIncomesAsync(date);
        return Ok(incomes);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIncomeRequest request)
    {
        var income = request.ToDomain();
        var validationResult = await _validator.ValidateAsync(income);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            });
        }
        
        var createdIncome = await _incomeService.CreateAsync(income);
        return CreatedAtAction(nameof(Get), new { id = createdIncome.Id }, createdIncome);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CreateIncomeRequest request)
    {
        var income = request.ToDomain();
        var validationResult = await _validator.ValidateAsync(income);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            });
        }

        var result = await _incomeService.UpdateAsync(id, income);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _incomeService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
