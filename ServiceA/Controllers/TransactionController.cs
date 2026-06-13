using MassTransit;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using ServiceA.Services;
using ServiceA.Models;

namespace ServiceA.Controllers;


[ApiController]
[Route("api/transaction")]
public class TransactionController : ControllerBase
{
    private readonly ISendEndpointProvider _sendEndpointPrivider;
    private readonly TransactionStateStore _stateStore;
    
    public TransactionController(
        ISendEndpointProvider sendEndpointProvider,
        TransactionStateStore stateStore
    )
    {
        _sendEndpointPrivider = sendEndpointProvider;
        _stateStore = stateStore;
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start()
    {
        var correlationId = Guid.NewGuid();

        var aEndpoint = await _sendEndpointPrivider.GetSendEndpoint(
            new Uri("queue:start-a-queue")
        );

        var bEndpoint = await _sendEndpointPrivider.GetSendEndpoint(
            new Uri("queue:start-b-queue")
        );

        _stateStore.Transaction[correlationId] = 
            new TransactionState
            {
                CorrelationId = correlationId
            };

        await aEndpoint.Send(
            new StartACommand(correlationId)
        );

        await bEndpoint.Send(
            new StartBCommand(correlationId)
        );

        return Ok(new
        {
            CorrelationId = correlationId
        });
    }
}