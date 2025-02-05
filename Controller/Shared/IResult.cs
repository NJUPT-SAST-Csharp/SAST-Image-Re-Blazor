namespace Controller.Shared;

public interface IResult
{
    public bool IsSuccessful { get; init; }
}

public readonly record struct Result(bool IsSuccessful) : IResult
{
    public static readonly Result Success = new(true);
    public static readonly Result Fail = new(false);
}
