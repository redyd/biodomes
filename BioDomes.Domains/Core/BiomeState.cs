using System.ComponentModel.DataAnnotations;

namespace BioDomes.Domains.Core;

public enum BiomeState
{
    [Display(Name = "Critique")]
    Critic,
    [Display(Name = "Instable")]
    Instable,
    [Display(Name = "Optimal")]
    Optimal
}