namespace RapidRabbitMQ.Services.Abstracts
{
    public interface IPreperationCheckHandler
    {
        /// <summary>
        /// Validate the readiness of the portable execution.
        /// </summary>
        /// <returns>True if dependencies is ready. False if preperation must be completed.</returns>
        Task<bool> ValidateReadiness();
    }
}