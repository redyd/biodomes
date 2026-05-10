using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Domains.Services;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.Mapper;
using BioDomes.Web.PageModels;
using BioDomes.Web.Services;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Biomes;

public class Detail(
    IImagePathResolver imageResolver,
    IBiomeRepository biomeRepo,
    IBiomeAnalyzer analyzer,
    IEquipmentRepository equipmentRepo,
    IBiomeManagementRepository managementRepo,
    IBiomeViewMapper biomeMapper,
    IEquipmentViewMapper equipmentMapper,
    ISpecieViewMapper specieMapper)
    : AppPageModel
{
    [BindProperty(SupportsGet = true)] public string Slug { get; set; } = null!;

    // BASICS
    public BiomeView<ComplexBiome> Biome { get; set; } = null!;
    public BiomeAnalysis Analyze { get; set; } = null!;
    public Dictionary<RecommendationType, List<Recommendation<SpecieView>>> Recommendations { get; set; } = null!;
    public List<EquipmentView> AllEquipments { get; set; } = null!;
    public string Creator { get; set; } = string.Empty;

    // FORMS
    [BindProperty] public BiomeSpecieInputModel SpecieInput { get; set; } = new();
    [BindProperty] public BiomeEquipmentInputModel EquipmentInput { get; set; } = new();

    public bool ReopenAddSpecieDialog { get; set; }
    public bool ReopenAddEquipmentDialog { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var loadResult = await LoadPageDataAsync();
        if (loadResult is not null) return loadResult;

        SpecieInput = new BiomeSpecieInputModel();
        EquipmentInput = new BiomeEquipmentInputModel();

        return Page();
    }

    // POST EQUIPMENTS
    public async Task<IActionResult> OnPostAddEquipmentAsync()
    {
        ModelState.Clear();
        if (!TryValidateModel(EquipmentInput, nameof(EquipmentInput)))
        {
            var loadResult = await LoadPageDataAsync();
            ReopenAddEquipmentDialog = true;
            if (loadResult != null) return loadResult;
            return Page();
        }

        var request = new EquipmentToBiomeRequest
        {
            EquipmentSlug = EquipmentInput.EquipmentSlug,
            Count = (int)EquipmentInput.Count!
        };

        var result = await managementRepo.AddEquipmentToBiome(request, Slug, User.GetUserId(), User.IsAdmin());

        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        ReopenAddEquipmentDialog = false;
        SuccessNotification("Equipement ajouté avec succès");

        return RedirectToPage(pageName: null, pageHandler: null, routeValues: new { Slug }, fragment: "title-section");
    }

    public async Task<IActionResult> OnPostRemoveEquipmentAsync()
    {
        ModelState.Clear();
        if (!TryValidateModel(EquipmentInput, nameof(EquipmentInput)))
        {
            var loadResult = await LoadPageDataAsync();
            if (loadResult != null) return loadResult;
            return Page();
        }

        var request = new EquipmentToBiomeRequest
        {
            EquipmentSlug = EquipmentInput.EquipmentSlug,
            Count = (int)EquipmentInput.Count!
        };

        var result = await managementRepo.RemoveEquipmentToBiome(request, Slug, User.GetUserId(), User.IsAdmin());

        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        SuccessNotification("Equipement retirée avec succès");
        return RedirectToPage(pageName: null, pageHandler: null, routeValues: new { slug = Slug }, fragment: "title-section");
    }

    // POST SPECIES
    public async Task<IActionResult> OnPostAddSpecieAsync()
    {
        ModelState.Clear();
        if (!TryValidateModel(SpecieInput, nameof(SpecieInput)))
        {
            var loadResult = await LoadPageDataAsync();
            ReopenAddSpecieDialog = true;
            if (loadResult != null) return loadResult;
            return Page();
        }

        var request = new SpecieToBiomeRequest
        {
            SpecieSlug = SpecieInput.SpecieSlug,
            Count = (int)SpecieInput.Count!
        };
        
        var result = await managementRepo.AddSpecieToBiome(request, Slug, User.GetUserId(), User.IsAdmin());

        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        ReopenAddSpecieDialog = false;
        SuccessNotification("Espèce ajoutée avec succès");
        return RedirectToPage(pageName: null, pageHandler: null, routeValues: new { Slug }, fragment: "title-section");
    }

    public async Task<IActionResult> OnPostRemoveSpecieAsync()
    {
        ModelState.Clear();
        if (!TryValidateModel(SpecieInput, nameof(SpecieInput)))
        {
            var loadResult = await LoadPageDataAsync();
            if (loadResult != null) return loadResult;
            return Page();
        }

        var request = new SpecieToBiomeRequest
        {
            SpecieSlug = SpecieInput.SpecieSlug,
            Count = (int)SpecieInput.Count!
        };
        
        var result = await managementRepo.RemoveSpecieToBiome(request, Slug, User.GetUserId(), User.IsAdmin());

        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        SuccessNotification("Espèce retirée avec succès");
        return RedirectToPage(pageName: null, pageHandler: null, routeValues: new { Slug }, fragment: "title-section");
    }

    // ACTIONS
    public async Task<IActionResult> OnPostExecuteActionAsync(string actionId)
    {
        var result = await analyzer.PerformAction(actionId, User.GetUserId(), User.IsAdmin());

        if (result.Failed)
        {
            ErrorNotification("Une erreur est survenue lors de l'action");
            return result.ToPageResult(this);
        }

        SuccessNotification("Action correctement effectuée");
        return RedirectToPage(pageName: null, pageHandler: null, routeValues: new { Slug },
            fragment: "title-section");
    }

    // HELPER 
    private async Task<IActionResult?> LoadPageDataAsync()
    {
        var userId = User.GetUserId();

        var biomeResult = await biomeRepo.FindComplexAsync(Slug, userId, User.IsAdmin());
        if (biomeResult.Failed) return biomeResult.ToPageResult(this);

        var creatorResult = await biomeRepo.GetCreatorAsync(biomeResult.Value!.Slug);
        if (creatorResult.Failed) return creatorResult.ToPageResult(this);

        var equipmentsResults = await equipmentRepo.GetAllAsync(User.GetUserId(), User.IsAdmin());
        if (equipmentsResults.Failed) return equipmentsResults.ToPageResult(this);

        var analyze = await analyzer.AnalyzeBiome(Slug, userId, User.IsAdmin());

        if (analyze is null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        
        Analyze = analyze;
        Creator = creatorResult.Value!;

        var species = await analyzer.RecommendationToAddForBiome(Slug, userId, User.IsAdmin(), Analyze.Graph);

        Biome = biomeMapper.MapSingle(
            biomeResult.Value!,
            User.IsAdmin(),
            userId,
            image => imageResolver.UseImageWithDefault(image, "biome_bg.png"));

        AllEquipments = equipmentMapper.MapAll(
            equipmentsResults.Value!,
            User.IsAdmin(),
            userId,
            image => imageResolver.UseImageWithDefault(image, "tool.png"));

        Recommendations = species
            .GroupBy(r => r.Type)
            .OrderBy(g => g.Key)
            .ToDictionary(
                rt => rt.Key,
                sps => sps.Select(sp => new Recommendation<SpecieView>
                {
                    ActionId = sp.ActionId,
                    Label = sp.Label,
                    Message = sp.Message,
                    Type = sp.Type,
                    Involved = specieMapper.MapSingle(
                        sp.Involved,
                        User.IsAdmin(),
                        userId,
                        image => imageResolver.UseImageWithDefault(image, "specie.png"))
                }).ToList());

        return null;
    }
}