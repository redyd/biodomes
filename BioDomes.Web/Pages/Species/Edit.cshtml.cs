using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Species;

public class EditModel(ISpeciesRepository repository, IWebHostEnvironment environment) : AppPageModel(environment)
{
    [BindProperty] public SpeciesInputModel Input { get; set; } = new();

    public Specie? Specie { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsAdmin { get; set; }

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        var result = await repository.FindBySlugAsync(slug, userId, User.IsAdmin());
        if (!result.IsSuccess)
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

        ImageUrl = specie.ImageUrl == null ? null : UseUploadedFromId(specie.ImageUrl);

        Input = new SpeciesInputModel
        {
            Name = specie.Name,
            Type = specie.Type,
            Diet = specie.Diet,
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
        if (!result.IsSuccess)
        {
            return result.ToPageResult(this);
        }

        var specie = result.Value!;
        Specie = specie;

        return action switch
        {
            "delete" => await PerformDelete(slug, userId, specie),
            "save" => await PerformUpdate(slug, userId, specie),
            _ => Page()
        };
    }

    private async Task<IActionResult> PerformDelete(string slug, string userId, Specie specie)
    {
        await repository.DeleteAsync(slug, userId, User.IsAdmin());

        if (!string.IsNullOrEmpty(specie.ImageUrl))
        {
            DeleteImage(specie.ImageUrl);
        }

        SuccessNotification($"Espèce {specie.Name} correctement supprimée");
        return RedirectToPage("/Species/Index");
    }

    private async Task<IActionResult> PerformUpdate(string slug, string userId, Specie specie)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var updatedSpecie = new UpdateSpecieRequest
        {
            PreviousSlug = slug,
            Name = Input.Name,
            Type = Input.Type,
            Diet = Input.Diet,
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

        if (!result.IsSuccess)
        {
            return result.ToPageResult(this);
        }

        var updated = result.Value!;

        SuccessNotification($"Espèce {updated.Name} correctement modifiée");
        return RedirectToPage("/Species/Index");
    }
}