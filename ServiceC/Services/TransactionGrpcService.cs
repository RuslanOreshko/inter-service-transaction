using Contracts.Grpc;
using Grpc.Core;

namespace ServiceC.Services;

public class TransactionGrpcService : TransactionService.TransactionServiceBase
{
    public override Task<ValidateTransactionResponse>
    ValidateTransaction(
        ValidateTransactionRequest request,
        ServerCallContext context
    )
    {
        return Task.FromResult(
            new ValidateTransactionResponse
            {
                Success = true
            }
        );
    }
}
