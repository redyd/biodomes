using BioDomes.Domains.Core;
using BioDomes.Domains.Services;
using BioDomes.Web.Extensions;
using BioDomes.Web.Mapper;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages;

public class IndexModel(
    IWebHostEnvironment env,
    IBiomeViewMapper mapper,
    IBiomeAnalyzer analyzer)
    : AppPageModel(env)
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
            image => UseImageWithDefault(image, "dome.png"));
        
        return Page();
    }
}