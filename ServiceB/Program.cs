using MassTransit;
using ServiceB.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StartBConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("start-b-queue", e =>
        {
            e.ConfigureConsumer<StartBConsumer>(context);
        });
    });
});


var app = builder.Build();


app.UseHttpsRedirection();

app.Run();


