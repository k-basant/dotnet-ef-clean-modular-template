using System.Text;

namespace ProjName.Application.Shared.Extensions;

#nullable enable
public static class StringExtensions
{
    public static bool IsNotNullOrEmpty(this string source)
    {
        return !string.IsNullOrEmpty(source);
    }
    public static bool IsNullOrEmpty(this string source)
    {
        return string.IsNullOrEmpty(source);
    }
    /// <summary>
    /// Lowers the first character of the <paramref name="input"/>.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToCamelCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        if (input.Length == 1)
        {
            return input.ToLowerInvariant();
        }

        return char.ToLowerInvariant(input[0]) + input.Substring(1);
    }
    public static string ToBase64Str(this string text)
    {
        return Convert.ToBase64String(Encoding.ASCII.GetBytes(text));
    }
    public static FileVM ToFile(this string data, string name, string contentType)
    {
        var memoryStream = new MemoryStream();
        var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);

        streamWriter.Write(data);
        streamWriter.Flush();

        // Reset the memory stream position to the beginning.
        memoryStream.Position = 0;

        return new FileVM(memoryStream, contentType, name);
    }
    public static T ParseAsTypedObject<T>(this string text)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(text)!;
    }
}