using BioDomes.Web.Middlewares;

namespace BioDomes.Web.Extensions;

public static class MiddlewareExtensions
{
    public static void UseReverseProxyMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ReverseProxyLinksMiddleware>();
    }
}