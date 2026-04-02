using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.SpeciesPages;

public class EditModel : AppPageModel
{
    private readonly IRepository _repository;

    public EditModel(IRepository repository, IWebHostEnvironment environment) : base(environment)
    {
        _repository = repository;
    }

    public Species? Species { get; set; }

    [BindProperty] public SpeciesInputModel? Input { get; set; }
    public string? ImageUrl { get; set; }

    public IActionResult OnGet(string slug)
    {
        Species = _repository.FindBySlug(slug);

        if (Species == null)
            return NotFound();
        
        ImageUrl = Species.ImageUrl == null ? null : UseFileFromPath(Species.ImageUrl);

        Input = new SpeciesInputModel
        {
            Name = Species.Name,
            Type = Species.Type,
            Diet = Species.Diet,
            Height = Species.Height
        };

        return Page();
    }

    public async Task<IActionResult> OnPost(string slug, string action)
    {
        var species = _repository.FindBySlug(slug);

        if (species == null)
            return NotFound();

        if (action == "delete")
        {
            _repository.Delete(slug);

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
            species.Name = Input.Name;
            species.Type = Input.Type;
            species.Diet = Input.Diet;
            species.Height = Input.Height;

            if (Input.Image != null)
            {
                if (!string.IsNullOrEmpty(species.ImageUrl))
                {
                    DeleteImage(species.ImageUrl);
                }

                var imageName = await SaveImage(Input.Image);
                species.ImageUrl = imageName;
            }

            SuccessNotification($"Espèce {species.Name} correctement modifiée");
            return RedirectToPage("/SpeciesPages/Species");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Input.Name", ex.Message);
            return Page();
        }
    }
}