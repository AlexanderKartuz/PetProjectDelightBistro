# DelightBistro.Services

> Общая библиотека логирования на Serilog для DelightBistro Minimal API (обёртка `IAppLogging` и конфигурация sinks).

**Проект:** `DelightBistro.Sevices/DelightBistro.Services.csproj`  
*(папка на диске — `DelightBistro.Sevices`, имя проекта — `DelightBistro.Services`)*

---

## Назначение

Выносит настройку Serilog и удобную обёртку над `ILogger<T>` из host-приложения. Host передаёт connection string и уровни через свой `appsettings`; библиотека не владеет БД.

---

## Публичный API

| Тип | Имя | Назначение |
|-----|-----|------------|
| Extension | `LoggingConfiguration.ConfigureSeriLog` | `UseSerilog` + sinks File / Console / MSSqlServer |
| Interface | `IAppLogging<T>` | Обёртка логов с Caller* и свойствами LogContext |
| Class | `AppLogging<T>` | Реализация `IAppLogging<T>` |

---

## Зависимости

| Пакет / проект | Назначение |
|----------------|------------|
| `Serilog.AspNetCore` | Интеграция с ASP.NET Core host |
| `Serilog.Enrichers.Environment` | Enrich `MachineName` |
| `Serilog.Sinks.MSSqlServer` | Запись в SQL Server |

---

## Конфигурация (ожидает host)

| Ключ | Назначение |
|------|------------|
| `ConnectionStrings:Logging` | Строка для SQL sink |
| `Logging:Console:restrictedToMinimumLevel` | Мин. уровень Console |
| `Logging:File:restrictedToMinimumLevel` | Мин. уровень File |
| `Logging:MSSqlServer:schema` / `tableName` / `restrictedToMinimumLevel` | SQL sink |
| `ApplicationName` | Свойство для `IAppLogging` / шаблона файла |

Подключение в host:

```csharp
builder.Services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));
builder.ConfigureSeriLog();
```

---

## Кто использует

| Проект | Как использует |
|--------|----------------|
| DelightBistroMinimalApi | `ConfigureSeriLog`, DI `IAppLogging<>`; таблица `Logging.SeriLogs` в БД API |

---

## Источники в коде

- `DelightBistro.Sevices/Logging/LoggingConfiguration.cs`
- `DelightBistro.Sevices/Logging/IAppLogging.cs`
- `DelightBistro.Sevices/Logging/AppLogging.cs`
