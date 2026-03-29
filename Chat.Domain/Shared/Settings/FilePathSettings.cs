namespace Chat.Domain.Shared.Settings;

public class FilePathSettings : ISettings
{
    public static string Path => "FilePath";

    public string Image { get; set; } = null!;
    public string File { get; set; } = null!;

}
