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
    private readonly TransactionComplateStore _complatStore;
    
    public TransactionController(
        ISendEndpointProvider sendEndpointProvider,
        TransactionStateStore stateStore,
        TransactionComplateStore complatStore
    )
    {
        _sendEndpointPrivider = sendEndpointProvider;
        _stateStore = stateStore;
        _complatStore = complatStore;
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start()
    {
        var correlationId = Guid.NewGuid();

        var tcs = new TaskCompletionSource<bool>();

        _complatStore.PendingTransaction[correlationId] = tcs;

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

        var completedTask = await Task.WhenAny(
            tcs.Task,
            Task.Delay(TimeSpan.FromSeconds(10))
        );

        if(completedTask != tcs.Task)
        {
            return StatusCode(504, new
            {
                CorrelationId = correlationId,
                Message = "Transaction timeout"
            });
        }

        var result = await tcs.Task;

        return Ok(new
        {
            CorrelationId = correlationId,  
            Success = result
        });
    }
}