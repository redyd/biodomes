using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Services;
using BioDomes.Domains.Requests;
using Moq;

namespace BioDomes.Domains.Tests.Services;

[TestFixture]
public class BiomeAnalyzerTests
{
    private Mock<IFoodChainRepository> _foodChainRepo = null!;
    private Mock<ISpeciesRepository> _speciesRepo = null!;
    private Mock<IBiomeRepository> _biomeRepo = null!;
    private Mock<IBiomeManagementRepository> _managementRepo = null!;

    private BiomeAnalyzer _analyzer = null!;

    [SetUp]
    public void Setup()
    {
        _foodChainRepo = new Mock<IFoodChainRepository>();
        _speciesRepo = new Mock<ISpeciesRepository>();
        _biomeRepo = new Mock<IBiomeRepository>();
        _managementRepo = new Mock<IBiomeManagementRepository>();

        _analyzer = new BiomeAnalyzer(
            _foodChainRepo.Object,
            _speciesRepo.Object,
            _biomeRepo.Object,
            _managementRepo.Object
        );
    }

    [Test]
    public async Task should_return_empty_list_when_repositories_fail()
    {
        _foodChainRepo
            .Setup(r => r.GetFullGraphAsync(It.IsAny<string?>(), It.IsAny<bool>()))
            .ReturnsAsync(ResultT<FoodChainGraph>.Failure("error"));

        _speciesRepo
            .Setup(r => r.FindAllAsDictAsync(It.IsAny<string?>(), It.IsAny<bool>()))
            .ReturnsAsync(ResultT<Dictionary<int, Specie>>.Failure("error"));

        var graph = new FoodChainGraph(
            new List<FoodChainNode>(),
            new List<FoodChainEdge>()
        );

        var result = await _analyzer.RecommendationToAddForBiome("user", false, 1, graph);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task should_return_not_found_when_action_is_unknown()
    {
        var result = await _analyzer.PerformAction("unknown-action", "user", false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task should_call_repository_when_add_species_action()
    {
        _managementRepo
            .Setup(r => r.AddSpecieToBiome(
                It.IsAny<SpecieToBiomeRequest>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(Result.Ok());

        var actionId = "add-biome-specie specie:1 count:2 biome:10";

        await _analyzer.PerformAction(actionId, "user", false);

        _managementRepo.Verify(r =>
            r.AddSpecieToBiome(
                It.Is<SpecieToBiomeRequest>(x => x.SpecieId == 1 && x.Count == 2),
                10,
                "user",
                false),
            Times.Once);
    }

    [Test]
    public async Task should_return_empty_analysis_when_graph_fails()
    {
        _foodChainRepo
            .Setup(r => r.GetFullGraphForBiomeAsync(It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<bool>()))
            .ReturnsAsync(ResultT<FoodChainGraph>.Failure("error"));

        _speciesRepo
            .Setup(r => r.FindAllAsDictAsync(It.IsAny<string?>(), It.IsAny<bool>()))
            .ReturnsAsync(ResultT<Dictionary<int, Specie>>.Failure("error"));

        var result = await _analyzer.AnalyzeBiome("user", false, 1);

        Assert.That(result.Graph.Nodes, Is.Empty);
        Assert.That(result.Graph.Edges, Is.Empty);
        Assert.That(result.Stats.BiomeId, Is.EqualTo(1));
    }

    [Test]
    public async Task should_return_global_statistic_even_if_empty()
    {
        _biomeRepo
            .Setup(r => r.GetAllComplexAsync(It.IsAny<string?>(), It.IsAny<bool>()))
            .ReturnsAsync(ResultT<List<ComplexBiome>>.Failure("error"));

        var result = await _analyzer.GlobalAnalyze("user", false);

        Assert.That(result.All, Is.Empty);
    }
}