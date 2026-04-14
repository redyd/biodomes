using System.Security.Claims;
using BioDomes.Domains.Repository;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;

namespace BioDomes.Web.Pages.Equipments;

public class EquipmentPage : AppPageModel
{
    private readonly IEquipmentRepository _repo;

    public EquipmentPage(IEquipmentRepository repo, IWebHostEnvironment environment) : base(environment)
    {
        _repo = repo;
    }

    public List<EquipmentView> Equipments { get; set; } = [];

    public async Task OnGetAsync()
    {
        var userId = CurrentUserId();
        var isAdmin = IsAdmin();

        var equipments = await _repo.GetAllAsync(userId, isAdmin);
        Equipments = equipments.Select(e => new EquipmentView
        {
            Id = e.Id,
            Name = e.Name,
            Image = UseUploadedFromId(e.ImageId),
            Produced = e.Produced.Label,
            Consumed = e.Consumed.Label,
        })
        .ToList();
    }
}