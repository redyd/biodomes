using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Equipments;

public class Edit(IWebHostEnvironment environment, ILogger<Edit> logger, IEquipmentRepository repository)
    : AppPageModel(environment)
{
    [BindProperty] public UpdateEquipmentInputModel Input { get; set; } = new();

    public string ImageSource { get; set; } = string.Empty;
    public List<EquipmentResource> Resources { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        var maybeEquipment = await repository.FindAsync(id, userId);
        if (maybeEquipment is null)
        {
            return NotFound();
        }

        ImageSource = UseUploadedFromId(maybeEquipment.ImageId);
        Resources = await repository.GetResourcesAsync();

        Input = new UpdateEquipmentInputModel
        {
            Name = maybeEquipment.Name,
            ConsumedResourceId = maybeEquipment.Consumed.Id,
            ProducedResourceId = maybeEquipment.Produced.Id
        };

        logger.LogInformation($"Editing {id}");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, string action)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        var original = await repository.FindAsync(id, userId);
        if (original is null) return NotFound();

        if (action == "delete")
        {
            var deleted = await repository.DeleteAsync(id, userId);
            if (deleted is null)
            {
                ErrorNotification("Impossible de supprimer cet équipement");
                return RedirectToPage("/Equipments/Index");
            }

            DeleteImage(original.ImageId);
            SuccessNotification($"L'équipement {deleted.Name} a été supprimé");

            return RedirectToPage("/Equipments/Index");
        }

        if (!ModelState.IsValid)
        {
            ErrorNotification("Formulaire invalide");
            return Page();
        }

        var request = new UpdateEquipmentRequest
        {
            Id = original.Id,
            Name = Input.Name,
            ConsumedResourceId = Input.ConsumedResourceId,
            ProducedResourceId = Input.ProducedResourceId,
            Image = original.ImageId
        };

        if (Input.Image is not null)
        {
            var newImage = await SaveImage(Input.Image);
            DeleteImage(original.ImageId);
            request.Image = newImage;
        }

        await repository.UpdateAsync(request, userId);
        SuccessNotification($"Equipement {Input.Name} modifié avec succès");

        return RedirectToPage("/Equipments/Index");
    }
}