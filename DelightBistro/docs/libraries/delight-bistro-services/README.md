# DelightBistro.Services

> Общая библиотека логирования на Serilog для DelightBistro Minimal API (обёртка `IAppLogging` и конфигурация sinks).

**Проект:** `DelightBistro.Sevices/DelightBistro.Services.csproj`  
*(папка на диске — `DelightBistro.Sevices`, имя проекта — `DelightBistro.Services`)*

---

## Назначение

Выносит настройку Serilog и удобную обёртку над `ILogger<T>` из host-приложения. Host передаёт connection string и уровни через свой `appsettings`; библиотека не владеет БД.

`IAppLogging<T>` обогащает каждое сообщение свойствами LogContext: `MemberName`, `FilePath`, `LineNumber`, `ApplicationName` (через `[Caller*]`).

---

## Публичный API

| Тип | Имя | Назначение |
|-----|-----|------------|
| Extension | `LoggingConfiguration.ConfigureSeriLog` | `UseSerilog` + sinks File / Console / MSSqlServer |
| Interface | `IAppLogging<T>` | Обёртка логов с Caller* и свойствами LogContext |
| Class | `AppLogging<T>` | Реализация `IAppLogging<T>` |

Перегрузки:

- `LogApp*(string message, …)` — простой текст
- `LogApp*(string message, object?[] args, …)` — structured logging (`"Drink {DrinkId}"`, `new object?[] { id }`)
- для Error/Critical также варианты с `Exception`

`AppLogging` пишет через общий `Write` с `using LogContext.PushProperty(...)`, чтобы свойства снимались даже при исключении в `Log*`.

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
| `Logging:File:path` | Путь file sink (default: `logs/delight-bistro-.txt`, rolling по дню) |
| `Logging:MSSqlServer:schema` / `tableName` / `restrictedToMinimumLevel` | SQL sink |
| `ApplicationName` | Свойство для `IAppLogging` / шаблона файла (fallback: `Unknown`) |

Уровни парсятся без учёта регистра (`Warning` / `warning`).

Подключение в host:

```csharp
builder.Services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));
builder.ConfigureSeriLog();
```

Пример в `appsettings.json`:

```json
"Logging": {
  "Console": { "restrictedToMinimumLevel": "Information" },
  "File": {
    "restrictedToMinimumLevel": "Warning",
    "path": "logs/delight-bistro-.txt"
  },
  "MSSqlServer": {
    "schema": "Logging",
    "tableName": "SeriLogs",
    "restrictedToMinimumLevel": "Warning"
  }
},
"ApplicationName": "DelightBistro.Api"
```

Папка `logs/` игнорируется корневым `.gitignore` (`[Ll]ogs/`). Старый паттерн `ErrorLog*.txt` тоже в ignore.

Колонки SQL sink дополнительно: `ApplicationName`, `MachineName`, `MemberName`, `FilePath`, `LineNumber`, `SourceContext`, `RequestPath`, `ActionName`. `RequestPath` / `ActionName` заполняются только если host пушит их в LogContext (сама обёртка этого не делает).

---

## Кто использует

| Проект | Как использует |
|--------|----------------|
| DelightBistroMinimalApi | `ConfigureSeriLog`, DI `IAppLogging<>`; middleware пишут через `IAppLogging`; таблица `Logging.SeriLogs` в БД API |

Подробности в host: [minimal-apis/delight-bistro](../../minimal-apis/delight-bistro/README.md).

---

## Источники в коде

- `DelightBistro.Sevices/Logging/LoggingConfiguration.cs`
- `DelightBistro.Sevices/Logging/IAppLogging.cs`
- `DelightBistro.Sevices/Logging/AppLogging.cs`
