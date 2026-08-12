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
            _applicationName = configuration.GetSection("ApplicationName").Value;
        }

        internal List<IDisposable> PushProperties(
            string memberName,
            string sourceFilePath,
            int sourceLineNumber)
        {
            var list = new List<IDisposable>
            {
                LogContext.PushProperty("MemberName", memberName),
                LogContext.PushProperty("FilePath", sourceFilePath),
                LogContext.PushProperty("LineNumber", sourceLineNumber),
                LogContext.PushProperty("ApplicationName", _applicationName)
            };

            return list;
        }

        public void LogAppCritical(Exception exception, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogCritical(exception, message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppCritical(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogCritical(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppDebug(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogDebug(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppError(Exception exception, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogError(exception, message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppError(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogError(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppInformation(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogInformation(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppTrace(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogTrace(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }

        public void LogAppWarning(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            var list = PushProperties(memberName, sourceFilePath, sourceLineNumber);
            _logger.LogWarning(message);

            foreach (var item in list)
            {
                item.Dispose();
            }
        }
    }
}
