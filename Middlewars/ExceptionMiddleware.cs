using Questions_and_Answers_API.Exceptions;

namespace Questions_and_Answers_API.Middlewars
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch(BadRequestException ex)
            {
                httpContext.Response.StatusCode = 400;              
                await httpContext.Response.WriteAsync(ex.Message);
                return;
            }
            catch (Exception)
            {
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync("Something went wrong!");
                return;
            }
        }
    }
}
