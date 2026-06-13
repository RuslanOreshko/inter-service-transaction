namespace Contracts;

public record AComplatedCommand(
    Guid CorrelaationId,
    bool Success
);