using System.Net;
using System.Text.Json;
using ExceptionFlow;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(UserRegistration));

var app = builder.Build();

app.UseExceptionHandler(configure =>
{
    configure.Run(async httpContext =>
    {
        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = 400;

        var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;

        var problemDetails = new ProblemDetails
        {
            Status = (int) HttpStatusCode.BadRequest
        };

        if (exception is not null)
        {
            problemDetails.Title = exception.Message;
        }

        await JsonSerializer.SerializeAsync(httpContext.Response.BodyWriter.AsStream(), problemDetails);
    });
});

app.MapPut("v1/users", async ([FromBody] RegisterUserRequest request, [FromServices] UserRegistration useCase) =>
{
    var result = await useCase.RegisterUserExceptionFlowAsync(request);

    return Results.Ok(result);
});

app.MapPut("v2/users", async ([FromBody] RegisterUserRequest request, [FromServices] UserRegistration useCase) =>
{
    var result = await useCase.RegisterUserAsync(request);

    return result.Match(Results.Ok, Results.BadRequest);
});

app.Run();
