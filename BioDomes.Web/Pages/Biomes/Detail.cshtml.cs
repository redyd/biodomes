using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Biomes;

public class Detail(IWebHostEnvironment environment, IBiomeRepository biomeRepo, ILogger<Detail> logger)
    : AppPageModel(environment)
{
    public BiomeView Biome { get; set; } = null!;
    public string Creator { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = User.GetUserId();
        var result = await biomeRepo.FindAsync(id, userId, User.IsAdmin());

        if (!result.IsSuccess || result.Value is null)
        {
            return result.ToPageResult(this);
        }

        var b = result.Value;

        var creator = await biomeRepo.GetCreatorAsync(b.Id);

        if (!creator.IsSuccess || creator.Value is null)
        {
            return creator.ToPageResult(this);
        }
        
        Creator = creator.Value;

        var rule = new HumidityRule(b.Temperature, b.Humidity);

        var humidityPercent = rule.MaxHumidity > 0
            ? (b.Humidity / rule.MaxHumidity) * 100
            : 0;


        Biome = new BiomeView
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

        return Page();
    }
}