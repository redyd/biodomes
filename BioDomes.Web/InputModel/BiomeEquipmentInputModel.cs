using System.ComponentModel.DataAnnotations;

namespace BioDomes.Web.InputModel;

public class BiomeEquipmentInputModel
{
    [Required(ErrorMessage = "Un équipement est requis")]
    public string EquipmentSlug { get; set; } = null!;
    
    [Required(ErrorMessage = "La quantité est requise")]
    [Range(minimum:1, maximum: int.MaxValue, ErrorMessage = "Entrez un nombre valide")]
    public int? Count { get; set; }
}