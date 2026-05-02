using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Domains.Services;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Biomes;

public class Detail(
    IWebHostEnvironment environment,
    IBiomeRepository biomeRepo,
    IFoodChainRepository foodChainRepo,
    ISpecieRecommendation recommendation,
    ILogger<Detail> logger)
    : AppPageModel(environment)
{
    [BindProperty] public BiomeSpecieInputModel Input { get; set; } = new();
    public BiomeView Biome { get; set; } = null!;
    public List<Recommendation<Specie>> Recommendations { get; set; } = null!;
    public BiomeAnalysis Analyze { get; set; } = null!;
    public string Creator { get; set; } = string.Empty;
    public bool ReopenAddSpecieDialog { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var loadResult = await LoadPageDataAsync(id);
        if (loadResult != null) return loadResult;

        Input = new BiomeSpecieInputModel();
        return Page();
    }

    public async Task<IActionResult> OnPostAddSpecieAsync(int id)
    {
        if (!ModelState.IsValid || Input.SpecieId is null || Input.Count is null)
        {
            var loadResult = await LoadPageDataAsync(id);
            ReopenAddSpecieDialog = true;
            if (loadResult != null) return loadResult;
            return Page();
        }

        logger.LogInformation("ADDING SPECIE");
        await biomeRepo.AddSpecie(new SpecieToBiomeRequest
        {
            SpecieId = (int)Input.SpecieId,
            Count = (int)Input.Count,
        }, id);
        
        SuccessNotification("Espèce ajoutée avec succès");
        return RedirectToPage(pageName: null, pageHandler: null, routeValues: new { id }, fragment: "title-section");
    }
    
    public async Task<IActionResult> OnPostRemoveSpecieAsync(int id)
    {
        if (!ModelState.IsValid || Input.SpecieId is null || Input.Count is null)
        {
            var loadResult = await LoadPageDataAsync(id);
            if (loadResult != null) return loadResult;
            return Page();
        }

        await biomeRepo.RemoveSpecie(new SpecieToBiomeRequest
        {
            SpecieId = (int)Input.SpecieId,
            Count    = (int)Input.Count,
        }, id);

        SuccessNotification("Espèce retirée avec succès");
        return RedirectToPage(pageName: null, pageHandler: null,
            routeValues: new { id }, fragment: "title-section");
    }

    public async Task<IActionResult> OnPostExecuteActionAsync(string actionId, int biomeId)
    {
        await recommendation.PerformAction(actionId);
        return RedirectToPage(pageName: null, pageHandler: null, routeValues: new { id = biomeId },
            fragment: "title-section");
    }

    private BiomeView BuildBiomeView(Biome b, string? userId)
    {
        var rule = new HumidityRule(b.Temperature, b.Humidity);

        var humidityPercent = rule.MaxHumidity > 0
            ? b.Humidity / rule.MaxHumidity * 100
            : 0;

        return new BiomeView
        {
            Id = b.Id,
            Name = b.Name,
            Temperature = b.Temperature,
            Humidity = b.Humidity,
            ImageUrl = !string.IsNullOrWhiteSpace(b.ImageUrl)
                ? UseUploadedFromId(b.ImageUrl)
                : "/img/defaults/biome_bg.png",
            CanEdit = userId is not null && (User.IsAdmin() || b.CreatedBy == User.GetUserId()),
            IsPublic = true,
            State = b.State,
            MaxHumidity = rule.MaxHumidity,
            HumidityPercent = Math.Min(humidityPercent, 100)
        };
    }

    private async Task<IActionResult?> LoadPageDataAsync(int id)
    {
        var userId = User.GetUserId();

        var biomeResult = await biomeRepo.FindAsync(id, userId, User.IsAdmin());
        if (!biomeResult.IsSuccess || biomeResult.Value is null)
            return biomeResult.ToPageResult(this);

        var creatorResult = await biomeRepo.GetCreatorAsync(biomeResult.Value.Id);
        if (!creatorResult.IsSuccess || creatorResult.Value is null)
            return creatorResult.ToPageResult(this);

        Creator = creatorResult.Value;
        Biome = BuildBiomeView(biomeResult.Value, userId);
        
        
        Analyze = await recommendation.AnalyzeBiome(userId, User.IsAdmin(), id);
        Recommendations = await recommendation.RecommendationToAddForBiome(userId, User.IsAdmin(), id, Analyze.Graph);
        return null;
    }
}