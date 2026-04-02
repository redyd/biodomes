using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.SpeciesPages;

public class AddModel : AppPageModel
{
    private readonly IRepository _repository;

    public AddModel(IRepository repository, IWebHostEnvironment environment) : base(environment)
    {
        _repository = repository;
    }

    public Species Species { get; set; }

    [BindProperty] public SpeciesInputModel? Input { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            var imageName = Input?.Image == null ? null : await SaveImage(Input.Image);
            var specie = new Species(Input.Name, Input.Type, Input.Diet, Input.Height, imageName);

            _repository.Add(specie);
            SuccessNotification($"Espèce {Input.Name} correctement ajoutée.");
            return RedirectToPage("/SpeciesPages/Species");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Input.Name", ex.Message);
            return Page();
        }
    }
}