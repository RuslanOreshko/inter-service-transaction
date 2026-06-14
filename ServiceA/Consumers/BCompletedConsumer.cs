using MassTransit;
using Contracts;
using ServiceA.Services;
using ServiceA.GrpcClient;


namespace ServiceA.Consumers;


public class BCompletedConsumer : IConsumer<BComplatedCommand>
{

    private readonly TransactionStateStore _stateStore;
    private readonly TransactionGrpcClient _grpcClient;


    public BCompletedConsumer(
        TransactionStateStore stateStore,
        TransactionGrpcClient grpcClient
    )
    {
        _stateStore = stateStore;
        _grpcClient = grpcClient;
    }

    public async Task Consume(
        ConsumeContext<BComplatedCommand> context
    )
    {
        if (_stateStore.Transaction.TryGetValue(
            context.Message.CorrelaationId,
            out var state))
        {
            state.BComplated = true;

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
            $"B complated {context.Message.CorrelaationId}"
        );
    }
}