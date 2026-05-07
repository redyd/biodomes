using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using BioDomes.Web.Mapper;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Biomes;

public class BiomePage(IWebHostEnvironment env, IBiomeRepository repository, IBiomeViewMapper mapper)
    : AppPageModel(env)
{
    public List<BiomeView<Biome>> Biomes { get; set; } = [];
    public bool IsUserConnected { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var biomesResult = await repository.GetAllAsync(User.GetUserId(), User.IsAdmin());
        if (biomesResult.Failed) return biomesResult.ToPageResult(this);
        
        Biomes = mapper.MapAll(
            biomesResult.Value!,
            User.IsAdmin(),
            User.GetUserId(),
            b => UseImageWithDefault(b, "dome.png"));
        
        return Page();
    }
}