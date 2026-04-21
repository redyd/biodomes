using System.ComponentModel.DataAnnotations;
using BioDomes.Web.Validators;

namespace BioDomes.Web.InputModel;

public class SpeciesInputModel
{
    [Required(ErrorMessage = "Le nom est requis.")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ\s-]+$", ErrorMessage = "Le nom ne peut contenir que des lettres, espaces ou tirets.")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Le type est requis.")]
    [IsSpeciesClassification(ErrorMessage = "Type invalide")]
    public string Type { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Le régime alimentaire est requis.")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ\s-]+$", ErrorMessage = "Le régime alimentaire ne peut contenir que des lettres, espaces ou tirets.")]
    public string Diet { get; set; } = string.Empty;
    
    [Range(0, 10000, ErrorMessage = "La taille doit être entre 0 et 10000")]
    public int Height { get; set; }

    [Range(0, 100000000, ErrorMessage = "Le poids doit être entre 0 et 100000000")]
    public int Weight { get; set; }

    public bool Public { get; set; }
    
    public IFormFile? Image { get; set; }
}
