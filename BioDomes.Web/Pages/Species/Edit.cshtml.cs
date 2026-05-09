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
public class EditModel(
    ISpeciesRepository repository,
    ISpecieAttributesRepository attributesRepository,
    IImagePathResolver imageResolver,
    IWebHostEnvironment env) : AppPageModel
{
    [BindProperty] public SpeciesInputModel Input { get; set; } = new();

    public Specie Specie { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public bool IsAdmin { get; set; }

    public SelectList Classifications { get; set; } = null!;
    public SelectList Diets { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        var result = await repository.FindBySlugAsync(slug, userId, User.IsAdmin());
        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        var specie = result.Value!;

        if (!User.IsAdmin() && specie.CreatedBy != userId)
        {
            return Forbid();
        }

        IsAdmin = User.IsAdmin();
        Specie = specie;

        ImageUrl = imageResolver.UseImageWithDefault(specie.ImageUrl, "specie.png");

        Input = new SpeciesInputModel
        {
            Name = specie.Name,
            ClassificationId = specie.Classification.Id,
            DietId = specie.Diet.Id,
            Height = specie.Height,
            Weight = specie.Weight,
            Public = specie.IsPublic
        };
        
        var loaded = await LoadSelectLists();
        if (loaded is not null) return loaded;

        return Page();
    }

    public async Task<IActionResult> OnPost(string slug, string action)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        var result = await repository.FindBySlugAsync(slug, userId, User.IsAdmin());
        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        var specie = result.Value!;
        Specie = specie;

        return action switch
        {
            "delete" => await PerformDelete(userId, specie),
            "save" => await PerformUpdate(userId, specie),
            _ => Page()
        };
    }

    private async Task<IActionResult> PerformDelete(string userId, Specie specie)
    {
        await repository.DeleteAsync(specie.Id, userId, User.IsAdmin());

        if (!string.IsNullOrEmpty(specie.ImageUrl))
        {
            imageResolver.DeleteImage(specie.ImageUrl, env);
        }

        SuccessNotification($"Espèce {specie.Name} correctement supprimée");
        return RedirectToPage("/Species/Index");
    }

    private async Task<IActionResult> PerformUpdate(string userId, Specie specie)
    {
        if (!ModelState.IsValid)
        {
            var loaded = await LoadSelectLists();
            if (loaded is not null) return loaded;
            ImageUrl = imageResolver.UseImageWithDefault(specie.ImageUrl, "specie.png");

            return Page();
        }

        var updatedSpecie = new UpdateSpecieRequest
        {
            Id = specie.Id,
            Name = Input.Name,
            ClassificationId = (int)Input.ClassificationId!,
            DietId = (int)Input.DietId!,
            Height = Input.Height,
            Weight = Input.Weight
        };

        if (User.IsAdmin())
        {
            updatedSpecie.IsPublic = Input.Public;
        }

        if (Input.Image != null)
        {
            if (!string.IsNullOrEmpty(specie.ImageUrl))
            {
                imageResolver.DeleteImage(specie.ImageUrl, env);
            }

            var imageName = await imageResolver.SaveImage(Input.Image, env);
            updatedSpecie.ImageUrl = imageName;
        }

        var result = await repository.UpdateAsync(updatedSpecie, userId, User.IsAdmin());
        if (result.Status == ResultStatus.Conflict)
        {
            ErrorNotification("Une espèce avec ce nom existe déjà");
            
            var loaded = await LoadSelectLists();
            if (loaded is not null) return loaded;
            ImageUrl = imageResolver.UseImageWithDefault(specie.ImageUrl, "specie.png");
            
            return Page();
        }

        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        var updated = result.Value!;

        SuccessNotification($"Espèce {updated.Name} correctement modifiée");
        return RedirectToPage("/Species/Index");
    }
    
    private async Task<IActionResult?> LoadSelectLists()
    {
        var classificationsResult = await attributesRepository.GetClassifications();
        if (classificationsResult.Failed) return classificationsResult.ToPageResult(this);

        var dietsResult = await attributesRepository.GetDiets();
        if (dietsResult.Failed) return dietsResult.ToPageResult(this);

        Classifications = new SelectList(classificationsResult.Value!, "Id", "Label", Input.ClassificationId);
        Diets = new SelectList(dietsResult.Value!, "Id", "Label", Input.DietId);

        return null;
    }
}