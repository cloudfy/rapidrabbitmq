namespace RapidRabbitMQ.Services.Abstracts
{
    public interface IPreperationService
    {
        string RuntimeRoot { get; }

        Task Prepare();
        Task<bool> ValidateReadiness();
    }
}