using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.SpeciesPages;

public class AddModel : AppPageModel
{
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IClassificationRepository _classificationRepository;

    public AddModel(
        ISpeciesRepository speciesRepository,
        IClassificationRepository classificationRepository,
        IWebHostEnvironment environment) : base(environment)
    {
        _speciesRepository = speciesRepository;
        _classificationRepository = classificationRepository;
    }

    public Specie Specie { get; set; }
    public List<string> Classifications { get; private set; } = [];

    [BindProperty] public SpeciesInputModel? Input { get; set; }

    public void OnGet()
    {
        LoadClassifications();
    }

    public async Task<IActionResult> OnPost()
    {
        LoadClassifications();

        if (Input == null || !ModelState.IsValid)
            return Page();

        try
        {
            var imageName = Input.Image == null ? null : await SaveImage(Input.Image);
            var specie = new Specie(Input.Name, Input.Type, Input.Diet, Input.Height, Input.Weight, imageName);

            _speciesRepository.Add(specie);
            SuccessNotification($"Espèce {Input.Name} correctement ajoutée.");
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
