using System.Collections.Concurrent;

namespace ServiceA.Services;

public class TransactionComplateStore
{
    public ConcurrentDictionary<Guid, TaskCompletionSource<bool>> PendingTransaction { get; } = new();
}