using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Runtime.CompilerServices;

namespace DelightBistro.Services.Logging
{
    public class AppLogging<T> : IAppLogging<T>
    {
        private readonly ILogger<T> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _applicationName;

        public AppLogging(ILogger<T> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _applicationName = configuration.GetSection("ApplicationName").Value ?? "Unknown";
        }

        //internal List<IDisposable> PushProperties(
        //    string memberName,
        //    string sourceFilePath,
        //    int sourceLineNumber)
        //{
        //    var list = new List<IDisposable>
        //    {
        //        LogContext.PushProperty("MemberName", memberName),
        //        LogContext.PushProperty("FilePath", sourceFilePath),
        //        LogContext.PushProperty("LineNumber", sourceLineNumber),
        //        LogContext.PushProperty("ApplicationName", _applicationName)
        //    };

        //    return list;
        //}

        private void Write(
            Action<ILogger<T>> write,
            string memberName,
            string sourceFilePath,
            int sourceLineNumber)
        {
            using var memberNameProperty = LogContext.PushProperty("MemberName", memberName);
            using var filePathProperty = LogContext.PushProperty("FilePath", sourceFilePath);
            using var lineNumberProperty = LogContext.PushProperty("LineNumber", sourceLineNumber);
            using var applicationNameProperty = LogContext.PushProperty("ApplicationName", _applicationName);
            write(_logger);
        }

        public void LogAppCritical(Exception exception, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Write(logger => _logger.LogCritical(exception, message),
                memberName, sourceFilePath, sourceLineNumber);
        }

        public void LogAppCritical(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Write(logger => _logger.LogCritical(message),
                memberName, sourceFilePath, sourceLineNumber);
        }

        public void LogAppDebug(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Write(logger => _logger.LogDebug(message),
                    memberName, sourceFilePath, sourceLineNumber);
        }

        public void LogAppError(Exception exception, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Write(logger => _logger.LogError(exception, message),
                    memberName, sourceFilePath, sourceLineNumber);
        }

        public void LogAppError(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Write(logger => _logger.LogError(message),
                     memberName, sourceFilePath, sourceLineNumber);
        }

        public void LogAppInformation(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Write(logger => _logger.LogInformation(message),
                     memberName, sourceFilePath, sourceLineNumber);
        }

        public void LogAppTrace(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Write(logger => _logger.LogTrace(message),
                     memberName, sourceFilePath, sourceLineNumber);
        }

        public void LogAppWarning(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Write(logger => _logger.LogWarning(message),
                    memberName, sourceFilePath, sourceLineNumber);
        }
    }
}
