using System.Text;
using Wonderlust.UI.Application.SessionManager.Requests;
using Wonderlust.UI.Application.SessionManager.Responses;
using Wonderlust.UI.Domain.Entities;
using SerializerLib.Json;

namespace Wonderlust.UI.Application.SessionManager;

public class SessionManager(HttpClient httpClient)
{
    public User? CurrentUser { get; set; } = new User(Guid.Parse("63fcae80-1acb-455f-a7d8-03178f994fca"),
        "acryoz", "acryoz@example.com")
    {
        Password = "password"
    };

    private static string? token = null;

    public async Task<string> GetToken()
    {
        if (token != null)
        {
            return token;
        }

        var content = new StringContent
        (
            JsonSerializer.Serialize(new LoginRequest(CurrentUser.Email, CurrentUser.Password)),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync("auth/login", content);
        response.EnsureSuccessStatusCode();

        var responseBody = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync());
        token = responseBody!.Token;

        return token;
    }
}