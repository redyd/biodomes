using BioDomes.Infrastructures.Models;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BioDomes.Web.Pages.Biomes;

[Authorize]
public class Add(IWebHostEnvironment environment) : AppPageModel(environment)
{
    [BindProperty]
    public InputModel Input { get; set; } = new();
    
    public string? Humidity { get; set; }
    
    public string BiomeState { get; set; } = string.Empty;

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom du biome est requis")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La température est requise.")]
        public double? Temperature { get; set; }

        [Required(ErrorMessage = "L'humidité absolue est requise.")]
        [Range(0, double.MaxValue, ErrorMessage = "L'humidité doit être positive.")]
        public double? AbsoluteHumidity { get; set; }
    }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        var temp = Input.Temperature!.Value;
        var humidity = Input.AbsoluteHumidity!.Value;

        var maxHumidity = CalculateMaxHumidity(temp);

        if (humidity > maxHumidity)
        {
            ErrorNotification($"Un biome avec une température de {temp}° a une humidité max de {maxHumidity:F2} g/m³.");
            Humidity = $"Humidité maximale autorisée pour {temp}° : {maxHumidity:F2} g/m³";
            return Page();
        }

        var state = CalculateBiomeState(temp);

        // TODO: sauvegarder en db

        SuccessNotification($"Biome créé avec succès ! État : {state}.");
        return RedirectToPage("/Biomes/Index");
    }

    private static double CalculateMaxHumidity(double temperature) =>
        216.7 * 6.112 * Math.Exp(17.67 * temperature / (temperature + 243.5)) / (temperature + 273.15);

    private static string CalculateBiomeState(double temperature) => temperature switch
    {
        < -20 or > 40 => "Critique",
        < 0 or > 30   => "Instable",
        _              => "Optimal"
    };
}