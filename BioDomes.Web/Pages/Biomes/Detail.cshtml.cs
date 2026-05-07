using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Domains.Services;
using BioDomes.Web.Extensions;
using BioDomes.Web.InputModel;
using BioDomes.Web.Mapper;
using BioDomes.Web.PageModels;
using BioDomes.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BioDomes.Web.Pages.Biomes;

public class Detail(
    IWebHostEnvironment env,
    IBiomeRepository biomeRepo,
    IBiomeAnalyzer analyzer,
    IEquipmentRepository equipmentRepo,
    IBiomeManagementRepository managementRepo,
    IBiomeViewMapper biomeMapper,
    IEquipmentViewMapper equipmentMapper)
    : AppPageModel(env)
{
    // BASICS
    public BiomeView<ComplexBiome> Biome { get; set; } = null!;
    public BiomeAnalysis Analyze { get; set; } = null!;
    public List<Recommendation<Specie>> Recommendations { get; set; } = null!;
    public List<EquipmentView> AllEquipments { get; set; } = null!;
    public string Creator { get; set; } = string.Empty;

    // FORMS
    [BindProperty] public BiomeSpecieInputModel SpecieInput { get; set; } = new();
    [BindProperty] public BiomeEquipmentInputModel EquipmentInput { get; set; } = new();

    public bool ReopenAddSpecieDialog { get; set; }
    public bool ReopenAddEquipmentDialog { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var loadResult = await LoadPageDataAsync(id);
        if (loadResult is not null) return loadResult;

        SpecieInput = new BiomeSpecieInputModel();
        EquipmentInput = new BiomeEquipmentInputModel();

        return Page();
    }

    // POST EQUIPMENTS
    public async Task<IActionResult> OnPostAddEquipmentAsync(int id)
    {
        ModelState.Clear();
        if (!TryValidateModel(EquipmentInput, nameof(EquipmentInput)))
        {
            var loadResult = await LoadPageDataAsync(id);
            ReopenAddEquipmentDialog = true;
            if (loadResult != null) return loadResult;
            return Page();
        }

        var result = await managementRepo.AddEquipmentToBiome(new EquipmentToBiomeRequest
        {
            EquipmentId = (int)EquipmentInput.EquipmentId!,
            Count = (int)EquipmentInput.Count!
        }, id, User.GetUserId(), User.IsAdmin());

        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        ReopenAddEquipmentDialog = false;
        SuccessNotification("Equipement ajouté avec succès");
        return RedirectToPage(pageName: null, pageHandler: null, routeValues: new { id }, fragment: "title-section");
    }

    public async Task<IActionResult> OnPostRemoveEquipmentAsync(int id)
    {
        ModelState.Clear();
        if (!TryValidateModel(EquipmentInput, nameof(EquipmentInput)))
        {
            var loadResult = await LoadPageDataAsync(id);
            if (loadResult != null) return loadResult;
            return Page();
        }

        var result = await managementRepo.RemoveEquipmentToBiome(new EquipmentToBiomeRequest
        {
            EquipmentId = (int)EquipmentInput.EquipmentId!,
            Count = (int)EquipmentInput.Count!
        }, id, User.GetUserId(), User.IsAdmin());

        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        SuccessNotification("Equipement retirée avec succès");
        return RedirectToPage(pageName: null, pageHandler: null,
            routeValues: new { id }, fragment: "title-section");
    }

    // POST SPECIES
    public async Task<IActionResult> OnPostAddSpecieAsync(int id)
    {
        ModelState.Clear();
        if (!TryValidateModel(SpecieInput, nameof(SpecieInput)))
        {
            var loadResult = await LoadPageDataAsync(id);
            ReopenAddSpecieDialog = true;
            if (loadResult != null) return loadResult;
            return Page();
        }

        var result = await managementRepo.AddSpecieToBiome(new SpecieToBiomeRequest
        {
            SpecieId = (int)SpecieInput.SpecieId!,
            Count = (int)SpecieInput.Count!
        }, id, User.GetUserId(), User.IsAdmin());

        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        ReopenAddSpecieDialog = false;
        SuccessNotification("Espèce ajoutée avec succès");
        return RedirectToPage(pageName: null, pageHandler: null, routeValues: new { id }, fragment: "title-section");
    }

    public async Task<IActionResult> OnPostRemoveSpecieAsync(int id)
    {
        ModelState.Clear();
        if (!TryValidateModel(SpecieInput, nameof(SpecieInput)))
        {
            var loadResult = await LoadPageDataAsync(id);
            if (loadResult != null) return loadResult;
            return Page();
        }

        var result = await managementRepo.RemoveSpecieToBiome(new SpecieToBiomeRequest
        {
            SpecieId = (int)SpecieInput.SpecieId!,
            Count = (int)SpecieInput.Count!
        }, id, User.GetUserId(), User.IsAdmin());

        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        SuccessNotification("Espèce retirée avec succès");
        return RedirectToPage(pageName: null, pageHandler: null,
            routeValues: new { id }, fragment: "title-section");
    }

    // ACTIONS
    public async Task<IActionResult> OnPostExecuteActionAsync(string actionId, int biomeId)
    {
        var result = await analyzer.PerformAction(actionId, User.GetUserId(), User.IsAdmin());

        if (result.Failed)
        {
            return result.ToPageResult(this);
        }

        return RedirectToPage(pageName: null, pageHandler: null, routeValues: new { id = biomeId },
            fragment: "title-section");
    }

    // HELPER 
    private async Task<IActionResult?> LoadPageDataAsync(int id)
    {
        var userId = User.GetUserId();

        var biomeResult = await biomeRepo.FindComplexAsync(id, userId, User.IsAdmin());
        if (biomeResult.Failed) return biomeResult.ToPageResult(this);

        var creatorResult = await biomeRepo.GetCreatorAsync(biomeResult.Value!.Id);
        if (creatorResult.Failed) return creatorResult.ToPageResult(this);

        var equipmentsResults = await equipmentRepo.GetAllAsync(User.GetUserId(), User.IsAdmin());
        if (equipmentsResults.Failed) return equipmentsResults.ToPageResult(this);

        Biome = biomeMapper.MapSingle(
            biomeResult.Value,
            User.IsAdmin(),
            userId,
            image => UseImageWithDefault(image, "biome_bg.png"));

        AllEquipments = equipmentMapper.MapAll(
            equipmentsResults.Value!,
            User.IsAdmin(),
            userId,
            image => UseImageWithDefault(image, "tool.png"));

        Analyze = await analyzer.AnalyzeBiome(userId, User.IsAdmin(), id);
        Recommendations = await analyzer.RecommendationToAddForBiome(userId, User.IsAdmin(), id, Analyze.Graph);
        Creator = creatorResult.Value!;

        return null;
    }
}