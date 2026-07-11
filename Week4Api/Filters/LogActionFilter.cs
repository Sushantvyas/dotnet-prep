using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Week4Api.Filters
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        private Stopwatch _sw = new();

        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _sw = Stopwatch.StartNew();
            _logger.LogInformation(
                "--> Action starting: {Action} | Args: {Args}",
                context.ActionDescriptor.DisplayName,
                string.Join(", ", context.ActionArguments
                    .Select(a => $"{a.Key}={a.Value}")));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _sw.Stop();
            _logger.LogInformation(
                "<-- Action completed: {Action} | {Ms}ms | Status: {Status}",
                context.ActionDescriptor.DisplayName,
                _sw.ElapsedMilliseconds,
                (context.Result as ObjectResult)?.StatusCode ?? 200);
        }
    }
}
