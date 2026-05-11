using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.Services;

namespace BioDomes.Web.Pages.Biomes;

[Authorize]
[ValidateAntiForgeryToken]
public class AddPage(IWebHostEnvironment env, IImagePathResolver imageResolver, IBiomeRepository repository)
    : AppPageModel
{
    [BindProperty] public BiomeInputModel Input { get; set; } = new();
    public string? Humidity { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var temp = Input.Temperature;
        var humidity = Input.AbsoluteHumidity;
        var rule = new HumidityRule(temp, humidity);

        if (!rule.IsValid)
        {
            ErrorNotification(
                $"Un biome avec une température de {temp}° a une humidité max de {rule.MaxHumidity:F2} g/m³.");
            Humidity = $"Humidité maximale autorisée pour {temp}° : {rule.MaxHumidity:F2} g/m³";
            return Page();
        }

        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        var imageName = Input.ImageUrl == null ? null : await imageResolver.SaveImage(Input.ImageUrl, env);

        var createRequest = new CreateBiomeRequest
        {
            Name = Input.Name,
            Temperature = Input.Temperature,
            Humidity = Input.AbsoluteHumidity,
            ImageUrl = imageName,
            IsPublic = true
        };

        var result = await repository.CreateBiomeAsync(createRequest, userId);

        if (result.Status == ResultStatus.Conflict)
        {
            ErrorNotification("Un biome de ce nom existe déjà");
            return Page();
        }

        if (result.Failed)
        {
            ErrorNotification("Erreur dans la création du biome");
            return result.ToPageResult(this);
        }

        var value = result.Value!;

        SuccessNotification($"Biome créé avec succès ! État : {value.State}.");
        return RedirectToPage("/Biomes/Index");
    }
}