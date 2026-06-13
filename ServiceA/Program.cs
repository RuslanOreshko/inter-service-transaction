using MassTransit;
using Scalar.AspNetCore;
using ServiceA.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StartAConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("start-a-queue", e =>
        {
            e.ConfigureConsumer<StartAConsumer>(context);
        });
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();