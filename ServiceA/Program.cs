using Contracts.Grpc;
using MassTransit;
using Scalar.AspNetCore;
using ServiceA.Consumers;
using ServiceA.GrpcClient;
using ServiceA.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<TransactionStateStore>();
builder.Services.AddScoped<TransactionGrpcClient>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StartAConsumer>();

    x.AddConsumer<ACompletedConsumer>();
    x.AddConsumer<BCompletedConsumer>();

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

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddGrpcClient<TransactionService.TransactionServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5106");
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