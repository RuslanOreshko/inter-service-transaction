using MassTransit;
using Contracts;

namespace ServiceA.Consumers;

public class StartAConsumer : IConsumer<StartACommand>
{
    public Task Consume(
        ConsumeContext<StartACommand> context
    )
    {
        Console.WriteLine(
            $"A received {context.Message.CorrelationID}"
        );

        return Task.CompletedTask; 
    }
}