namespace BioDomes.Web.Middlewares.extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseReverseProxyMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ReverseProxyLinksMiddleware>();
    }
}