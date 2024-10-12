using System.ComponentModel.DataAnnotations;

public class ContactFormModel
{
    [Required(ErrorMessage = "Naam is verplicht.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "E-mail is verplicht.")]
    [EmailAddress(ErrorMessage = "Voer een geldig e-mailadres in.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Bericht is verplicht.")]
    public string Message { get; set; }
}
