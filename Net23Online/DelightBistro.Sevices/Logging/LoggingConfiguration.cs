using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace DelightBistro.Services.Logging
{
    public static class LoggingConfiguration
    {
        private static readonly string OutPutTemplate =
            @"[{Timestamp: yy-MMM-dd HH:mm:ss} {Level} {ApplicationName}:
            {SourceContext} {NewLine} Message: {Message} {NewLine} in method
            {MemberName} at {FilePath} : {LineNumber} {NewLine} {Exception} {NewLine}";

        private static readonly ColumnOptions ColumnOptions = new ColumnOptions
        {
            AdditionalColumns = new List<SqlColumn>
            {
                new SqlColumn{DataType = SqlDbType.VarChar, ColumnName = "ApplicationName"},
                new SqlColumn{DataType = SqlDbType.VarChar, ColumnName = "MachineName"},
                new SqlColumn{DataType = SqlDbType.VarChar, ColumnName = "MemberName"},
                new SqlColumn{DataType = SqlDbType.VarChar, ColumnName = "FilePath"},
                new SqlColumn{DataType = SqlDbType.Int, ColumnName = "LineNumber"},
                new SqlColumn{DataType = SqlDbType.VarChar, ColumnName = "SourceContext"},
                new SqlColumn{DataType = SqlDbType.VarChar, ColumnName = "RequestPath"},
                new SqlColumn{DataType = SqlDbType.VarChar, ColumnName = "ActionName"},
            }
        };

        public static WebApplicationBuilder ConfigureSeriLog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, loggerConfiguration) =>
            {
                var config = builder.Configuration;
                var connectionString = config.GetConnectionString("Logging");
                var tableName = config["Logging:MSSqlServer:tableName"] ?? "SeriLogs";
                var schema = config["Logging:MSSqlServer:schema"] ?? "Logging";

                // Уровень для каждого sink читаем отдельно из appsettings
                var consoleLevel = ParseLogLevel(
                    config["Logging:Console:restrictedToMinimumLevel"], LogEventLevel.Information);
                var fileLevel = ParseLogLevel(
                    config["Logging:File:restrictedToMinimumLevel"], LogEventLevel.Warning);
                var sqlLevel = ParseLogLevel(
                    config["Logging:MSSqlServer:restrictedToMinimumLevel"], LogEventLevel.Warning);

                var sqlOptions = new MSSqlServerSinkOptions
                {
                    AutoCreateSqlTable = false,
                    SchemaName = schema,
                    TableName = tableName,
                };

                if (context.HostingEnvironment.IsDevelopment())
                {
                    sqlOptions.BatchPeriod = TimeSpan.FromSeconds(1);
                    sqlOptions.BatchPostingLimit = 1;
                }

                loggerConfiguration
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.File(
                        path: "ErrorLog.txt",
                        rollingInterval: RollingInterval.Day,
                        restrictedToMinimumLevel: fileLevel,
                        outputTemplate: OutPutTemplate)
                    .WriteTo.Console(restrictedToMinimumLevel: consoleLevel)
                    .WriteTo.MSSqlServer(connectionString: connectionString,
                        sqlOptions,
                        restrictedToMinimumLevel: sqlLevel,
                        columnOptions: ColumnOptions);
            });

            return builder;
        }

        private static LogEventLevel ParseLogLevel(string? value, LogEventLevel fallback)
        {
            if (Enum.TryParse<LogEventLevel>(value, out var level))
            {
                return level;
            }

            return fallback;
        }
    }
}
