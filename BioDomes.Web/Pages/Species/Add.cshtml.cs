using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BioDomes.Web.Pages.Species;

[Authorize]
public class AddModel(
    ISpeciesRepository speciesRepository,
    ISpecieAttributesRepository attributesRepository,
    IWebHostEnvironment env) : AppPageModel(env)
{
    [BindProperty] public SpeciesInputModel? Input { get; set; }

    public SelectList Classifications { get; set; } = null!;
    public SelectList Diets { get; set; } = null!;

    private async Task<IActionResult?> LoadSelectLists()
    {
        var classifications = await attributesRepository.GetClassifications();
        var diets = await attributesRepository.GetDiets();
        
        if (classifications.Failed) return classifications.ToPageResult(this);
        if (diets.Failed) return diets.ToPageResult(this);

        Classifications = new SelectList(classifications.Value!, "Id", "Label", Input?.ClassificationId);
        Diets = new SelectList(diets.Value!, "Id", "Label", Input?.DietId);
        
        return null;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        var loaded = await LoadSelectLists();
        if (loaded is not null) return loaded;
        
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (Input == null || !ModelState.IsValid)
        {
            var loaded = await LoadSelectLists();
            if (loaded is not null) return loaded;
            return Page();
        }

        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        try
        {
            var imageName = Input.Image == null ? null : await SaveImage(Input.Image);
            var specie = new CreateSpecieRequest
            {
                Name = Input.Name,
                Image = imageName,
                Height = Input.Height,
                Weight = Input.Weight,
                DietId = Input.DietId,
                ClassificationId = Input.ClassificationId
            };

            var result = await speciesRepository.AddAsync(specie, userId);
            if (result.Status == ResultStatus.Conflict)
            {
                ErrorNotification("Une espèce avec ce nom existe déjà");
                if (imageName is not null)
                {
                    DeleteImage(imageName);
                }
                var loaded = await LoadSelectLists();
                if (loaded is not null) return loaded;
                
                return Page();
            }

            if (result.Failed)
            {
                return result.ToPageResult(this);
            }

            SuccessNotification($"Espèce {Input.Name} correctement ajoutée.");
            return RedirectToPage("/Species/Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Input.Name", ex.Message);
            var loaded = await LoadSelectLists();
            if (loaded is not null) return loaded;
            return Page();
        }
    }
}