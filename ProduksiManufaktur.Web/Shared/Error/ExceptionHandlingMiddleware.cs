using System.Net;

namespace ProduksiManufaktur.Web.Shared
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode >= HttpStatusCode.BadRequest)
                {
                    context.Response.Redirect("/error");
                    return;
                }
            }
            catch (JsonException)
            {
                var cookies = context.Request.Cookies;
                foreach (var cookie in cookies.Keys) context.Response.Cookies.Delete(cookie);
                context.Response.Redirect("/Account/Login");
                return;
            }
        }
    }
}