using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Biomes;

public class Detail(IWebHostEnvironment environment, IBiomeRepository biomeRepo, IFoodChainRepository foodChainRepo)
    : AppPageModel(environment)
{
    public BiomeView Biome { get; set; } = null!;
    public FoodChainGraph FoodChain { get; set; } = null!;
    public string Creator { get; set; } = string.Empty;
    public AddBiomeSpecieInputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = User.GetUserId();

        var biomeResult = await biomeRepo.FindAsync(id, userId, User.IsAdmin());
        if (!biomeResult.IsSuccess || biomeResult.Value is null)
        {
            return biomeResult.ToPageResult(this);
        }

        var creatorResult = await biomeRepo.GetCreatorAsync(biomeResult.Value.Id);
        if (!creatorResult.IsSuccess || creatorResult.Value is null)
        {
            return creatorResult.ToPageResult(this);
        }


        Creator = creatorResult.Value;
        Biome = BuildBiomeView(biomeResult.Value, userId);

        var graphResult = await foodChainRepo.GetFullGraphForBiomeAsync(Biome.Id);
        if (!graphResult.IsSuccess || graphResult.Value is null)
        {
            return graphResult.ToPageResult(this);
        }

        FoodChain = graphResult.Value;

        Input = new AddBiomeSpecieInputModel();
        return Page();
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
}