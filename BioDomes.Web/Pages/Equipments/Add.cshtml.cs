using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using BioDomes.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Equipments;

[Authorize]
public class Add(IWebHostEnvironment env, IImagePathResolver imageResolver, IEquipmentRepository repository, ILogger<Add> logger)
    : AppPageModel
{
    [BindProperty] public EquipmentInputModel Input { get; set; } = new();
    [BindProperty] public List<EquipmentResource> Resources { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }
        var resourcesResult = await repository.GetResourcesAsync();
        if (resourcesResult.Failed) return resourcesResult.ToPageResult(this);
        
        Resources = resourcesResult.Value!; 
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var resourcesResult = await repository.GetResourcesAsync();
            if (resourcesResult.Failed) return resourcesResult.ToPageResult(this);
        
            Resources = resourcesResult.Value!; 
            return Page();
        }

        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        var imageId = await imageResolver.SaveImage(Input.Image, env);

        var createRequest = new CreateEquipmentRequest
        {
            Name = Input.Name,
            Image = imageId,
            ConsumedResourceId = Input.ConsumedResourceId,
            ProducedResourceId = Input.ProducedResourceId,
            UserId = userId
        };

        var result = await repository.CreateEquipmentAsync(createRequest, userId);

        if (result.Status == ResultStatus.Conflict)
        {
            ErrorNotification("Un équipement de ce nom existe déjà.");
            var resourcesResult = await repository.GetResourcesAsync();
            if (resourcesResult.Failed) return resourcesResult.ToPageResult(this);
        
            Resources = resourcesResult.Value!; 
            return Page();
        }

        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        logger.LogInformation("Adding new equipment: {Name}", Input.Name);
        SuccessNotification($"L'équipement {Input.Name} a été créé avec succès");

        return RedirectToPage("/Equipments/Index");
    }
}