namespace Wonderlust.UI.Domain.Entities;

public class User(string username, string email)
{
    public Guid Id { get; private set; }

    public string Username { get; set; } = username;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = email;
    public DateTimeOffset RegistrationDate { get; set; } = DateTimeOffset.UtcNow;
    public int Karma { get; set; } = 0;

    public User(Guid id, string username, string email) : this(username, email)
    {
        Id = id;
    }
}