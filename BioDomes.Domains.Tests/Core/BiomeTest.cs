using BioDomes.Domains.Core;

namespace BioDomes.Domains.Tests.Core;

[TestFixture]
public class BiomeTests
{
    private Biome CreateBiome(double temperature)
    {
        return new Biome
        {
            Id = 1,
            CreatedBy = "tester",
            Name = "TestBiome",
            Temperature = temperature,
            Humidity = 50,
            ImageUrl = null,
            IsPublic = true
        };
    }

    [Test]
    public void should_return_critic_when_temperature_is_below_minus_20()
    {
        var biome = CreateBiome(-21);

        var state = biome.State;

        Assert.That(state, Is.EqualTo(BiomeState.Critic));
    }

    [Test]
    public void should_return_critic_when_temperature_is_above_40()
    {
        var biome = CreateBiome(41);

        var state = biome.State;

        Assert.That(state, Is.EqualTo(BiomeState.Critic));
    }

    [Test]
    public void should_return_instable_when_temperature_is_between_minus_20_and_0()
    {
        var biome = CreateBiome(-10);

        var state = biome.State;

        Assert.That(state, Is.EqualTo(BiomeState.Instable));
    }

    [Test]
    public void should_return_instable_when_temperature_is_between_30_and_40()
    {
        var biome = CreateBiome(35);

        var state = biome.State;

        Assert.That(state, Is.EqualTo(BiomeState.Instable));
    }

    [Test]
    public void should_return_optimal_when_temperature_is_between_0_and_30()
    {
        var biome = CreateBiome(20);

        var state = biome.State;

        Assert.That(state, Is.EqualTo(BiomeState.Optimal));
    }

    [Test]
    public void should_return_optimal_when_temperature_is_equal_to_0()
    {
        var biome = CreateBiome(0);

        var state = biome.State;

        Assert.That(state, Is.EqualTo(BiomeState.Optimal));
    }

    [Test]
    public void should_return_optimal_when_temperature_is_equal_to_30()
    {
        var biome = CreateBiome(30);

        var state = biome.State;

        Assert.That(state, Is.EqualTo(BiomeState.Optimal));
    }
}