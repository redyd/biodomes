using System.ComponentModel.DataAnnotations;

namespace BioDomes.Web.InputModel;

public class AddBiomeSpecieInputModel
{
    [Required(ErrorMessage = "Une espèce est requise")]
    public int SpecieId { get; set; }
    [Range(minimum:0, maximum: int.MaxValue, ErrorMessage = "Entrez un nombre valide")]
    public int Count { get; set; }
}