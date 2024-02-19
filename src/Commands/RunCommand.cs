using RapidRabbitMQ.Commands.Abstracts;
using RapidRabbitMQ.Services.Abstracts;
using System.CommandLine;

namespace RapidRabbitMQ.Commands;

internal class RunCommand
    : Command<RunCommand.RunCommandOptions, RunCommand.RunCommandHandler>
{
    public RunCommand() : base("run", "Run RabbitMQ portable.")
    {
        AddOption(new Option<bool>("--management", "Enable or disable the management UI at 'http://localhost:15672/'."));
        AddOption(new Option<bool>("--detached", "Run RabbitMQ in detached mode."));
    }

    internal class RunCommandOptions
        : ICommandOptions
    {
        public bool Management { get; set; } = true;
        public bool Detached { get; set; } = false;
    }

    internal class RunCommandHandler(
        IRabbitMqBootstrapService rabbitMq
        , IPreperationCheckHandler preperationCheckHandler)
        : ICommandOptionsHandler<RunCommandOptions>
    {
        private readonly IRabbitMqBootstrapService _rabbitMq = rabbitMq;
        private readonly IPreperationCheckHandler _preperationCheckHandler = preperationCheckHandler;

        public async Task<int> HandleAsync(RunCommandOptions options, CancellationToken cancellationToken)
        {
            if (!await _preperationCheckHandler.ValidateReadiness())
            {
                Console.WriteLine("Please prepare rapidrabbitmq by using command 'rapidrabbitmq prepare'.");
                return 1;
            }
            if (options.Management)
            {
                var pluginsFile = Path.Combine(Directories.DataDirectory, "enabled_plugins");
                if (!File.Exists(pluginsFile))
                {
                    ColorConsole.WriteWarning("Enabling RabbitMQ management plugins...");
                    await File.WriteAllTextAsync(pluginsFile, "[rabbitmq_management,rabbitmq_web_mqtt,rabbitmq_web_stomp].\r\n");
                }
            }

            await _rabbitMq.Run();
            Console.ReadLine();

            ColorConsole.WriteWarning("Stopping RabbitMQ...");
            _rabbitMq.Stop();

            return 0;
        }
    }
}
