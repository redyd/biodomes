using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Web.Extensions;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class IndexModel(
    IEquipmentRepository equipmentRepo,
    ISpeciesRepository speciesRepo,
    IBiomeAnalyzer analyzer)
    : PageModel
{
    public List<BiomeView<ComplexBiome>> RankedBiomes { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var analyze = await analyzer.GlobalAnalyze(User.GetUserId(), User.IsAdmin());
        return Page();
    }
}