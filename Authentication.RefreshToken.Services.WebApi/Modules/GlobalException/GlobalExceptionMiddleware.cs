using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Authentication.RefreshToken.Services.WebApi.Modules.GlobalException
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly JsonSerializerOptions _jsonOptions;
        public GlobalExceptionMiddleware(
            ILogger<GlobalExceptionMiddleware> logger,
            IOptions<JsonOptions> jsonOptions)
        {
            _logger = logger;
            _jsonOptions = jsonOptions.Value.SerializerOptions;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                await WriteResponseAsync(context, ex.Message, StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedException ex)
            {
                await WriteResponseAsync(context, ex.Message, StatusCodes.Status401Unauthorized);
            }
            catch (ConflictException ex)
            {
                await WriteResponseAsync(context, ex.Message, StatusCodes.Status409Conflict);
            }
            catch (ValidationExceptionCustom ex)
            {
                await WriteValidationResponseAsync(context,ex.Message, ex.Errors!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                await WriteResponseAsync(context, "Internal Server Error", StatusCodes.Status500InternalServerError);
            }
        }

        private async Task WriteResponseAsync(HttpContext context, string message , int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var response = new ErrorResponse(message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
        }

        private async Task WriteValidationResponseAsync(
            HttpContext context,
            string message,
            IEnumerable<BaseError> errors)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var response = new ErrorResponse(message, errors);

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
        }
    }
}
