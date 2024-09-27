using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using RestApi.Domain;
using RestApi.Mappers;
using RestApi.Services;

[ApiController]
[Route("api/v1/[controller]")]
public class ExpenseCategoryController : ControllerBase
{
    private readonly ExpenseCategoryService _expenseCategoryService;

    public ExpenseCategoryController(ExpenseCategoryService expenseCategoryService)
    {
        _expenseCategoryService = expenseCategoryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateExpenseCategoryRequest request)
    {
        var category = request.ToDomain(); 
        var createdCategory = await _expenseCategoryService.CreateAsync(category);
        return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _expenseCategoryService.ReadAsync();
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var category = await _expenseCategoryService.ReadAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] ExpenseCategory updatedCategory)
    {
        var existingCategory = await _expenseCategoryService.ReadAsync(id);

        if (existingCategory == null) return NotFound();
        var result = await _expenseCategoryService.UpdateAsync(id, updatedCategory);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var existingCategory = await _expenseCategoryService.ReadAsync(id);

        if (existingCategory == null) return NotFound();

        var result = await _expenseCategoryService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}