namespace Teisti.Application.Abstractions;

public interface IQueryHandler<in TQ, TR>
{
    Task<TR> HandleAsync(TQ query, CancellationToken cancellationToken = default);
}
