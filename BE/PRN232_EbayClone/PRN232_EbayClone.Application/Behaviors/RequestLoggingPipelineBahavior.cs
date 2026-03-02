using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PRN232_EbayClone.Domain.Shared.Results;
using Serilog.Context;

namespace PRN232_EbayClone.Application.Behaviors
{
    internal sealed class RequestLoggingPipelineBahavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly ILogger<RequestLoggingPipelineBahavior<TRequest, TResponse>> _logger;
        public RequestLoggingPipelineBahavior(ILogger<RequestLoggingPipelineBahavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;
            _logger.LogInformation("Handling {RequestName}", requestName);

            TResponse result = await next();
            if (result.IsSuccess)
            {
                _logger.LogInformation("Handled {RequestName}", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    _logger.LogError("Handling {RequestName} failed with error: {Error}", requestName, result.Error);
                }
            }
            return result;
        }
    }
}

