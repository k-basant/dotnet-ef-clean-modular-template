using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace ProjName.Application.Shared.Extensions;

public static class HttpExtensions
{
    public static string ToQueryString(this object obj)
    {
        if (obj == null)
        {
            return string.Empty;
        }

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(p => p.GetValue(obj) != null);
        var queryParams = new StringBuilder("?");

        bool first = true;
        foreach (var property in properties)
        {
            if (!first) queryParams.Append("&");
            queryParams.AppendFormat("{0}={1}", Uri.EscapeDataString(property.Name), Uri.EscapeDataString(property.GetValue(obj).ToString()));
            first = false;
        }

        return queryParams.ToString();
    }
    public static string ToJsonString(this object obj)
    {
        if (obj == null)
        {
            return "{}";
        }
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }
}
