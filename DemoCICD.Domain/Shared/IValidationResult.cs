namespace DemoCICD.Domain.Shared;

public interface IValidationResult
{
    static readonly Error ValidationError = new(
       "ValidationError",
       "A validation problem occurred.");
    Error[] Errors { get; }
}
