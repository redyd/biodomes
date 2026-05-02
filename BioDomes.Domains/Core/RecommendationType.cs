using System.ComponentModel.DataAnnotations;

namespace BioDomes.Domains.Core;

public enum RecommendationType
{
    [Display(Name = "Excellent")]
    Excellent,
    [Display(Name = "Bien")]
    Good,
    [Display(Name = "Moyen")]
    Ok,
    [Display(Name = "Mauvais")]
    Bad
}