using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;

namespace BioDomes.Web.Pages.Biomes;

[Authorize]
public class AddPage(IWebHostEnvironment env, IBiomeRepository repository) : AppPageModel(env)
{
    [BindProperty]
    public BiomeInputModel Input { get; set; } = new();
    
    public string? Humidity { get; set; }
    

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var temp = Input.Temperature;
        var humidity = Input.AbsoluteHumidity;
        var rule = new HumidityRule(temp, humidity);

        if (!rule.IsValid)
        {
            ErrorNotification($"Un biome avec une température de {temp}° a une humidité max de {rule.MaxHumidity:F2} g/m³.");
            Humidity = $"Humidité maximale autorisée pour {temp}° : {rule.MaxHumidity:F2} g/m³";
            return Page();
        }

        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }
        
        var imageName = Input.ImageUrl == null ? null : await SaveImage(Input.ImageUrl);

        var createRequest = new CreateBiomeRequest
        {
            Name = Input.Name,
            Temperature = Input.Temperature,
            Humidity = Input.AbsoluteHumidity,
            ImageUrl = imageName,
            IsPublic = true
        };
        
        var result = await repository.CreateBiomeAsync(createRequest, userId);
        if (result.Failed)
        {
            ErrorNotification($"Erreur dans la création du biome");
            return Page();
        }
        
        var value = result.Value!;
        
        SuccessNotification($"Biome créé avec succès ! État : {value.State}.");
        return RedirectToPage("/Biomes/Index");
    }
}