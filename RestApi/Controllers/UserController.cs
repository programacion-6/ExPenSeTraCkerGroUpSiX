using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RestApi.Domain;
using RestApi.Mappers.Concretes;
using RestApi.Persistence.DataBase;
using RestApi.Services.Concretes; 

namespace RestApi.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userManager;
    private readonly IValidator<User> _validator;

    public UserController(UserService userManager, IValidator<User> validator)
    {
        _userManager = userManager;
        _validator = validator;
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
        var validationResult = await _validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = validationResult.Errors.FirstOrDefault().ErrorMessage
            });
        }

        var createdUser = await _userManager.CreateAsync(user);
        return Ok(CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser));

        
    }

    [HttpPut("profile/{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CreateUserRequest request)
    {
        var user = request.ToDomain();
        var validationResult = await _validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = validationResult.Errors.FirstOrDefault().ErrorMessage
            });
        }

        var result = await _userManager.UpdateAsync(id, user);
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
