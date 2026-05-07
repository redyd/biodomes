using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Equipments;

[Authorize]
public class Edit(IWebHostEnvironment env, ILogger<Edit> logger, IEquipmentRepository repository)
    : AppPageModel(env)
{
    [BindProperty] public UpdateEquipmentInputModel Input { get; set; } = new();
    
    public List<EquipmentResource> Resources { get; set; } = [];
    public string ImageSource { get; set; } = null!;
    public bool IsAdmin { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        var equipmentResult = await repository.FindAsync(id, userId, User.IsAdmin());
        if (equipmentResult.Failed)
        {
            return equipmentResult.ToPageResult(this);
        }
        
        var resourcesResult = await repository.GetResourcesAsync();
        if (resourcesResult.Failed) return resourcesResult.ToPageResult(this);
        
        var equipment = equipmentResult.Value!;

        if (!User.IsAdmin() && equipment.CreatedBy != userId)
        {
            return Forbid();
        }

        IsAdmin = User.IsAdmin();
        ImageSource = UseImageWithDefault(equipment.ImageId, "tool.png");
        Resources = resourcesResult.Value!;

        Input = new UpdateEquipmentInputModel
        {
            Name = equipment.Name,
            ConsumedResourceId = equipment.Consumed.Id,
            ProducedResourceId = equipment.Produced.Id,
            Public = equipment.IsPublic
        };

        logger.LogInformation($"Editing {id}");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, string action)
    {
        // block unauth user
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        // find the result
        var equipmentResult = await repository.FindAsync(id, userId, User.IsAdmin());
        if (equipmentResult.Failed)
        {
            return equipmentResult.ToPageResult(this);
        }

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
            return Page();
        }

        var request = new UpdateEquipmentRequest
        {
            Id = equipment.Id,
            Name = Input.Name,
            ConsumedResourceId = Input.ConsumedResourceId,
            ProducedResourceId = Input.ProducedResourceId,
            Image = equipment.ImageId
        };

        if (User.IsAdmin())
        {
            request.IsPublic = Input.Public;
        }

        if (Input.Image is not null)
        {
            var newImage = await SaveImage(Input.Image);
            DeleteImage(equipment.ImageId);
            request.Image = newImage;
        }

        await repository.UpdateAsync(request, userId, User.IsAdmin());
        SuccessNotification($"Equipement {Input.Name} modifié avec succès");

        return RedirectToPage("/Equipments/Index");
    }

    private async Task<IActionResult> PerformDelete(int id, string userId, Equipment equipment)
    {
        var deletedResult = await repository.DeleteAsync(id, userId, User.IsAdmin());
        if (deletedResult.Failed)
        {
            return deletedResult.ToPageResult(this);
        }

        var deleted = deletedResult.Value!;

        DeleteImage(equipment.ImageId);
        SuccessNotification($"L'équipement {deleted.Name} a été supprimé");

        return RedirectToPage("/Equipments/Index");
    }
}