using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.Dtos;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.FoodChain;

public class Index(IWebHostEnvironment environment, IFoodChainRepository repo)
    : AppPageModel(environment)
{
    public FoodChainGraph? Graph { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Graph = await repo.GetFullGraphAsync();

        return Page();
    }

    public async Task OnPostAddRelation([FromBody] NodeActionDto action)
    {
        await repo.AddRelation(action.PredatorId, action.PreyId);
    }

    public async Task OnPostRemoveRelation([FromBody] NodeActionDto action)
    {
        await repo.DeleteRelation(action.PredatorId, action.PreyId);
    }
    
    public async Task<IActionResult> OnPostNodeClicked([FromBody] NodeClickDto node)
    {
        var graph = await repo.GetFullGraphAsync();
        var specie = graph.Nodes.FirstOrDefault(x => x.SpecieId == node.SpecieId);
        var preys = graph.PreyOf(node.SpecieId);
        var available = graph.AvailablePreyFor(node.SpecieId);

        return new JsonResult(new
        {
            id = specie?.SpecieId,
            name = specie?.Name,
            classification = specie?.Classification,
            preys,
            available,
        });
    }
}