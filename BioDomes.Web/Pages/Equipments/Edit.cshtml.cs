using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Equipments;

public class Edit : AppPageModel
{
    private readonly ILogger<Edit> _logger;
    private readonly IEquipmentRepository _repository;

    public Edit(IWebHostEnvironment environment, ILogger<Edit> logger, IEquipmentRepository repository) :
        base(environment)
    {
        _logger = logger;
        _repository = repository;
    }

    [BindProperty] public UpdateEquipmentInputModel Input { get; set; } = new();
    [BindProperty] public string ImageSource { get; set; } = string.Empty;
    public List<EquipmentResource> Resources { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var maybeEquipment = await _repository.FindAsync(id);
        if (maybeEquipment is null)
        {
            return NotFound();
        }

        ImageSource = UseUploadedFromId(maybeEquipment.ImageId);
        Resources = await _repository.GetResourcesAsync();

        Input = new UpdateEquipmentInputModel
        {
            Name = maybeEquipment.Name,
            ConsumedResourceId = maybeEquipment.Consumed.Id,
            ProducedResourceId = maybeEquipment.Produced.Id
        };

        _logger.LogInformation($"Editing {id}");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, string action)
    {
        var original = await _repository.FindAsync(id);
        if (original is null) return NotFound();

        if (action == "delete")
        {
            var deleted = await _repository.DeleteAsync(id);
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
            Resources = await _repository.GetResourcesAsync();
            ImageSource = UseUploadedFromId(original.ImageId);
            ErrorNotification("Formulaire invalide");
            return Page();
        }

        var request = new UpdateEquipmentRequest
        {
            Id = original.Id,
            Name = Input.Name,
            ConsumedResourceId = Input.ConsumedResourceId!.Value,
            ProducedResourceId = Input.ProducedResourceId!.Value,
            Image = original.ImageId
        };

        if (Input.Image is not null)
        {
            var newImage = await SaveImage(Input.Image);
            DeleteImage(original.ImageId);
            request.Image = newImage;
        }

        await _repository.UpdateAsync(request);

        return RedirectToPage("/Equipments/Index");
    }
}