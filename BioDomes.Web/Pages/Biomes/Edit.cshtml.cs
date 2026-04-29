using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BioDomes.Domains.Core;

namespace BioDomes.Web.Pages.Biomes;

[Authorize]
public class EditPage(IBiomeRepository repository, IWebHostEnvironment environment) : AppPageModel(environment)
{
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? CurrentImageUrl { get; set; }
    public string? BiomeName { get; set; }
    public bool IsAdmin { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Le nom du biome est requis")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La température est requise.")]
        public double? Temperature { get; set; }

        [Required(ErrorMessage = "L'humidité absolue est requise.")]
        [Range(0, double.MaxValue, ErrorMessage = "L'humidité doit être positive.")]
        public double? AbsoluteHumidity { get; set; }

        public IFormFile? Image { get; set; }
        public bool? IsPublic { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.GetUserId();
        if (userId is null) return Forbid();

        IsAdmin = User.IsAdmin();

        var result = await repository.FindAsync(Id, userId, IsAdmin);
        if (!result.IsSuccess)
            return result.ToPageResult(this);

        var biome = result.Value!;
        BiomeName = biome.Name;
        CurrentImageUrl = SafeImageUrl(biome.ImageUrl);
        Input = new InputModel
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
            if (!deleteResult.IsSuccess)
            {
                ErrorNotification("Erreur lors de la suppression du biome.");
                return Page();
            }

            SuccessNotification("Biome supprimé.");
            return RedirectToPage("/Biomes/Index");
        }

        if (!ModelState.IsValid)
        {
            var biomeForImage = await repository.FindAsync(Id, userId, IsAdmin);
            if (biomeForImage.IsSuccess)
                CurrentImageUrl = SafeImageUrl(biomeForImage.Value!.ImageUrl);
            return Page();
        }

        var maxHumidity = CalculateMaxHumidity(Input.Temperature!.Value);
        if (Input.AbsoluteHumidity!.Value > maxHumidity)
        {
            ErrorNotification($"Humidité max pour {Input.Temperature}° : {maxHumidity:F2} g/m³.");
            var biomeForImage = await repository.FindAsync(Id, userId, IsAdmin);
            if (biomeForImage.IsSuccess)
                CurrentImageUrl = SafeImageUrl(biomeForImage.Value!.ImageUrl);
            return Page();
        }

        var imageName = Input.Image is not null ? await SaveImage(Input.Image) : null;

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

        if (!result.IsSuccess)
        {
            ErrorNotification("Erreur lors de la modification du biome.");
            return Page();
        }

        SuccessNotification($"Biome '{Input.Name}' modifié avec succès.");
        return RedirectToPage("/Biomes/Index");
    }

    private static double CalculateMaxHumidity(double temperature) =>
        216.7 * 6.112 * Math.Exp(17.67 * temperature / (temperature + 243.5)) / (temperature + 273.15);
    
    private string? SafeImageUrl(string? imageUrl) =>
        string.IsNullOrWhiteSpace(imageUrl) ? null : UseUploadedFromId(imageUrl);
}