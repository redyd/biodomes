using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.Dtos;
using BioDomes.Web.InputModel;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.FoodChain;

public class Index(IWebHostEnvironment environment, IFoodChainRepository repo, ILogger<Index> logger)
    : AppPageModel(environment)
{
    public FoodChainGraph? Graph { get; set; }
    public string? Selected { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Graph = await repo.GetFullGraphAsync();

        return Page();
    }

    public async Task OnPostAddRelation([FromBody] NodeActionDto action)
    {
        await repo.AddRelation(action.PredatorSlug, action.PreySlug);
    }

    public async Task OnPostRemoveRelation([FromBody] NodeActionDto action)
    {
        await repo.DeleteRelation(action.PredatorSlug, action.PreySlug);
    }
    
    public async Task<IActionResult> OnPostNodeClicked([FromBody] NodeClickDto node)
    {
        var graph = await repo.GetFullGraphAsync();
        var specie = graph.Nodes.FirstOrDefault(x => x.Slug == node.Slug);
        var preys = graph.PreyOf(node.Slug);
        var available = graph.AvailablePreyFor(node.Slug);

        logger.LogInformation("CLICKED {@Slug}", node.Slug);

        return new JsonResult(new
        {
            slug = specie?.Slug,
            name = specie?.Name,
            classification = specie?.Classification,
            preys,
            available,
        });
    }
}