using System.ComponentModel.DataAnnotations;
using BioDomes.Web.Validators;

namespace BioDomes.Web.InputModel;

public class BiomeInputModel
{
    [Required(ErrorMessage = "Le nom du biome est requis")]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La température est requise.")]
    public double Temperature { get; set; }

    [Required(ErrorMessage = "L'humidité absolue est requise.")]
    [Range(0, double.MaxValue, ErrorMessage = "L'humidité doit être positive.")]
    public double AbsoluteHumidity { get; set; }

    [AllowedExtensions([".jpg", ".jpeg", ".png", ".gif"],
        ErrorMessage = "Le fichier doit être une image (jpg, jpeg, png, gif).")]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public IFormFile? ImageUrl { get; set; }
}