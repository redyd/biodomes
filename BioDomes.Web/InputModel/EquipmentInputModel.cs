using System.ComponentModel.DataAnnotations;
using BioDomes.Web.Validators;

namespace BioDomes.Web.InputModel;

public class EquipmentInputModel
{
    [Required(ErrorMessage = "Le nom est requis.")]
    [MaxLength(75, ErrorMessage = "Le nom ne peut pas dépasser 75 caractères.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "La ressource produite est requise.")]
    public string ProducedResourceSlug { get; set; } = null!;

    [Required(ErrorMessage = "La ressource consommée est requise.")]
    public string ConsumedResourceSlug { get; set; } = null!;

    [Required(ErrorMessage = "Une image est requise.")]
    [DataType(DataType.Upload)]
    [AllowedExtensions([".jpg", ".jpeg", ".png", ".gif"],
        ErrorMessage = "Le fichier doit être une image (jpg, jpeg, png, gif).")]
    public IFormFile Image { get; set; } = null!;
}