using MassTransit;
using Contracts;


namespace ServiceB.Consumers;

public class StartBConsumer : IConsumer<StartBCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public StartBConsumer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(
        ConsumeContext<StartBCommand> context
    )
    {
        Console.WriteLine(
            $"B received {context.Message.CorrelationID}"
        );

        await _publishEndpoint.Publish(
            new BComplatedCommand(
                context.Message.CorrelationID,
                true
            )
        );
    }
}