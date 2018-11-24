using FamUnion.Core.Validation;
using log4net;
using System;

namespace FamUnion.Core.Logging
{
    public class Logger : ILogger
    {
        private readonly ILog _logger;

        public Logger(ILog logger)
        {
            _logger = Validator.ThrowIfNull(logger, nameof(logger));
        }

        public void LogError(string message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void LogError(string message, int errorCode)
        {
            _logger.Error($"Error Code: {errorCode} -- {message}");
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }
    }
}
