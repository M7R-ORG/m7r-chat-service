using System.Data;

namespace Chat.Domain.Common;

public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync();
}

public partial interface IUnitOfWork
{
    Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel);
}
