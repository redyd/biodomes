using BioDomes.Domains.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Stats;

[Authorize]
public class StatsModel : PageModel
{
    private readonly IBiomeRepository _biomeRepository;

    public StatsModel(IBiomeRepository biomeRepository)
    {
        _biomeRepository = biomeRepository;
    }

    public async Task<IActionResult> OnGetStatsDataAsync()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");

        var stats = await _biomeRepository.GetStatsAsync(userId, isAdmin);
        return new JsonResult(stats);
    }

    public IActionResult OnGet()
    {
        return Page();
    }
}