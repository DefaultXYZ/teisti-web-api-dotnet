using Teisti.Domain.Providers;

namespace Teisti.Infrastructure.Providers;

public class IdProvider : IIdProvider
{
    public Guid NewId()
    {
        return Guid.CreateVersion7();
    }
}
