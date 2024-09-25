using Microsoft.AspNetCore.Mvc;
using RestApi.Domain;
using RestApi.Services; 

namespace RestApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager _userManager;

    public UserController(UserManager userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var users = await _userManager.ReadAsync(id);
        var user = users.FirstOrDefault();
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        var createdUser = await _userManager.CreateAsync(user);
        return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> Update([FromBody] User user)
    {
        if (user.Id == Guid.Empty)
            return BadRequest("El ID del usuario es obligatorio.");

        var result = await _userManager.UpdateAsync(user.Id, user);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _userManager.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}

