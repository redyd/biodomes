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

namespace BioDomes.Web.Pages.Equipments;

[Authorize]
public class Edit(
    IWebHostEnvironment env,
    IImagePathResolver imageResolver,
    ILogger<Edit> logger,
    IEquipmentRepository repository)
    : AppPageModel
{
    [BindProperty] public UpdateEquipmentInputModel Input { get; set; } = new();

    public SelectList Productions { get; set; } = null!;
    public SelectList Consomations { get; set; } = null!;
    public string ImageSource { get; set; } = null!;
    public bool IsAdmin { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = User.GetUserId();
        if (userId == null) return Forbid();

        var equipmentResult = await repository.FindAsync(id, userId, User.IsAdmin());
        if (equipmentResult.Failed) return equipmentResult.ToPageResult(this);

        var equipment = equipmentResult.Value!;

        if (!User.IsAdmin() && equipment.CreatedBy != userId) return Forbid();

        IsAdmin = User.IsAdmin();
        ImageSource = imageResolver.UseImageWithDefault(equipment.ImageId, "tool.png");

        Input = new UpdateEquipmentInputModel
        {
            Name = equipment.Name,
            ConsumedResourceId = equipment.Consumed.Id,
            ProducedResourceId = equipment.Produced.Id,
            Public = equipment.IsPublic
        };

        var loaded = await LoadSelectLists();
        if (loaded is not null) return loaded;

        logger.LogInformation("Editing {Id}", id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, string action)
    {
        var userId = User.GetUserId();
        if (userId == null) return Forbid();

        var equipmentResult = await repository.FindAsync(id, userId, User.IsAdmin());
        if (equipmentResult.Failed) return equipmentResult.ToPageResult(this);

        var equipment = equipmentResult.Value!;

        return action switch
        {
            "delete" => await PerformDelete(id, userId, equipment),
            "save" => await PerformUpdate(equipment, userId),
            _ => Page()
        };
    }

    private async Task<IActionResult> PerformUpdate(Equipment equipment, string userId)
    {
        if (!ModelState.IsValid)
        {
            var loaded = await LoadSelectLists();
            if (loaded is not null) return loaded;

            ImageSource = imageResolver.UseImageWithDefault(equipment.ImageId, "tool.png");
            IsAdmin = User.IsAdmin();
            return Page();
        }

        var request = new UpdateEquipmentRequest
        {
            Id = equipment.Id,
            Name = Input.Name,
            ConsumedResourceId = (int)Input.ConsumedResourceId!,
            ProducedResourceId = (int)Input.ProducedResourceId!,
            Image = equipment.ImageId
        };

        if (User.IsAdmin())
        {
            request.IsPublic = Input.Public;
        }

        if (Input.Image is not null)
        {
            var newImage = await imageResolver.SaveImage(Input.Image, env);
            imageResolver.DeleteImage(equipment.ImageId, env);
            request.Image = newImage;
        }

        var result = await repository.UpdateAsync(request, userId, User.IsAdmin());

        if (result.Status == ResultStatus.Conflict)
        {
            ErrorNotification("Un équipement de ce nom existe déjà.");

            var loaded = await LoadSelectLists();
            if (loaded is not null) return loaded;

            ImageSource = imageResolver.UseImageWithDefault(equipment.ImageId, "tool.png");
            IsAdmin = User.IsAdmin();
            return Page();
        }

        if (result.Failed) return result.ToPageResult(this);

        SuccessNotification($"Equipement {Input.Name} modifié avec succès");
        return RedirectToPage("/Equipments/Index");
    }

    private async Task<IActionResult> PerformDelete(int id, string userId, Equipment equipment)
    {
        var deletedResult = await repository.DeleteAsync(id, userId, User.IsAdmin());
        if (deletedResult.Failed) return deletedResult.ToPageResult(this);

        imageResolver.DeleteImage(equipment.ImageId, env);
        SuccessNotification($"L'équipement {deletedResult.Value!.Name} a été supprimé");

        return RedirectToPage("/Equipments/Index");
    }

    private async Task<IActionResult?> LoadSelectLists()
    {
        var resourcesResult = await repository.GetResourcesAsync();
        if (resourcesResult.Failed) return resourcesResult.ToPageResult(this);

        Productions = new SelectList(resourcesResult.Value!, "Id", "Label", Input.ProducedResourceId);
        Consomations = new SelectList(resourcesResult.Value!, "Id", "Label", Input.ConsumedResourceId);
        return null;
    }
}