namespace ServiceA.Models;

public class TransactionState
{
    public Guid CorrelationId { get; set; }
    public bool AComplated { get; set; }
    public bool BComplated { get; set; }
}