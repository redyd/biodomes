using BioDomes.Domains.Core;

namespace BioDomes.Domains.Tests.Core;

[TestFixture]
public class HumidityRuleTests
{
    [Test]
    public void should_be_valid_when_humidity_is_below_or_equal_max_humidity()
    {
        var rule = new HumidityRule(20, 10);

        var result = rule.IsValid;

        Assert.That(result, Is.True);
    }

    [Test]
    public void should_be_invalid_when_humidity_is_above_max_humidity()
    {
        var rule = new HumidityRule(20, 999999);

        var result = rule.IsValid;

        Assert.That(result, Is.False);
    }

    [Test]
    public void should_compute_max_humidity_greater_than_zero()
    {
        var rule = new HumidityRule(20, 50);

        var result = rule.MaxHumidity;

        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public void should_compute_humidity_percent_correctly_under_100()
    {
        var rule = new HumidityRule(20, 10);

        var result = rule.HumidityPercent;

        Assert.That(result, Is.GreaterThanOrEqualTo(0));
        Assert.That(result, Is.LessThanOrEqualTo(100));
    }

    [Test]
    public void should_cap_humidity_percent_at_100()
    {
        var rule = new HumidityRule(20, 999999);

        var result = rule.HumidityPercent;

        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void should_return_zero_when_max_humidity_is_zero_or_negative()
    {
        var rule = new HumidityRule(-300, 50);

        var result = rule.HumidityPercent;

        Assert.That(result, Is.EqualTo(0));
    }
}