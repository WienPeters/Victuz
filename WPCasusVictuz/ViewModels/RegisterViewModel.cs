using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Het wachtwoord is verplicht.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Bevestig je wachtwoord.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen.")]
    public string ConfirmPassword { get; set; }
}
