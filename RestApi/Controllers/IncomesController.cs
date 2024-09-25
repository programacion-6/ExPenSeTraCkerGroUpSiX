
using Microsoft.AspNetCore.Mvc;
using RestApi.Mappers;
using RestApi.Services;

namespace RestApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncomeRequest request)
        {
            var income = request.ToDomain();
            var result = await _incomeService.CreateAsync(income);
            return CreatedAtAction(nameof(Get), new { income.Id }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string category)
        {
            var incomes = await _incomeService.ReadAsync(startDate, endDate, category);
            return Ok(incomes);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CreateIncomeRequest request)
        {
            var income = request.ToDomain();
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