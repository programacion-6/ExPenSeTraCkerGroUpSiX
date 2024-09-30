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
    private readonly IValidator<User> _userProfileValidator;
    private readonly IValidator<User.PasswordUpdate> _userPasswordUpdateValidator;
    private PasswordRepositoryService _passwordRepositoryService;

    public UserController(UserService userManager, IValidator<User> validator, IValidator<User> userProfileValidator,
        IValidator<User.PasswordUpdate> userPasswordUpdateValidator, PasswordRepositoryService passwordRepositoryService)
    {
        _userManager = userManager;
        _validator = validator;
        _userProfileValidator = userProfileValidator;
        _userPasswordUpdateValidator = userPasswordUpdateValidator;
        _passwordRepositoryService = passwordRepositoryService;
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
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProfileRequest request)
    {
        var user = request.ToDomain();
        var validationResult = await _userProfileValidator.ValidateAsync(user);
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

    [HttpGet("password-reset/{id:guid}")]
    public async Task<IActionResult> PasswordReset([FromRoute] Guid id)
    {
        var user = await _userManager.ReadAsync(id);
        var resetCode = _userManager.PasswordReset(user);
        _passwordRepositoryService.Add(id, resetCode);
        return Ok("Please check you email inbox");
    }

    [HttpPut("password-reset/{id:guid}")]
    public async Task<IActionResult> PasswordResetConfirm([FromRoute] Guid id, UpdatePasswordRequest request)
    {
        if (_passwordRepositoryService.IsEmpty())
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Password reset code not available",
                Status = StatusCodes.Status400BadRequest,
                Detail = "You have to ask for a reset code first"
            });
        }

        if (!_passwordRepositoryService.Contains(id))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Password reset code not available",
                Status = StatusCodes.Status400BadRequest,
                Detail = "You have to ask for a reset code first"
            });
        }

        var petition = request.ToDomain();
        var validationResult = await _userPasswordUpdateValidator.ValidateAsync(petition);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = validationResult.Errors.FirstOrDefault().ErrorMessage
            });
        }

        var resetCode = _passwordRepositoryService.Get(id);
        if (!resetCode.Equals(petition.Code))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid password reset code",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"Provided code {petition.Code} is invalid"
            });
        }

        var result = await _userManager.UpdateAsync(id, petition);
        _passwordRepositoryService.Remove(id);
        return result ? Ok() : NotFound();
    }
}