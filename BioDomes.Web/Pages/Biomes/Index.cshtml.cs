using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Biomes;

public class BiomePage(IBiomeRepository repository, IWebHostEnvironment env) : AppPageModel(env)
{
    public List<BiomeView>? Biomes { get; set; }
    
    public bool IsUserConnected { get; set; }

    public async Task OnGetAsync()
    {
        IsUserConnected = User.GetUserId() is not null;
        

        var biomes = await repository.GetAllAsync(User.GetUserId(), User.IsAdmin());
        Biomes = biomes.Select(b => new BiomeView
        {
            ImageUrl = !string.IsNullOrWhiteSpace(b.ImageUrl) ? UseUploadedFromId(b.ImageUrl) : null,
            CanEdit = IsUserConnected && (User.IsAdmin() || b.CreatedBy == User.GetUserId()),
            IsPublic = true,
            Model = b
        })
            .ToList();

    }

}