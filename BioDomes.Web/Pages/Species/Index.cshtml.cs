using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Species;

public class SpeciesModel(ISpeciesRepository repo, IWebHostEnvironment env) : AppPageModel(env)
{
    public List<SpecieView> SpeciesList { get; set; } = [];
    public bool IsUserConnected { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.GetUserId();
        IsUserConnected = userId is not null;

        var result = await repo.FindAllAsync(userId, User.IsAdmin());
        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        var species = result.Value!;

        SpeciesList = species
            .Select(specie =>
            {
                var path = specie.ImageUrl != null ? UseUploadedFromId(specie.ImageUrl) : null;

                return new SpecieView
                {
                    Slug = specie.Slug,
                    Name = specie.Name,
                    Type = specie.Classification.Label,
                    Diet = specie.Diet.Label,
                    Height = FormatCentimeter(specie.Height),
                    Weight = FormatGram(specie.Weight),
                    ImageUrl = path,
                    CanEdit = IsUserConnected && (User.IsAdmin() || User.GetUserId() == specie.CreatedBy),
                };
            })
            .ToList();

        return Page();
    }
}