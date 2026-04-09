using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Species;

public class EditModel : AppPageModel
{
    private readonly ISpeciesRepository _speciesRepository;

    public EditModel(
        ISpeciesRepository speciesRepository,
        IWebHostEnvironment environment) : base(environment)
    {
        _speciesRepository = speciesRepository;
    }

    public Specie? Species { get; set; }

    [BindProperty] public SpeciesInputModel? Input { get; set; }
    public string? ImageUrl { get; set; }

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        Species = await _speciesRepository.FindBySlugAsync(slug);

        if (Species == null)
            return NotFound();
        
        ImageUrl = Species.ImageUrl == null ? null : UseUploadedFromId(Species.ImageUrl);

        Input = new SpeciesInputModel
        {
            Name = Species.Name,
            Type = Species.Type,
            Diet = Species.Diet,
            Height = Species.Height,
            Weight = Species.Weight
        };

        return Page();
    }

    public async Task<IActionResult> OnPost(string slug, string action)
    {
        var species = await _speciesRepository.FindBySlugAsync(slug);

        if (species == null)
            return NotFound();

        if (action == "delete")
        {
            await _speciesRepository.DeleteAsync(slug);

            if (!string.IsNullOrEmpty(species.ImageUrl))
            {
                DeleteImage(species.ImageUrl);
            }

            SuccessNotification($"Espèce {species.Name} correctement supprimée");
            return RedirectToPage("/Species/Index");
        }

        if (Input == null || !ModelState.IsValid)
            return Page();

        try
        {
            var updatedSpecie = new Specie(
                Input.Name,
                Input.Type,
                Input.Diet,
                Input.Height,
                Input.Weight,
                species.ImageUrl);

            if (Input.Image != null)
            {
                if (!string.IsNullOrEmpty(species.ImageUrl))
                {
                    DeleteImage(species.ImageUrl);
                }

                var imageName = await SaveImage(Input.Image);
                updatedSpecie.ImageUrl = imageName;
            }

            var saved = await _speciesRepository.UpdateAsync(slug, updatedSpecie);
            if (saved == null)
                return NotFound();

            SuccessNotification($"Espèce {saved.Name} correctement modifiée");
            return RedirectToPage("/Species/Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Input.Name", ex.Message);
            return Page();
        }
    }
}
