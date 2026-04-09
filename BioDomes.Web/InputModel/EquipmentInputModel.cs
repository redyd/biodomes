using System.ComponentModel.DataAnnotations;
using BioDomes.Web.Validators;

namespace BioDomes.Web.InputModel;

public class EquipmentInputModel
{
    [Required(ErrorMessage = "Le nom est requis.")]
    [MaxLength(75, ErrorMessage = "Le nom ne peut pas dépasser 75 caractères.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ressource produite est requise.")]
    [Range(1, int.MaxValue, ErrorMessage = "La ressource produite doit être un ID valide.")]
    public int? ProducedResourceId { get; set; }

    [Required(ErrorMessage = "La ressource consommée est requise.")]
    [Range(1, int.MaxValue, ErrorMessage = "La ressource consommée doit être un ID valide.")]
    public int? ConsumedResourceId { get; set; }

    [Required(ErrorMessage = "Une image est requise.")]
    [DataType(DataType.Upload)]
    [AllowedExtensions([".jpg", ".jpeg", ".png", ".gif"],
        ErrorMessage = "Le fichier doit être une image (jpg, jpeg, png, gif).")]
    public IFormFile? Image { get; set; }
}