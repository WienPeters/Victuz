using Microsoft.AspNetCore.Identity;
using System;

public class PasswordService
{
    private readonly PasswordHasher<string> _passwordHasher;

    public PasswordService()
    {
        _passwordHasher = new PasswordHasher<string>();
    }

    // Hasht een wachtwoord
    public string HashPassword(string plainPassword)
    {
        // Gebruik een leeg tekenreeks-id (of geef de gebruiker-id door als je wilt)
        return _passwordHasher.HashPassword(null, plainPassword);
    }

    // Verifieert het wachtwoord
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        // Controleer het wachtwoord
        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
