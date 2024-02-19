using RapidRabbitMQ.Commands.Abstracts;
using RapidRabbitMQ.Services.Abstracts;

namespace RapidRabbitMQ.Commands;
internal class PrepareCommand
    : Command<PrepareCommand.PrepareCommandOptions, PrepareCommand.PrepareCommandHandler>
{
    public PrepareCommand() : base("prepare", "Downloads dependencies and completed the setup to run RabbitMQ.")
    {
    }

    internal class PrepareCommandOptions
        : ICommandOptions
    {
    }

    internal class PrepareCommandHandler
        : ICommandOptionsHandler<PrepareCommandOptions>
    {
        private readonly IPreperationService _preperationService;

        public PrepareCommandHandler(IPreperationService preperationService)
        {
            _preperationService = preperationService;
        }

        public async Task<int> HandleAsync(PrepareCommandOptions options, CancellationToken cancellationToken)
        {
            await _preperationService.Prepare();

            return 0;
        }
    }
}
