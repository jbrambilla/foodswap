namespace foodswap.Common.Extensions;

public static class OpenApiExtensions
{
    public static RouteHandlerBuilder WithSummaryAndDescription(this RouteHandlerBuilder builder, string summary, string description)
    {
        return builder.WithOpenApi(operations =>
        {
            operations.Summary = summary;
            operations.Description = description;
            return operations;
        });
    }

    public static RouteHandlerBuilder WithIdDescription(this RouteHandlerBuilder builder, string idDescription)
    {
        return builder.WithOpenApi(operations =>
        {
            var parameters = operations.Parameters[0];
            parameters.Description = idDescription;
            return operations;
        });
    }
}