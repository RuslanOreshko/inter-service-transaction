using MassTransit;
using Contracts;
using ServiceA.Services;

namespace ServiceA.Consumers;


public class BCompletedConsumer : IConsumer<BComplatedCommand>
{

    private readonly TransactionStateStore _stateStore;

    public BCompletedConsumer(
        TransactionStateStore stateStore
    )
    {
        _stateStore = stateStore;
    }

    public Task Consume(
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
                Console.WriteLine(
                    $"Transaction completed: {state.CorrelationId}");
            }
        }

        Console.WriteLine(
            $"B complated {context.Message.CorrelaationId}"
        );

        return Task.CompletedTask;
    }
}