﻿namespace RapidRabbitMQ.Commands.Abstracts;

public interface ICommandOptionsHandler<in TOptions>
{
    Task<int> HandleAsync(TOptions options, CancellationToken cancellationToken);
}