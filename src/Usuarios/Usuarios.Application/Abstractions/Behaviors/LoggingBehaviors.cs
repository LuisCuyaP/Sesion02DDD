﻿using MediatR;
using Microsoft.Extensions.Logging;
using Usuarios.Application.Abstractions.Messaging;

namespace Usuarios.Application.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        try
        {
            _logger.LogInformation($"Ejecutando commando: {requestName}");
            var response = await next();
            _logger.LogInformation($"Commando ejecutado: {requestName}");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Commando: {requestName} falló");
            throw;
        }
    }
}