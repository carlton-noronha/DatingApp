using System.Net;
using System.Text.Json;
using DatingAppAPI.Errors;

namespace DatingAppAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this._env = env;
            this._logger = logger;
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context) { // Name needs to be InvokeAsync
            try {
                await _next(context);
            } catch(Exception ex) {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                APIException response = _env.IsDevelopment() ? new APIException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) : new APIException(context.Response.StatusCode, ex.Message, "Internal Server Error");
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, options: new JsonSerializerOptions {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
            }
        }
    }
}