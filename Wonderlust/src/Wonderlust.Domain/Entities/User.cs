﻿namespace Wonderlust.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }

    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTimeOffset RegistrationDate { get; private set; }
    public int Karma { get; private set; } = 0;

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
        Email = email.ToLower();
        RegistrationDate = DateTimeOffset.UtcNow;
    }

    public void UpdateKarma(int change)
    {
        Karma += change;
    }

    public void UpdateUsername(string newUsername)
    {
        if (string.IsNullOrEmpty(newUsername))
        {
            throw new ArgumentException("New username cannot be empty", nameof(newUsername));
        }

        Username = newUsername;
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

        Email = newEmail.ToLower();
    }
}