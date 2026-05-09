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
    [BindProperty(SupportsGet = true)] public int Id { get; set; }
    [BindProperty] public UpdateBiomeInputModel Input { get; set; } = new();

    public string CurrentImageUrl { get; set; } = null!;
    public string BiomeName { get; set; } = null!;
    public bool IsAdmin { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.GetUserId();
        if (userId is null) return Forbid();

        IsAdmin = User.IsAdmin();

        var result = await repository.FindAsync(Id, userId, IsAdmin);
        if (result.Failed) return result.ToPageResult(this);

        var biome = result.Value!;
        BiomeName = biome.Name;
        CurrentImageUrl = imageResolver.UseImageWithDefault(biome.ImageUrl, "dome.png");

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

        IsAdmin = User.IsAdmin();

        if (action == "delete")
        {
            var deleteResult = await repository.DeleteAsync(Id, userId, IsAdmin);
            if (deleteResult.Failed)
            {
                ErrorNotification("Erreur lors de la suppression du biome.");
                return deleteResult.ToPageResult(this);
            }

            var deleted = deleteResult.Value;
            if (deleted?.ImageUrl != null)
            {
                imageResolver.DeleteImage(deleted.ImageUrl, env);
            }

            SuccessNotification("Biome supprimé.");
            return RedirectToPage("/Biomes/Index");
        }

        if (!ModelState.IsValid)
        {
            var biomeForImage = await repository.FindAsync(Id, userId, IsAdmin);
            if (!biomeForImage.Failed)
            {
                CurrentImageUrl = imageResolver.UseImageWithDefault(biomeForImage.Value!.ImageUrl, "dome.png");
            }

            return Page();
        }

        var rule = new HumidityRule(Input.Temperature, Input.AbsoluteHumidity);
        if (!rule.IsValid)
        {
            ErrorNotification($"Humidité max pour {Input.Temperature}° : {rule.MaxHumidity:F2} g/m³.");
            var biomeForImage = await repository.FindAsync(Id, userId, IsAdmin);
            
            if (!biomeForImage.Failed)
            {
                CurrentImageUrl = imageResolver.UseImageWithDefault(biomeForImage.Value!.ImageUrl, "dome.png");
            }

            return Page();
        }

        var imageName = Input.Image is not null ? await imageResolver.SaveImage(Input.Image, env) : null;

        var request = new UpdateBiomeRequest
        {
            Id = Id,
            Name = Input.Name,
            Temperature = Input.Temperature,
            Humidity = Input.AbsoluteHumidity,
            ImageUrl = imageName,
            IsPublic = IsAdmin ? Input.IsPublic : null,
        };

        var result = await repository.UpdateAsync(request, userId, IsAdmin);

        if (result.Status == ResultStatus.Conflict)
        {
            ErrorNotification($"Un biome nommé '{Input.Name}' existe déjà.");
            return Page();
        }

        if (result.Failed)
        {
            ErrorNotification("Erreur lors de la modification du biome.");
            return Page();
        }

        SuccessNotification($"Biome '{Input.Name}' modifié avec succès.");
        return RedirectToPage("/Biomes/Index");
    }
}