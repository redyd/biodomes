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
        var equipments = await _repo.GetAllAsync();
        Equipments = equipments.Select(e => new EquipmentView
        {
            Name = e.Name,
            Image = UseUploadedFromId(e.ImageId),
            Produced = e.Produced,
            Consumed = e.Consumed,
        })
        .ToList();
    }
}