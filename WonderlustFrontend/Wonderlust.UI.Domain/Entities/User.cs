namespace Wonderlust.UI.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }

    public string Username { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; }
    public DateTimeOffset RegistrationDate { get; set; } = DateTimeOffset.UtcNow;
    public int Karma { get; set; } = 0;

    public User(Guid id, string username, string email) : this(username, email)
    {
        Id = id;
    }

    public User(string username, string email)
    {
        Username = username;
        Email = email;
    }

    public User() { }
}