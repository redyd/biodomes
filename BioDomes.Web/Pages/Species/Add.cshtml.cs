using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using BioDomes.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BioDomes.Web.Pages.Species;

[Authorize]
public class AddModel(
    ISpeciesRepository speciesRepository,
    ISpecieAttributesRepository attributesRepository,
    IImagePathResolver imageResolver,
    IWebHostEnvironment env) : AppPageModel
{
    [BindProperty] public SpeciesInputModel Input { get; set; } = new();

    public SelectList Classifications { get; set; } = null!;
    public SelectList Diets { get; set; } = null!;

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
        if (!ModelState.IsValid)
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

        var imageName = Input.Image == null ? null : await imageResolver.SaveImage(Input.Image, env);
        var specie = new CreateSpecieRequest
        {
            Name = Input.Name,
            Image = imageName,
            Height = Input.Height,
            Weight = Input.Weight,
            DietSlug = Input.DietSlug,
            ClassificationSlug = Input.ClassificationSlug
        };

        var result = await speciesRepository.AddAsync(specie, userId);
        if (result.Status == ResultStatus.Conflict)
        {
            ErrorNotification("Une espèce avec ce nom existe déjà");
            if (imageName is not null)
            {
                imageResolver.DeleteImage(imageName, env);
            }

            var loaded = await LoadSelectLists();
            if (loaded is not null) return loaded;

            return Page();
        }

        if (result.Failed)
        {
            if (imageName is not null)
            {
                imageResolver.DeleteImage(imageName, env);
            }

            return result.ToPageResult(this);
        }

        SuccessNotification($"Espèce {Input.Name} correctement ajoutée.");
        return RedirectToPage("/Species/Index");
    }

    private async Task<IActionResult?> LoadSelectLists()
    {
        var classifications = await attributesRepository.GetClassifications();
        var diets = await attributesRepository.GetDiets();

        if (classifications.Failed) return classifications.ToPageResult(this);
        if (diets.Failed) return diets.ToPageResult(this);

        Classifications = new SelectList(classifications.Value!, "Slug", "Label", Input.ClassificationSlug);
        Diets = new SelectList(diets.Value!, "Slug", "Label", Input.DietSlug);

        return null;
    }
}