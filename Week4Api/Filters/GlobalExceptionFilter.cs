using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Week4Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(
                context.Exception,
                "Unhandled exception in {Action}",
                context.ActionDescriptor.DisplayName);

            context.Result = new ObjectResult(new
            {
                error = "An unexpected error occured",
                detail = context.Exception.Message,
                traceId = context.HttpContext.TraceIdentifier
            })
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}
