using System.Security.Claims;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Equipments;

public class Add(IWebHostEnvironment environment, IEquipmentRepository repository, ILogger<Add> logger)
    : AppPageModel(environment)
{
    [BindProperty] public EquipmentInputModel Input { get; set; } = new();
    [BindProperty] public List<EquipmentResource> Resources { get; set; } = [];

    public async Task OnGetAsync()
    {
        Resources = await repository.GetResourcesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Resources = await repository.GetResourcesAsync();
            return Page();
        }

        var userId = User.GetUserId();
        if (userId == null)
        {
            return Forbid();
        }

        var imageId = await SaveImage(Input.Image);

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
            Resources = await repository.GetResourcesAsync();
            return Page();
        }

        if (!result.IsSuccess)
        {
            return result.ToPageResult(this);
        }

        logger.LogInformation("Adding new equipment: {Name}", Input.Name);
        SuccessNotification($"L'équipement {Input.Name} a été créé avec succès");

        return RedirectToPage("/Equipments/Index");
    }
}