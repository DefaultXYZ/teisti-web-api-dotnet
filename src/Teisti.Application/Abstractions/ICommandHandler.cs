namespace Teisti.Application.Abstractions;

public interface ICommandHandler<in TC, TR>
{
    Task<TR> HandleAsync(TC command, CancellationToken cancellationToken = default);
}
