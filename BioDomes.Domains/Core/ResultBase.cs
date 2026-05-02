namespace BioDomes.Domains.Core;

public abstract class ResultBase
{
    public string? Error { get; }
    public ResultStatus Status { get; }
    public bool Failed => Status != ResultStatus.Success;

    protected ResultBase(ResultStatus status, string? error = null)
    {
        Status = status;
        Error = error;
    }
}
