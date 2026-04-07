using System.Text.RegularExpressions;

namespace BioDomes.Web.Constraints;

public class KebabRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.TryGetValue(routeKey, out var value) || value == null)
            return false;

        return Regex.IsMatch(value.ToString()!, @"^[a-z0-9]+(-[a-z0-9]+)*$");
    }
}