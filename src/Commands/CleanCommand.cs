using RapidRabbitMQ.Commands.Abstracts;
using System.Diagnostics;

namespace RapidRabbitMQ.Commands;

internal class CleanCommand
    : Command<CleanCommand.CleanCommandOptions, CleanCommand.CleanCommandHandler>
{
    public CleanCommand() : base("clean", "Cleans any dependencies, data and folders.")
    {
    }

    internal class CleanCommandOptions
        : ICommandOptions
    {

    }

    
    internal class CleanCommandHandler
        : ICommandOptionsHandler<CleanCommandOptions>
    {

        public CleanCommandHandler()
        {
        }

        public Task<int> HandleAsync(CleanCommandOptions options, CancellationToken cancellationToken)
        {
            ColorConsole.WriteError("Removing dependencies and data...");

            if (Directory.Exists(Directories.RuntimeDirectory))
                Directory.Delete("runtime", true);
            if (File.Exists(Path.Combine(Directories.DataDirectory, ".erlang.cookie")))
            {
                File.SetAttributes(Path.Combine(Directories.DataDirectory, ".erlang.cookie"), FileAttributes.Normal);
            }
            if (Directory.Exists(Directories.DataDirectory))
                Directory.Delete("data", true);

            ColorConsole.WriteSuccess("Dependencies and data removed.");

            return Task.FromResult(0);
        }
    }
}
