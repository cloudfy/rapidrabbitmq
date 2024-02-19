namespace RapidRabbitMQ.Services.Abstracts
{
    public interface IRabbitMqBootstrapService
    {
        Task Run();
        void Stop();
    }
}