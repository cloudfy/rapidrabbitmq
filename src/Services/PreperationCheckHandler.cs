using RapidRabbitMQ.Services.Abstracts;

namespace RapidRabbitMQ.Services;

public class PreperationCheckHandler : IPreperationCheckHandler
{
    public Task<bool> ValidateReadiness()
    {
        if (Directory.Exists(Directories.RuntimeDirectory))
        {
            return Task.FromResult(true);
        }
        else
        {
            return Task.FromResult(false);
        }
    }
}
