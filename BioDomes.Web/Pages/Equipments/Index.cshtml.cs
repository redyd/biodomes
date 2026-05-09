using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using BioDomes.Web.Mapper;
using BioDomes.Web.PageModels;
using BioDomes.Web.Services;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Equipments;

public class EquipmentPage(IEquipmentRepository repo, IEquipmentViewMapper mapper, IImagePathResolver imageResolver)
    : AppPageModel
{
    public List<EquipmentView> Equipments { get; set; } = [];
    public bool IsUserConnected { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        IsUserConnected = User.GetUserId() is not null;

        var equipmentsResult = await repo.GetAllAsync(User.GetUserId(), User.IsAdmin());
        if (equipmentsResult.Failed) return equipmentsResult.ToPageResult(this);

        Equipments = mapper.MapAll(
            equipmentsResult.Value!, 
            User.IsAdmin(),
            User.GetUserId(),
            image => imageResolver.UseImageWithDefault(image, "tool.png"));
        
        return Page();
    }
}