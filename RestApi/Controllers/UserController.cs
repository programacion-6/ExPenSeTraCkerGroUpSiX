using Microsoft.AspNetCore.Mvc;
using RestApi.Domain;
using RestApi.Mappers.Concretes;
using RestApi.Persistence.DataBase;
using RestApi.Services.Concretes; 

namespace RestApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userManager;

    public UserController(UserService userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var user = await _userManager.ReadAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userManager.ReadAsync();
        return users == null ? NotFound() : Ok(users);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = request.ToDomain();
        var createdUser = await _userManager.CreateAsync(user);
        return Ok(CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser));
    }

    [HttpPut("profile")]
    public async Task<IActionResult> Update([FromBody] CreateUserRequest request)
    {
        var user = request.ToDomain();
        var result = await _userManager.UpdateAsync(user.Id, user);
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _userManager.DeleteAsync(id);
        return result ? Ok() : NotFound();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] Guid id)
    {
        UserContext.CurrentUserId = id;
        return Ok();
    }

}
