using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;

namespace BioDomes.Web.Pages.Equipments;

public class EquipmentPage(IEquipmentRepository repo, IWebHostEnvironment env) : AppPageModel(env)
{
    public List<EquipmentView> Equipments { get; set; } = [];
    public bool IsUserConnected { get; set; }
    
    public async Task OnGetAsync()
    {
        IsUserConnected = User.GetUserId() is not null;
        
        var equipments = await repo.GetAllAsync(User.GetUserId(), User.IsAdmin());
        Equipments = equipments.Select(e => new EquipmentView
        {
            Id = e.Id,
            Name = e.Name,
            Image = UseUploadedFromId(e.ImageId),
            Produced = e.Produced.Label,
            Consumed = e.Consumed.Label,
            CanEdit = IsUserConnected && (User.IsAdmin() || e.CreatedBy == User.GetUserId())
        })
        .ToList();
    }
}