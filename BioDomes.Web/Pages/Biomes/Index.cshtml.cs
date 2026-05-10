using System.Text.Json;
using BioDomes.Domains.Core;
using BioDomes.Domains.Services;
using BioDomes.Web.Extensions;
using BioDomes.Web.Mapper;
using BioDomes.Web.PageModels;
using BioDomes.Web.Services;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Biomes;

public class BiomePage(
    IBiomeViewMapper mapper,
    IImagePathResolver imageResolver,
    IBiomeAnalyzer analyzer)
    : AppPageModel
{
    public Dictionary<BiomeView<ComplexBiome>, BiomeStats> BiomesStats { get; set; } = [];
    public bool IsUserConnected { get; set; }
    public string BiomesJson { get; set; } = "[]";

    public async Task<IActionResult> OnGetAsync()
    {
        IsUserConnected = User.Identity?.IsAuthenticated ?? false;

        BiomesStats = (await analyzer.AnalyzeAllBiomes(User.GetUserId(), User.IsAdmin()))
            .Select(kvp => (
                view: mapper.MapSingle(
                    kvp.Key,
                    User.IsAdmin(),
                    User.GetUserId(),
                    b => imageResolver.UseImageWithDefault(b, "dome.png")),
                stats: kvp.Value
            ))
            .ToDictionary(x => x.view, x => x.stats);

        var items = BiomesStats.Keys.Select(b => new
        {
            slug = b.Model.Slug,
            name = b.Model.Name,
            temperature = b.Model.Temperature,
            humidity = b.Model.Humidity,
            isPublic = b.Model.IsPublic,
            state = b.Model.State.ToString(),
            species = b.Model.Species.Select(s => s.Model.Name).ToList(),
            equipments = b.Model.Equipments.Select(e => e.Model.Name).ToList(),
            allSpeciesFed = BiomesStats.FirstOrDefault(kvp => kvp.Key.Model == b.Model).Value?.AllSpeciesFed ?? false,
        }).ToList();
        
        BiomesJson = JsonSerializer.Serialize(items);

        return Page();
    }
}