using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using RestApi.Domain;

[ApiController]
[Route("api/[controller]")]
public class ExpenseCategoryController : ControllerBase
{
    private static List<ExpenseCategory> _defaultCategories = new List<ExpenseCategory>
    {
        new ExpenseCategory { Id = Guid.NewGuid(), Name = "Electronics" },
        new ExpenseCategory { Id = Guid.NewGuid(), Name = "Transport" },
        new ExpenseCategory { Id = Guid.NewGuid(), Name = "Products" }
    };

    private static List<ExpenseCategory> _customCategories = new List<ExpenseCategory>();

    [HttpGet]
    public IActionResult GetAllCategories()
    {
        var allCategories = _defaultCategories.Concat(_customCategories).ToList();
        return Ok(allCategories);
    }

    [HttpPost("{Name}")]
    public IActionResult CreateCustomExpenseCategory([FromRoute] ExpenseCategory newCategory)
    {
        if (_defaultCategories.Any(category => category.Name.Equals(newCategory.Name)) ||
            _customCategories.Any(category => category.Name.Equals(newCategory.Name)))
        {
            return BadRequest("Category already exists.");
        }

        newCategory.Id = Guid.NewGuid();
        _customCategories.Add(newCategory);
        return CreatedAtAction(nameof(GetExpenseCategoryById), new { id = newCategory.Id }, newCategory);
    }

    [HttpGet("{id}")]
    public IActionResult GetExpenseCategoryById(Guid id)
    {
        var category = _defaultCategories.Find(category => category.Id == id) ??
                       _customCategories.Find(category => category.Id == id);

        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateExpenseCustomCategory(Guid id, [FromBody] ExpenseCategory updatedCategory)
    {
        var category = _customCategories.FirstOrDefault(category => category.Id == id);
        if (category == null)
        {
            return BadRequest("Custom expense category not found.");
        }

        category.Name = updatedCategory.Name;
        return NoContent();  
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteExpenseCustomCategory(Guid id)
    {
        var category = _customCategories.FirstOrDefault(category => category.Id == id);
        if (category == null)
        {
            return BadRequest("Custom expense category not found.");
        }

        _customCategories.Remove(category);
        return NoContent();
    }
}