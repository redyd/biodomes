using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.SpeciesPages;

public class EditModel : AppPageModel
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IClassificationRepository _classificationRepository;

    public EditModel(
        ISpeciesRepository speciesRepository,
        IClassificationRepository classificationRepository,
        IWebHostEnvironment environment) : base(environment)
    {
        _speciesRepository = speciesRepository;
        _classificationRepository = classificationRepository;
    }

    public Specie? Species { get; set; }
    public List<string> Classifications { get; private set; } = [];

    [BindProperty] public SpeciesInputModel? Input { get; set; }
    public string? ImageUrl { get; set; }

    public IActionResult OnGet(string slug)
    {
        Species = _speciesRepository.FindBySlug(slug);

        if (Species == null)
            return NotFound();
        
        LoadClassifications();
        ImageUrl = Species.ImageUrl == null ? null : UseFileFromPath(Species.ImageUrl);

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
        var species = _speciesRepository.FindBySlug(slug);
        LoadClassifications();

        if (species == null)
            return NotFound();

        if (action == "delete")
        {
            _speciesRepository.Delete(slug);

            if (!string.IsNullOrEmpty(species.ImageUrl))
            {
                DeleteImage(species.ImageUrl);
            }

            SuccessNotification($"Espèce {species.Name} correctement supprimée");
            return RedirectToPage("/SpeciesPages/Species");
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

            var saved = _speciesRepository.Update(slug, updatedSpecie);
            if (saved == null)
                return NotFound();

            SuccessNotification($"Espèce {saved.Name} correctement modifiée");
            return RedirectToPage("/SpeciesPages/Species");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Input.Name", ex.Message);
            return Page();
        }
    }

    private void LoadClassifications()
    {
        Classifications = _classificationRepository.FetchAll();
    }
}
