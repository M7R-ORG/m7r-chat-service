using System.Data;
using Chat.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Chat.Persistence.UnitOfWork;

public partial class UnitOfWork
{
    public async Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
    {
        IDbContextTransaction transaction = await _eFContext.Database.BeginTransactionAsync(isolationLevel);
        return new TransactionWrapper(transaction);
    }

    private class TransactionWrapper(IDbContextTransaction transaction) : ITransaction
    {
        public Task CommitAsync() => transaction.CommitAsync();
        public ValueTask DisposeAsync() => transaction.DisposeAsync();
    }
}
