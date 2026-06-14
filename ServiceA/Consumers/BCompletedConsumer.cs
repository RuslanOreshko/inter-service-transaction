using MassTransit;
using Contracts;
using ServiceA.Services;
using ServiceA.GrpcClient;


namespace ServiceA.Consumers;


public class BCompletedConsumer : IConsumer<BComplatedCommand>
{

    private readonly TransactionStateStore _stateStore;
    private readonly TransactionGrpcClient _grpcClient;
    private readonly TransactionComplateStore _complateStore;


    public BCompletedConsumer(
        TransactionStateStore stateStore,
        TransactionGrpcClient grpcClient,
        TransactionComplateStore complateStore
    )
    {
        _stateStore = stateStore;
        _grpcClient = grpcClient;
        _complateStore = complateStore;
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
                state.BComplated)
            {
                if (Interlocked.Exchange(
                        ref state.GrpcCalled,
                        1) == 1)
                {
                    return;
                }

                try
                {
                    var result = await _grpcClient.ValidateTransactionAsync(
                        state.CorrelationId
                    );

                    if(_complateStore.PendingTransaction.TryGetValue(
                        state.CorrelationId,
                        out var tcs
                    ))
                    {
                        tcs.TrySetResult(result);

                        _complateStore.PendingTransaction.TryRemove(
                            state.CorrelationId,
                            out _
                        );
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(
                        $"gRPC error: {ex.Message}"
                    );

                    
                    if(_complateStore.PendingTransaction.TryGetValue(
                        state.CorrelationId,
                        out var tcs
                    ))
                    {
                        tcs.SetResult(false);

                        _complateStore.PendingTransaction.TryRemove(
                            state.CorrelationId,
                            out _
                        );
                    }
                }
                
            }
        }

        Console.WriteLine(
            $"B complated {context.Message.CorrelaationId}"
        );
    }
}