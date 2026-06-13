using MassTransit;
using Contracts;
using ServiceA.Services;

namespace ServiceA.Consumers;


public class ACompletedConsumer : IConsumer<AComplatedCommand>
{
    private readonly TransactionStateStore _stateStore;

    public ACompletedConsumer(
        TransactionStateStore stateStore
    )
    {
        _stateStore = stateStore;
    }

    public Task Consume(
        ConsumeContext<AComplatedCommand> context
    )
    {
        if (_stateStore.Transaction.TryGetValue(
            context.Message.CorrelaationId,
            out var state))
        {
            state.AComplated = true;

            if (state.AComplated &&
                state.BComplated)
            {
                Console.WriteLine(
                    $"Transaction completed: {state.CorrelationId}");
            }
        }

        Console.WriteLine(
            $"A complated {context.Message.CorrelaationId}"
        );

        return Task.CompletedTask;
    }
}