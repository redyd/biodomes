using System.ComponentModel.DataAnnotations;

namespace BioDomes.Web.InputModel;

public class BiomeSpecieInputModel
{
    [Required(ErrorMessage = "Une espèce est requise")]
    public int? SpecieId { get; set; }
    [Required(ErrorMessage = "La quantité est requise")]
    [Range(minimum:1, maximum: int.MaxValue, ErrorMessage = "Entrez un nombre valide")]
    public int? Count { get; set; }
}