using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class IndexModel(ILogger<IndexModel> logger, ISpeciesRepository specieRepo, IEquipmentRepository equipmentRepos)
    : PageModel
{
    public int SpeciesCount { get; set; }
    public int EquipmentCount { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var countSpecieResult = await specieRepo.CountAsync();
        var countEquipmentResult = await equipmentRepos.CountAsync();
        if (!countSpecieResult.IsSuccess || !countEquipmentResult.IsSuccess)
        {
            return countSpecieResult.ToPageResult(this);
        }

        SpeciesCount = countSpecieResult.Value;
        EquipmentCount = countEquipmentResult.Value;
        
        return Page();
    }
}