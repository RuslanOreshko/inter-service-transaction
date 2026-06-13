using MassTransit;
using Contracts;
using System.Runtime.CompilerServices;

namespace ServiceA.Consumers;

public class StartAConsumer : IConsumer<StartACommand>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public StartAConsumer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(
        ConsumeContext<StartACommand> context
    )
    {
        Console.WriteLine(
            $"A received {context.Message.CorrelationID}"
        );
        
        await _publishEndpoint.Publish(
            new AComplatedCommand(
                context.Message.CorrelationID,
                true
            )
        );
    }
}