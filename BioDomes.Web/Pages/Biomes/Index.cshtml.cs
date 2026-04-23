using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.Extensions;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BioDomes.Web.Pages.Biomes;

public class BiomePage(IBiomeRepository repository, IWebHostEnvironment environment) : AppPageModel(environment)
{
    public List<BiomeView>? Biomes { get; set; }
    
    public bool IsUserConnected { get; set; }

    public async Task OnGetAsync()
    {
        IsUserConnected = User.GetUserId() is not null;

        var biomes = await repository.GetAllAsync(User.GetUserId(), User.IsAdmin());
        Biomes = biomes.Select(b => new BiomeView
        {
            Id = b.Id,
            Name = b.Name,
            Temperature = b.Temperature,
            Humidity = b.Humidity,
            ImageUrl = UseUploadedFromId(b.ImageUrl),
            CanEdit = IsUserConnected && (User.IsAdmin() || b.CreatedBy == User.GetUserId())
        })
            .ToList();

    }
}