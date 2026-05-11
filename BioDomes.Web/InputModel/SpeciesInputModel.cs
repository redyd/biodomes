using System.ComponentModel.DataAnnotations;
using BioDomes.Web.Validators;

namespace BioDomes.Web.InputModel;

public class SpeciesInputModel
{
    [Required(ErrorMessage = "Le nom est requis.")]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Le type est requis.")]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public string ClassificationSlug { get; set; } = null!;

    [Required(ErrorMessage = "Le régime alimentaire est requis.")]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public string DietSlug { get; set; } = null!;
    
    [Range(1, 10000, ErrorMessage = "La taille doit être entre 1 et 10000")]
    public int Height { get; set; }

    [Range(1, 100000000, ErrorMessage = "Le poids doit être entre 1 et 100000000")]
    public int Weight { get; set; }

    public bool Public { get; set; }
    
    [AllowedExtensions([".jpg", ".jpeg", ".png", ".gif"],
        ErrorMessage = "Le fichier doit être une image (jpg, jpeg, png, gif).")]
    [RegularExpression(@"^[^<>&]*$", ErrorMessage = "HTML tags ne sont pas autorisé")]
    public IFormFile? Image { get; set; }
}
