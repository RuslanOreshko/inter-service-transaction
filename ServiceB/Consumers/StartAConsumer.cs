using MassTransit;
using Contracts;


namespace ServiceB.Consumers;

public class StartBConsumer : IConsumer<StartBCommand>
{
    public Task Consume(
        ConsumeContext<StartBCommand> context
    )
    {
        Console.WriteLine(
            $"B received {context.Message.CorrelationID}"
        );

        return Task.CompletedTask; 
    }
}