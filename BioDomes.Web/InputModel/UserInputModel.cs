using System.ComponentModel.DataAnnotations;
using BioDomes.Web.Validators;

namespace BioDomes.Web.InputModel;

public class UserInputModel
{
    [Required, EmailAddress]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public string Email { get; set; } = null!;

    [MinLength(3, ErrorMessage = "Minimum 3 caractères"), Required]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public string? Name { get; set; }

    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public DateOnly? Birthdate { get; set; }

    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public string? SearchOrganism { get; set; }

    [DataType(DataType.Password)]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public string? CurrentPassword { get; set; }

    [DataType(DataType.Password), StringLength(100, MinimumLength = 6)]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password), Compare("NewPassword")]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public string? ConfirmPassword { get; set; }

    [AllowedExtensions([".jpg", ".jpeg", ".png", ".gif"],
        ErrorMessage = "Le fichier doit être une image (jpg, jpeg, png, gif).")]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public IFormFile? ImageUpload { get; set; }
}