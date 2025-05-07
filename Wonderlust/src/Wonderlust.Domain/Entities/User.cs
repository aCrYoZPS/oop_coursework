namespace Wonderlust.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }

    public string Username { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public DateTimeOffset RegistrationDate { get; private set; }

    public int Karma { get; private set; } = 0;

    public virtual ICollection<Subscription> Subscriptions { get; private set; } = new List<Subscription>();

    public virtual ICollection<Moderator> ModeratedCommunities { get; private set; } = new List<Moderator>();

    private User() { }

    public User(string username, string passwordHash, string email)
    {
        Id = Guid.NewGuid();
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
        }

        if (!Utils.IsValidEmail(email))
        {
            throw new ArgumentException($"{email} is an invalid email.", nameof(email));
        }

        Username = username;
        PasswordHash = passwordHash;
        Email = email;
        RegistrationDate = DateTimeOffset.UtcNow;
    }

    public void UpdateKarma(int change)
    {
        Karma += change;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
        {
            throw new ArgumentException("New password hash cannot be empty.", nameof(newPasswordHash));
        }

        PasswordHash = newPasswordHash;
    }

    public void UpdateEmail(string newEmail)
    {
        if (!Utils.IsValidEmail(newEmail))
        {
            throw new ArgumentException($"New email {newEmail} is invalid.", nameof(newEmail));
        }

        Email = newEmail;
    }
}