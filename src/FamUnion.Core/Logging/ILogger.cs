using System;

namespace FamUnion.Core.Logging
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message, Exception exception);
        void LogError(string message, int errorCode);
    }
}
