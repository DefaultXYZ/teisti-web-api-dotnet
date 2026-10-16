namespace Teisti.Application.Database;

public interface IEntityDbContext
{
    Task ApplyChangesAsync(CancellationToken cancellationToken = default);
}
