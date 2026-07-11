namespace Week4Api
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKey = "my-secret-key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip key check for health endpoint:
            if (context.Request.Path.StartsWithSegments("/health"))
            {
                await _next(context);
                return;
            }

            var key = context.Request.Headers["X-Api-Key"].ToString();

            if (key != ApiKey)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(
                    "Unauthorized: missing or invalid API key");
                return; // short-circuit — _next never called
            }

            await _next(context); // valid key — continue pipeline
        }
    }
}
