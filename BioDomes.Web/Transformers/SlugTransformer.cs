using System.Text.RegularExpressions;

namespace BioDomes.Web.Transformers;

public class SlugTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) return null;
        
        return Regex.Replace(value.ToString(), @"[\s\p{P}]", "-").ToLower();

    }
}