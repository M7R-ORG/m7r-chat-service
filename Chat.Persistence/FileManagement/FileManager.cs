namespace Chat.Persistence.Extensions;

public static class FileManager
{
    public static async Task<string?> WriteToFileAsync(
        byte[]? file,
        string path,
        string fileName
    )
    {
        string fullName = $"{Guid.NewGuid()}.{fileName}.{DateTime.UtcNow:ddMMyyyy.HHmm}";
        string fullPath = $"{path}/{fullName}";

        if (file == null || file.Length <= 0)
            return null;

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        await File.WriteAllBytesAsync(fullPath, file);

        return fullPath;
    }

    public static bool RemoveFile(string filePath)
    {
        if (!File.Exists(filePath))
            return false;

        File.Delete(filePath);

        return true;
    }

    public static async Task<byte[]?> ReadToBytesAsync(string? path)
    {
        if (path == null || !File.Exists(path))
            return null;

        return await File.ReadAllBytesAsync(path);
    }

    public static byte[]? ReadToBytes(string? path)
    {
        if (path == null || !File.Exists(path))
            return null;

        return File.ReadAllBytes(path);
    }
}
