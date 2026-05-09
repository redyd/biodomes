using BioDomes.Domains.Core;
using BioDomes.Domains.Services;
using BioDomes.Web.Extensions;
using BioDomes.Web.Mapper;
using BioDomes.Web.Services;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class IndexModel(IBiomeViewMapper mapper, IBiomeAnalyzer analyzer, IImagePathResolver imagePathResolver)
    : PageModel
{
    public GlobalStatistic Statistic { get; set; } = null!;
    public List<BiomeView<ComplexBiome>> RankedBiomes { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var analyze = await analyzer.GlobalAnalyze(User.GetUserId(), User.IsAdmin());

        Statistic = analyze;
        RankedBiomes = mapper.MapAll(
            analyze.GetBalancedBiome(3),
            User.IsAdmin(),
            User.GetUserId(),
            image => imagePathResolver.UseImageWithDefault(image, "dome.png"));

        return Page();
    }
}