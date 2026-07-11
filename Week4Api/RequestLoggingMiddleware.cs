using System.Diagnostics;
namespace Week4Api
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = new Stopwatch();
            var method = context.Request.Method;
            var path = context.Request.Path;
            Console.WriteLine($"[LOG] --> {method} {path}");

            await _next(context);
            sw.Stop();
            Console.WriteLine($"[LOG] {method} {path} " +
                $"{context.Response.StatusCode} ({sw.ElapsedMilliseconds}ms)");
        }
    }
}
