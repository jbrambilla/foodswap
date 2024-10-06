using Carter;
using Microsoft.AspNetCore.Identity;

namespace foodswap.Common.Api;
public abstract class BaseEndpoint : CarterModule
{
    public BaseEndpoint(string basePath) 
        : base(basePath)
    {
    }
    protected IResult Ok<T>(T data, string message = "")
    {
        return Results.Ok(
            new ApiResponse<T>(true, message, data)
        );
    }

    protected IResult Created<T>(T data, string message = "")
    {
        return Results.Created(
            string.Empty,
            new ApiResponse<T>(true, message, data)
        );
    }

    protected IResult BadRequest(List<string> errors, string message = "")
    {
        return Results.BadRequest(
            new ApiResponse<object>(false, message, null!, errors)
        );
    }

    protected IResult BadRequest(IEnumerable<IdentityError> errors, string message = "")
    {
        return Results.BadRequest(
            new ApiResponse<object>(false, message, null!, errors.Select(x => x.Description).ToList())
        );
    }
}