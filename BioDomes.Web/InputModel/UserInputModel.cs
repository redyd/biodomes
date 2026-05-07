using System.ComponentModel.DataAnnotations;

namespace BioDomes.Web.InputModel;

public class UserInputModel
{
    [Required, EmailAddress] public string Email { get; set; } = null!;

    [MinLength(3, ErrorMessage = "Minimum 3 caractères"), Required] public string? Name { get; set; }

    public DateOnly? Birthdate { get; set; }
    public string? SearchOrganism { get; set; }

    [DataType(DataType.Password)] public string? CurrentPassword { get; set; }

    [DataType(DataType.Password), StringLength(100, MinimumLength = 6)]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password), Compare("NewPassword")]
    public string? ConfirmPassword { get; set; }
    
    public IFormFile? ImageUpload { get; set; }
}