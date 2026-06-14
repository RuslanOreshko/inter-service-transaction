using MassTransit;
using Contracts;
using ServiceA.Services;
using ServiceA.GrpcClient;

namespace ServiceA.Consumers;


public class ACompletedConsumer : IConsumer<AComplatedCommand>
{
    private readonly TransactionStateStore _stateStore;
    private readonly TransactionGrpcClient _grpcClient;

    public ACompletedConsumer(
        TransactionStateStore stateStore,
        TransactionGrpcClient grpcClient
    )
    {
        _stateStore = stateStore;
        _grpcClient = grpcClient;
    }

    public async Task Consume(
        ConsumeContext<AComplatedCommand> context
    )
    {
        if (_stateStore.Transaction.TryGetValue(
            context.Message.CorrelaationId,
            out var state))
        {
            state.AComplated = true;

            if (state.AComplated &&
                state.BComplated &&
                !state.GrpcCalled)
            {
                state.GrpcCalled = true;

                var result = await _grpcClient.ValidateTransactionAsync(
                    state.CorrelationId
                );

                Console.WriteLine(
                    $"gRPC result {result}"
                );
            }
        }

        Console.WriteLine(
            $"A complated {context.Message.CorrelaationId}"
        );
    }
}