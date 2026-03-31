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
    
    [BindProperty]
    public SpeciesInputModel? Input  { get; set; }

    public IActionResult OnGet(string slug)
    {
        Species = _repository.FindBySlug(slug);

        if (Species == null)
            return NotFound();

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
        Species = _repository.FindBySlug(slug);

        if (Input == null || !ModelState.IsValid || Species == null)
        {
            return Page();
        }
        
        if (action == "delete")
        {
            _repository.Delete(slug);
            if (Species.ImageUrl != null) DeleteImage(Species.ImageUrl);
            SuccessNotification($"Espèce {Species.Name} correctement supprimée");

            return RedirectToPage("/SpeciesPages/Species");
        }

        var imageName = Input?.Image != null ? await SaveImage(Input.Image) : null;

        try
        {
            _repository.Update(slug, new SpeciesData
            {
                Name = Input.Name,
                Type = Input.Type,
                Diet = Input.Diet,
                Height = Input.Height,
            });

            if (imageName != null)
            {
                _repository.UpdateImage(slug, imageName);
            }

            SuccessNotification($"Espèce {Input.Name} correctement modifiée");
            return RedirectToPage("/SpeciesPages/Species");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Input.Name", ex.Message);
            Species = _repository.FindBySlug(slug);
            return Page();
        }
    }
}