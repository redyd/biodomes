using System.ComponentModel.DataAnnotations;

namespace BioDomes.Web.InputModel;

public class UpdateBiomeInputModel
{
    [Required(ErrorMessage = "Le nom du biome est requis")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La température est requise.")]
    public double Temperature { get; set; }

    [Required(ErrorMessage = "L'humidité absolue est requise.")]
    [Range(0, double.MaxValue, ErrorMessage = "L'humidité doit être positive.")]
    public double AbsoluteHumidity { get; set; }

    public IFormFile? Image { get; set; }
    public bool? IsPublic { get; set; }
}