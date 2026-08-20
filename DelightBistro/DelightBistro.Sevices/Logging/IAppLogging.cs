using System.Runtime.CompilerServices;

namespace DelightBistro.Services.Logging
{
    public interface IAppLogging<T>
    {
        void LogAppError(Exception exception, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void LogAppError(Exception exception, string message, object?[] args,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void LogAppError(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void LogAppError(string message, object?[] args,
          [CallerMemberName] string memberName = "",
          [CallerFilePath] string sourceFilePath = "",
          [CallerLineNumber] int sourceLineNumber = 0);

        void LogAppCritical(Exception exception, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void LogAppCritical(Exception exception, string message, object?[] args,
          [CallerMemberName] string memberName = "",
          [CallerFilePath] string sourceFilePath = "",
          [CallerLineNumber] int sourceLineNumber = 0);

        void LogAppCritical(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void LogAppCritical(string message, object?[] args,
         [CallerMemberName] string memberName = "",
         [CallerFilePath] string sourceFilePath = "",
         [CallerLineNumber] int sourceLineNumber = 0);

        void LogAppDebug(string message,
           [CallerMemberName] string memberName = "",
           [CallerFilePath] string sourceFilePath = "",
           [CallerLineNumber] int sourceLineNumber = 0);
        void LogAppDebug(string message, object?[] args,
          [CallerMemberName] string memberName = "",
          [CallerFilePath] string sourceFilePath = "",
          [CallerLineNumber] int sourceLineNumber = 0);

        void LogAppTrace(string message,
           [CallerMemberName] string memberName = "",
           [CallerFilePath] string sourceFilePath = "",
           [CallerLineNumber] int sourceLineNumber = 0);
        void LogAppTrace(string message, object?[] args,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

        void LogAppInformation(string message,
           [CallerMemberName] string memberName = "",
           [CallerFilePath] string sourceFilePath = "",
           [CallerLineNumber] int sourceLineNumber = 0);
        void LogAppInformation(string message, object?[] args,
           [CallerMemberName] string memberName = "",
           [CallerFilePath] string sourceFilePath = "",
           [CallerLineNumber] int sourceLineNumber = 0);

        void LogAppWarning(string message,
           [CallerMemberName] string memberName = "",
           [CallerFilePath] string sourceFilePath = "",
           [CallerLineNumber] int sourceLineNumber = 0);
        void LogAppWarning(string message, object?[] args,
           [CallerMemberName] string memberName = "",
           [CallerFilePath] string sourceFilePath = "",
           [CallerLineNumber] int sourceLineNumber = 0);
    }
}
