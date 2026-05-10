using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BioDomes.Domains.Core;
using BioDomes.Web.InputModel;
using BioDomes.Web.Services;

namespace BioDomes.Web.Pages.Biomes;

[Authorize]
public class EditPage(IBiomeRepository repository, IWebHostEnvironment env, IImagePathResolver imageResolver) : AppPageModel
{
    [BindProperty(SupportsGet = true)] public string Slug { get; set; } = null!;
    [BindProperty] public UpdateBiomeInputModel Input { get; set; } = new();

    public string CurrentImageUrl { get; set; } = null!;
    public string BiomeName { get; set; } = null!;
    public bool IsAdmin { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.GetUserId();
        if (userId is null) return Forbid();

        var result = await repository.FindAsync(Slug, userId, User.IsAdmin());
        if (result.Failed) return result.ToPageResult(this);

        var biome = result.Value!;

        LoadPageData(biome);

        Input = new UpdateBiomeInputModel
        {
            Name = biome.Name,
            Temperature = biome.Temperature,
            AbsoluteHumidity = biome.Humidity,
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string action)
    {
        var userId = User.GetUserId();
        if (userId is null) return Forbid();

        var biomeResult = await repository.FindAsync(Slug, userId, User.IsAdmin());
        if (biomeResult.Failed) return biomeResult.ToPageResult(this);

        var biome = biomeResult.Value!;

        return action switch
        {
            "delete" => await PerformDelete(userId, biome),
            "save" => await PerformUpdate(userId, biome),
            _ => Page()
        };
    }

    private async Task<IActionResult> PerformDelete(string userId, Biome biome)
    {
        var result = await repository.DeleteAsync(Slug, userId, User.IsAdmin());
        
        if (result.Failed)
        {
            ErrorNotification("Erreur lors de la suppression du biome.");
            return result.ToPageResult(this);
        }

        if (biome.ImageUrl != null)
        {
            imageResolver.DeleteImage(biome.ImageUrl, env);
        }

        SuccessNotification($"Biome '{biome.Name}' supprimé.");
        return RedirectToPage("/Biomes/Index");
    }

    private async Task<IActionResult> PerformUpdate(string userId, Biome biome)
    {
        if (!ModelState.IsValid)
        {
            LoadPageData(biome);
            return Page();
        }

        var rule = new HumidityRule(Input.Temperature, Input.AbsoluteHumidity);
        if (!rule.IsValid)
        {
            ErrorNotification($"Humidité max pour {Input.Temperature}° : {rule.MaxHumidity:F2} g/m³.");
            LoadPageData(biome);
            return Page();
        }

        var imageName = Input.Image is not null
            ? await imageResolver.SaveImage(Input.Image, env)
            : null;

        var request = new UpdateBiomeRequest
        {
            PreviousSlug = Slug,
            Name = Input.Name,
            Temperature = Input.Temperature,
            Humidity = Input.AbsoluteHumidity,
            ImageUrl = imageName,
            IsPublic = User.IsAdmin() ? Input.IsPublic : null,
        };

        var result = await repository.UpdateAsync(request, userId, User.IsAdmin());

        if (result.Status == ResultStatus.Conflict)
        {
            ErrorNotification($"Un biome nommé '{Input.Name}' existe déjà.");
            LoadPageData(biome);
            return Page();
        }

        if (result.Failed)
        {
            ErrorNotification("Erreur lors de la modification du biome.");
            LoadPageData(biome);
            return Page();
        }

        SuccessNotification($"Biome '{Input.Name}' modifié avec succès.");
        return RedirectToPage("/Biomes/Index");
    }

    private void LoadPageData(Biome biome)
    {
        IsAdmin = User.IsAdmin();
        BiomeName = biome.Name;
        CurrentImageUrl = imageResolver.UseImageWithDefault(biome.ImageUrl, "dome.png");
    }
}