using Microsoft.AspNetCore.Mvc;
namespace RestApi.Domain;
[ApiController]
[Route("api/user")]
public class UserTestController: ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> requestPost([FromBody] User user) 
    {
        return Ok("user");
    }
}