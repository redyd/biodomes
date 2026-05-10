using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Stats;

[Authorize]
public class StatsModel(IBiomeStatRepository repo) : PageModel
{
    public bool HasBiomes { get; set; }
    
    public async Task<IActionResult> OnGetStatsDataAsync()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");

        var statsResults = await repo.GetStatsAsync(userId, isAdmin);
        if (statsResults.Failed) return statsResults.ToPageResult(this);
        
        return new JsonResult(statsResults.Value!);
    }
    
    public async Task<IActionResult> OnGetHistoryDataAsync(string? biomeSlug, int days = 30)
    {
        var userId  = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");

        var historyResults = await repo.GetHistoryAsync(biomeSlug, userId, isAdmin, days);
        if (historyResults.Failed) return historyResults.ToPageResult(this);
        
        return new JsonResult(historyResults.Value!);
    }

    public async Task<IActionResult> OnGet()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");

        var statsResults = await repo.GetStatsAsync(userId, isAdmin);
        if (statsResults.Failed) return statsResults.ToPageResult(this);
        
        HasBiomes = statsResults.Value!.Count > 0;
        return Page();
    }
}