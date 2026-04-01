namespace MiniWarehouse.Server.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                await Handle(context, 404, ex.Message);
            }
            catch (ArgumentException ex)
            {
                await Handle(context, 400, ex.Message);
            }
            catch (Exception)
            {
                await Handle(context, 500, "Neočekávaná chyba.");
            }
        }

        private static async Task Handle(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                message
            });
        }
    }
}
