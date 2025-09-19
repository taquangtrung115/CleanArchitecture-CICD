using MediatR;
using Serilog;
using System.Diagnostics;

namespace DemoCICD.Application.Behaviors;

public class TracingPipelineBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;

    public TracingPipelineBehavior()
    {
        _timer = new Stopwatch();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        var requestName = typeof(TRequest).Name;
        Log.Information("Request Details: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", requestName, elapsedMilliseconds, request);

        return response;
    }
}
