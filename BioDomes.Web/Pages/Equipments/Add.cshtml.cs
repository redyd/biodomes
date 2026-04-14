using System.Security.Claims;
using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Equipments;

public class Add : AppPageModel
{
    private readonly IEquipmentRepository _repository;
    private readonly ILogger<Add> _logger;

    public Add(IWebHostEnvironment environment, IEquipmentRepository repository, ILogger<Add> logger)
        : base(environment)
    {
        _repository = repository;
        _logger = logger;
    }

    [BindProperty] public EquipmentInputModel Input { get; set; } = new();
    [BindProperty] public List<EquipmentResource> Resources { get; set; } = [];

    public async Task OnGetAsync()
    {
        Resources = await _repository.GetResourcesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Resources = await _repository.GetResourcesAsync();
            return Page();
        }

        var userId = CurrentUserId();
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
            UserId = (int)userId
        };

        await _repository.CreateEquipmentAsync(createRequest, (int)userId);

        _logger.LogInformation("Adding new equipment: {Name}", Input.Name);
        SuccessNotification($"L'équipement {Input.Name} a été créé avec succès");

        return RedirectToPage("/Equipments/Index");
    }
}