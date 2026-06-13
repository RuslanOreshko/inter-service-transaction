namespace Contracts;

public record BComplatedCommand(
    Guid CorrelaationId,
    bool Success
);