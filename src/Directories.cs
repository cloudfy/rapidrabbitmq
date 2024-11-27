namespace RapidRabbitMQ;

internal static class Directories
{
    internal static string RootDirectory => Directory.GetCurrentDirectory();
    internal static string DataDirectory => 
        Path.Combine(Directory.GetCurrentDirectory(), "data");

    internal static string RuntimeDirectory => 
        Path.Combine(Directory.GetCurrentDirectory(), "runtime");

    internal static string ErlangDirectory => 
        Path.Combine(Directory.GetCurrentDirectory(), "runtime", "erl");
    internal static string RabbitMqSbinDirectory => 
        Path.Combine(Directory.GetCurrentDirectory(), "runtime", "rmq", StaticConstants.RMQ_VERSION, "sbin");
    internal static string RabbitMqDirectory => 
        Path.Combine(Directory.GetCurrentDirectory(), "runtime", "rmq");
    internal static string RabbitMqConfigFile => 
        Path.Combine(Directory.GetCurrentDirectory(), "data", "rabbitmq.config");

}
