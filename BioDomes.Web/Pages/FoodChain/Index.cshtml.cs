using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Web.Dtos;
using BioDomes.Web.Extensions;
using BioDomes.Web.PageModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.FoodChain;

public class Index(IFoodChainRepository repo) : AppPageModel
{
    public FoodChainGraph Graph { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var result = await repo.GetFullGraphAsync(User.GetUserId(), User.IsAdmin());
        if (result.Failed) return result.ToPageResult(this);

        Graph = result.Value!;

        return Page();
    }

    public async Task<IActionResult> OnPostAddRelation([FromBody] NodeActionDto action)
    {
        var result = await repo.AddRelation(action.PredatorId, action.PreyId);
        if (result.Failed) return result.ToPageResult(this);

        return Page();
    }

    public async Task<IActionResult> OnPostRemoveRelation([FromBody] NodeActionDto action)
    {
        var result = await repo.DeleteRelation(action.PredatorId, action.PreyId);
        if (result.Failed) return result.ToPageResult(this);
        
        return Page();
    }

    public async Task<IActionResult> OnPostNodeClicked([FromBody] NodeClickDto node)
    {
        var result = await repo.GetFullGraphAsync(User.GetUserId(), User.IsAdmin());
        if (result.Failed) return result.ToPageResult(this);

        var graph = result.Value!;
        var chainNode = graph.Nodes.First(x => x.Specie.Slug == node.SpecieSlug);
        var preys = graph.PreyOf(node.SpecieSlug);
        var available = graph.AvailablePreyFor(node.SpecieSlug);

        return new JsonResult(new NodeClickedDetailDto
        {
            Slug = chainNode.Specie.Slug,
            Name = chainNode.Specie.Name,
            Classification = chainNode.Specie.Classification,
            Preys = preys,
            Available = available
        });
    }
}