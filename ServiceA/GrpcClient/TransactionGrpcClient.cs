using Contracts.Grpc;
using Grpc.Core;

namespace ServiceA.GrpcClient;

public class TransactionGrpcClient
{
    private readonly TransactionService.TransactionServiceClient _client;

    public TransactionGrpcClient(
        TransactionService.TransactionServiceClient client
    )
    {
        _client = client;
    }

    public async Task<bool> ValidateTransactionAsync(
        Guid correlationId
    )
    {
        var response = await _client.ValidateTransactionAsync(
            new ValidateTransactionRequest
            {
                CorrelationId = correlationId.ToString()
            }
        );

        return response.Success;
    }
}