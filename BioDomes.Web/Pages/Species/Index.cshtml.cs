using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using BioDomes.Web.Mapper;
using BioDomes.Web.PageModels;
using BioDomes.Web.Services;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Species;

public class SpeciesModel(ISpeciesRepository repo, IImagePathResolver imageResolver, ISpecieViewMapper specieMapper)
    : AppPageModel
{
    public List<SpecieView> SpeciesList { get; set; } = [];
    public bool IsUserConnected { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.GetUserId();
        IsUserConnected = userId is not null;

        var result = await repo.FindAllAsync(userId, User.IsAdmin());
        if (result.Failed) return result.ToPageResult(this);

        var species = result.Value!;

        SpeciesList = specieMapper.MapAll(
            species,
            User.IsAdmin(),
            User.GetUserId(),
            image => imageResolver.UseImageWithDefault(image, "specie.png"));

        return Page();
    }
}