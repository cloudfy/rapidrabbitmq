
using System.CommandLine.Builder;
using System.CommandLine;
using RapidRabbitMQ.Commands;
using RapidRabbitMQ.Builder;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using RapidRabbitMQ.Services.Abstracts;
using RapidRabbitMQ.Services;
using Microsoft.Extensions.Logging;
using RapidRabbitMQ;

var rootCommand = new RootCommand("Use RapidRabbitMQ to run RabbitMQ without any installation. This portable file option, enable quick execution.")
{
    new PrepareCommand()
    , new RunCommand()
    , new CleanCommand()
};

// build and run
var builder = new CommandLineBuilder(rootCommand)
    .UseDefaults()
    .UseDependencyInjection(services =>
{
    services.AddLogging(options => {
        options.AddConsole();
#if DEBUG
        options.SetMinimumLevel(LogLevel.Debug);
#else
        options.SetMinimumLevel(LogLevel.Warning);
#endif
    });
    services.AddHttpClient();

    services.AddSingleton<IPreperationCheckHandler, PreperationCheckHandler>();
    services.AddSingleton<IPreperationService, PreperationService>();
    services.AddSingleton<IRabbitMqBootstrapService, RabbitMqBootstrapService>();
    //services.addh
});

ColorConsole.WriteInformation("\nWelcome to RapidRabbitMQ v1.0");
return await builder
    .Build()
    .InvokeAsync(args);

//    // run
//    // C:\dev\Repos\rapidrabbitmq\src\bin\Debug\net8.0\runtime\erl\erts-14.2.2\bin>epmd -daemon 
//    //..then ..
//    // C:\dev\Repos\rapidrabbitmq\src\bin\Debug\net8.0\runtime\rmq\sbin>rabbitmq-server.bat
