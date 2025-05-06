using System.Net.Mail;

namespace Wonderlust.Domain;

public class Utils
{
    public static bool IsValidEmail(string email)
    {
        return MailAddress.TryCreate(email, out _);
    }
}