using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.SessionManager;

public class SessionManager
{
    public User? CurrentUser { get; set; } = new User(Guid.Parse("a98e1225-3916-4c79-9775-7d9a737c5027"),
        "acryoz", "acryoz@email.com");
}