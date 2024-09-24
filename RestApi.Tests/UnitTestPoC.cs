using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Newtonsoft.Json;
namespace RestApi;
using RestApi.Domain;

public class UnitTestPoC : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UnitTestPoC(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Request_Returns200OK_WhenValidUserIsSent()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "User@email.com",
            Password = "password"
        };
        var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/user", jsonContent);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}