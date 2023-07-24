using System.Text;

namespace WebApiPostgres.Configurations;

public static class CertificateConfiguration
{
    public static void CreateCertificate()
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") return;
        
        if (!Directory.Exists(@"./certs"))
        {
            Directory.CreateDirectory("./certs");
        }

        var path = @"./certs/be-tmp-web-api-postgres.cer"; // path to file
        var cs = Environment.GetEnvironmentVariable("CERTIFICATE");
        if (cs != null)
        {
            using var fs = File.Create(path);
            var info = new UTF8Encoding(true).GetBytes(cs ?? throw new InvalidOperationException());
            fs.Write(info, 0, info.Length);
        }

        path = @"./certs/be-tmp-web-api-postgres.key.txt"; // path to file
        cs = Environment.GetEnvironmentVariable("CERTIFICATE_KEY");
        if (cs == null) return;
        {
            using var fs = File.Create(path);
            var info = new UTF8Encoding(true).GetBytes(cs ?? throw new InvalidOperationException());
            fs.Write(info, 0, info.Length);
        }
    }
}