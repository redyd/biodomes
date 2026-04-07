using BioDomes.Domains.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ISpeciesRepository _repo;

    public IndexModel(ILogger<IndexModel> logger, ISpeciesRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public int SpeciesCount { get; set; }

    public async Task OnGetAsync()
    {
        SpeciesCount = await _repo.CountAsync();
    }
}