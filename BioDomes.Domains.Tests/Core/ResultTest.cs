using BioDomes.Domains.Core;

namespace BioDomes.Domains.Tests.Core;

[TestFixture]
public class ResultTests
{
    [Test]
    public void should_return_success_result_when_calling_ok()
    {
        var result = Result.Ok();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Error, Is.Null);
    }

    [Test]
    public void should_return_not_found_result_with_error()
    {
        var result = Result.NotFound("not found");

        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound));
        Assert.That(result.Error, Is.EqualTo("not found"));
    }

    [Test]
    public void should_return_forbidden_result_with_error()
    {
        var result = Result.Forbidden("forbidden");

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
        Assert.That(result.Error, Is.EqualTo("forbidden"));
    }

    [Test]
    public void should_return_conflict_result_with_error()
    {
        var result = Result.Conflict("conflict");

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Conflict));
        Assert.That(result.Error, Is.EqualTo("conflict"));
    }

    [Test]
    public void should_return_failure_result_with_error()
    {
        var result = Result.Failure("failure");

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
        Assert.That(result.Error, Is.EqualTo("failure"));
    }
}