namespace PandaWebApi.Configurations;

public static class CertificateReader
{
    public static void ReadCertificatesFromEnvironments(this WebApplicationBuilder builder,
        string directory = "Certificate/")
    {
        if (builder.Environment.IsDevelopment()) return;

        EnsureDirectoryExists(directory);
        ReadTextFromEnvironmentAndSaveToFile("CERTIFICATE", Path.Combine(directory, "certificate.cer"));
        ReadTextFromEnvironmentAndSaveToFile("CERTIFICATE_KEY", Path.Combine(directory, "certificate.key"));
    }

    private static void EnsureDirectoryExists(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private static void ReadTextFromEnvironmentAndSaveToFile(string environmentVariableName, string fileName)
    {
        File.WriteAllText(fileName, Environment.GetEnvironmentVariable(environmentVariableName));
    }
}