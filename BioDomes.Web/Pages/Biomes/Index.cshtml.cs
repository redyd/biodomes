using System.Security.Claims;
using System.Text.Json;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using BioDomes.Web.Mapper;
using BioDomes.Web.PageModels;
using BioDomes.Web.Services;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Biomes;

public class BiomePage(IBiomeRepository repository, IBiomeViewMapper mapper, IImagePathResolver imageResolver)
    : AppPageModel
{
    public List<BiomeView<Biome>> Biomes { get; set; } = [];
    public bool IsUserConnected { get; set; }
    public string BiomesJson { get; set; } = "[]";

    public async Task<IActionResult> OnGetAsync()
    {
        IsUserConnected = User.Identity?.IsAuthenticated ?? false;

        var biomesResult = await repository.GetAllAsync(User.GetUserId(), User.IsAdmin());
        if (biomesResult.Failed) return biomesResult.ToPageResult(this);

        Biomes = mapper.MapAll(
            biomesResult.Value!,
            User.IsAdmin(),
            User.GetUserId(),
            b => imageResolver.UseImageWithDefault(b, "dome.png"));

        if (!IsUserConnected) return Page();
        
        var complexResult = await repository.GetAllComplexAsync(User.GetUserId(), User.IsAdmin());
        if (complexResult.Failed) return Page();
        var items = complexResult.Value!.Select(b => new
        {
            id = b.Id,
            name = b.Name,
            temperature = b.Temperature,
            humidity = b.Humidity,
            isPublic = b.IsPublic,
            state = b.State.ToString(),
            species = b.Species.Select(s => s.Model.Name).ToList(),
            equipments = b.Equipments.Select(e => e.Model.Name).ToList(),
        }).ToList();
        BiomesJson = JsonSerializer.Serialize(items);
        
        return Page();
    }
}