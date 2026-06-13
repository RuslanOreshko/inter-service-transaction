using System.Collections.Concurrent;
using ServiceA.Models;

namespace ServiceA.Services;

public class TransactionStateStore
{
    public ConcurrentDictionary<Guid, TransactionState> Transaction { get; } = new();
}