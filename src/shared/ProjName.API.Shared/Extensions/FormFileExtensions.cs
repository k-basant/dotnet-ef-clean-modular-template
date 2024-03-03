namespace ProjName.API.Shared.Extensions;

public static class FormFileExtensions
{
    public static FileIM ToFileModel(this IFormFile file)
    {
        return new FileIM(file.OpenReadStream(), file.ContentType, file.FileName);
    }
}
