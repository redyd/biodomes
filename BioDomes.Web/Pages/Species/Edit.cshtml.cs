using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Species;

public class EditModel(
    ISpeciesRepository repository,
    ISpecieAttributesRepository attributesRepository,
    IWebHostEnvironment environment) : AppPageModel(environment)
{
    [BindProperty] public SpeciesInputModel Input { get; set; } = new();

    public Specie? Specie { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsAdmin { get; set; }

    public List<Classification> Classifications { get; set; } = [];
    public List<Diet> Diets { get; set; } = [];

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

        Classifications = await attributesRepository.GetClassifications();
        Diets = await attributesRepository.GetDiets();

        ImageUrl = specie.ImageUrl == null ? null : UseUploadedFromId(specie.ImageUrl);

        Input = new SpeciesInputModel
        {
            Name = specie.Name,
            ClassificationId = specie.Classification.Id,
            DietId = specie.Diet.Id,
            Height = specie.Height,
            Weight = specie.Weight,
            Public = specie.IsPublic
        };

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
            DeleteImage(specie.ImageUrl);
        }

        SuccessNotification($"Espèce {specie.Name} correctement supprimée");
        return RedirectToPage("/Species/Index");
    }

    private async Task<IActionResult> PerformUpdate(string userId, Specie specie)
    {
        if (!ModelState.IsValid)
        {
            Classifications = await attributesRepository.GetClassifications();
            Diets = await attributesRepository.GetDiets();
            return Page();
        }

        var updatedSpecie = new UpdateSpecieRequest
        {
            Id = specie.Id,
            Name = Input.Name,
            ClassificationId = Input.ClassificationId,
            DietId = Input.DietId,
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
                DeleteImage(specie.ImageUrl);
            }

            var imageName = await SaveImage(Input.Image);
            updatedSpecie.ImageUrl = imageName;
        }

        var result = await repository.UpdateAsync(updatedSpecie, userId, User.IsAdmin());
        if (result.Status == ResultStatus.Conflict)
        {
            ErrorNotification("Une espèce avec ce nom existe déjà");
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
}