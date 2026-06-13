using MassTransit;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers;


[ApiController]
[Route("api/transaction")]
public class TransactionController : ControllerBase
{
    private readonly ISendEndpointProvider _sendEndpointPrivider;
    
    public TransactionController(
        ISendEndpointProvider sendEndpointProvider
    )
    {
        _sendEndpointPrivider = sendEndpointProvider;
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