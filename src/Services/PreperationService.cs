using System.IO.Compression;
using RapidRabbitMQ.Services.Abstracts;

namespace RapidRabbitMQ.Services;

public class PreperationService(
    IHttpClientFactory httpClientFactory
    , IPreperationCheckHandler preperationCheckHandler) : IPreperationService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IPreperationCheckHandler _preperationCheckHandler = preperationCheckHandler;

    public Task<bool> ValidateReadiness()
        => _preperationCheckHandler.ValidateReadiness();

    public async Task Prepare()
    {
        if (!Contains("erl"))
        {
            ColorConsole.WriteWarning("Setting up dependency Erlang...");
            var erlFolder = await DownloadArtifact(@"https://raw.githubusercontent.com/cloudfy/rapidrabbitmq/main/dep/erl.bin", "erl");

            Console.WriteLine("Configuring dependency Erlang...");
            string iniSource = erlFolder + "/bin/erl.ini.src";
            string iniDestination = erlFolder + "/bin/erl.ini";
            var iniContent = await File.ReadAllTextAsync(iniSource);
            iniContent = iniContent.Replace("{{path}}", (Directory.GetCurrentDirectory() + "\\runtime\\erl").Replace("\\", "\\\\"));
            if (File.Exists(iniDestination)) File.Delete(iniDestination);
            await File.WriteAllTextAsync(iniDestination, iniContent);

            ColorConsole.WriteSuccess("Erlang is ready");
        }
        if (!Contains("rmq"))
        {
            ColorConsole.WriteWarning("Setting up dependency RabbitMQ...");
            await DownloadArtifact(@"https://raw.githubusercontent.com/cloudfy/rapidrabbitmq/main/dep/rmq.bin", "rmq");

            ColorConsole.WriteSuccess("RabbitMQ is ready");
        }
    }

    private static bool Contains(string folder)
    {
        return Directory.Exists(Path.Combine(Directories.RuntimeDirectory, folder));
    }

    public string RuntimeRoot => Directories.RuntimeDirectory;

    private async Task<string> DownloadArtifact(string downloadUri, string folder)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(downloadUri);
        response.EnsureSuccessStatusCode();

        string dependencyFolder = Path.Combine(Directories.RuntimeDirectory, folder);
        Directory.CreateDirectory(dependencyFolder);
        using (var fileStream = new FileStream($"{dependencyFolder}/temp.zip", FileMode.Create))
        {
            await response.Content.CopyToAsync(fileStream);
            fileStream.Position = 0;
            ZipFile.ExtractToDirectory(fileStream, dependencyFolder);
        }
        File.Delete(dependencyFolder + "/temp.zip");

        return dependencyFolder;
    }
}
