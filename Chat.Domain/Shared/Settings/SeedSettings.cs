namespace Chat.Domain.Shared.Settings;

public class SeedSettings : ISettings
{
    public static string Path => "Seed";

    public required string Password { get; set; }
}
