using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Species;

public class AddModel(ISpeciesRepository speciesRepository, IWebHostEnvironment environment) : AppPageModel(environment)
{
    [BindProperty] public SpeciesInputModel? Input { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (Input == null || !ModelState.IsValid)
            return Page();

        try
        {
            var imageName = Input.Image == null ? null : await SaveImage(Input.Image);
            var specie = new CreateSpecieRequest
            {
                Name = Input.Name,
                Diet = Input.Diet,
                Image = imageName,
                Height = Input.Height,
                Weight = Input.Weight,
                Type = Input.Type
            };

            var result = await speciesRepository.AddAsync(specie);
            if (result.Status == ResultStatus.Conflict)
            {
                ErrorNotification("Une espèce avec ce nom existe déjà");
                return Page();
            }

            if (!result.IsSuccess)
            {
                return result.ToPageResult(this);
            }
            
            SuccessNotification($"Espèce {Input.Name} correctement ajoutée.");
            return RedirectToPage("/Species/Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Input.Name", ex.Message);
            return Page();
        }
    }
}