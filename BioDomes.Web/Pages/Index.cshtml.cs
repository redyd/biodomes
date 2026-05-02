using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages;

public class IndexModel(
    ILogger<IndexModel> logger,
    ISpeciesRepository specieRepo,
    IEquipmentRepository equipmentRepo,
    IBiomeRepository biomeRepo)
    : PageModel
{
    public int SpeciesCount { get; set; }
    public int EquipmentCount { get; set; }
    public int BiomeCount { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var countSpecieResult = await specieRepo.CountAsync();
        var countEquipmentResult = await equipmentRepo.CountAsync();
        var countBiomeResult = await biomeRepo.CountAsync();

        if (countSpecieResult.Failed)
        {
            return countSpecieResult.ToPageResult(this);
        }

        if (countEquipmentResult.Failed)
        {
            return countEquipmentResult.ToPageResult(this);
        }

        if (countBiomeResult.Failed)
        {
            return countBiomeResult.ToPageResult(this);
        }
        
        SpeciesCount = countSpecieResult.Value;
        EquipmentCount = countEquipmentResult.Value;
        BiomeCount = countBiomeResult.Value;

        return Page();
    }
}