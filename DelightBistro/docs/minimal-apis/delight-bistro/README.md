# DelightBistroMinimalApi

> API каталога чая/напитков с демо кэширования (Memory + Redis), OutputCache, rate limiting и Serilog.

**Проект:** `DelightBistroMinimalApi/DelightBistroMinimalApi.csproj`  
**Порт (HTTPS):** 7090  
**Swagger:** `https://localhost:7090/swagger`

---

## Назначение

Отдельный Minimal API для каталога чая. Вынесен из WebNet23Online, чтобы демонстрировать кэш, Redis, rate limiting и централизованное логирование (Serilog) на независимой БД `WebNet23Tea`. Потребитель — JS модуля DelightBistro (`tea.js`).

---

## Запуск

| Параметр | Значение |
|----------|----------|
| HTTPS | `https://localhost:7090` |
| HTTP | `http://localhost:5047` |
| Swagger | `/swagger` |

---

## Эндпоинты

| Метод | Путь | Описание | Request | Response |
|-------|------|----------|---------|----------|
| GET | `/` | Health / hello | — | string |
| GET | `/GetTeas` | Список чаёв (MemoryCache + OutputCache) | — | `Tea[]` |
| GET | `/GetTea/{id}` | Чай по id | route `id` | `Tea` / 404 |
| POST | `/CreateTea` | Создать чай | body `Tea` | `Tea` |
| PUT | `/ChangeDrink/{id}` | Изменить чай | route `id`, body `Tea` | `Tea` / 404 |
| DELETE | `/DeleteDrink` | Удалить чай | body `int id` | `bool` |
| GET | `/Exception` | Тест exception middleware | — | error |
| GET | `/redis-test` | Тест Redis | — | string |
| GET | `/GetTeasRedis` | Список через Redis (`TeaCacheService`) | — | `Tea[]` |
| GET | `/GetTeaRedis/{id}` | Чай через Redis | route `id` | `Tea` |
| POST | `/CreateTeaRedis` | Создать + инвалидация Redis/OutputCache | body `Tea` | `Tea` |
| PUT | `/ChangeDrinkRedis/{id}` | Изменить через Redis | route + body | `Tea` / 404 |
| DELETE | `/DeleteDrinkRedis` | Удалить через Redis | body `int id` | `bool` |

---

## DbContext и БД

| Параметр | Значение |
|----------|----------|
| DbContext | `MiniDbContext` |
| База данных | `WebNet23Tea` |
| Connection | LocalDB, ключ `ConnectionStrings:Drinks` |

**Сущности:**

- `Tea` — напиток (имя, цена и др.)
- `SeriLogEntry` — чтение таблицы `Logging.SeriLogs` (запись идёт через Serilog sink, не через EF)

---

## Логирование (Serilog)

Подключается из библиотеки [DelightBistro.Services](../../libraries/delight-bistro-services/README.md): `ConfigureSeriLog()`, регистрация `IAppLogging<>`.

| Параметр | Значение |
|----------|----------|
| Connection | `ConnectionStrings:Logging` (пока та же БД `WebNet23Tea`) |
| Таблица | `Logging.SeriLogs` |
| Файл | `ErrorLog.txt` (в `.gitignore` как `ErrorLog*.txt`) |

Уровни sink’ов задаются в `appsettings` (`Logging:Console` / `File` / `MSSqlServer`):

| Sink | Ключ | Типичный уровень |
|------|------|------------------|
| Console | `Logging:Console:restrictedToMinimumLevel` | `Information` |
| File | `Logging:File:restrictedToMinimumLevel` | `Warning` |
| MSSqlServer | `Logging:MSSqlServer:restrictedToMinimumLevel` | `Warning` |

Обычный `ILogger<>` в middleware уже пишет в Serilog. `IAppLogging<>` — опциональная обёртка с `MemberName` / `FilePath` / `ApplicationName`.

---

## Middleware и инфраструктура

- **CORS:** default policy — any header/method, credentials, any origin
- **Кэш:** MemoryCache, OutputCache (60 сек default), StackExchange Redis (`localhost:6379`, instance `DelightBistro_`)
- **Rate limiting:** `AddCustomRateLimiter` / `UseRateLimiter`
- **Прочее:** `UseCustomExeptionHandling`, `UseResponseHeader`, `UseCustomRequestLogging`
- **Serilog:** `builder.ConfigureSeriLog()` в `Program.cs`

---

## Потребители

| Потребитель | Файл | URL API |
|-------------|------|---------|
| DelightBistro | `wwwroot/js/delight-bistro/tea.js` | `https://localhost:7090` |

→ [DelightBistro module](../../web-net23-online/modules/delight-bistro/README.md)

---

## Миграции

```bash
dotnet ef migrations add {MigrationName} --project Net23Online/DelightBistroMinimalApi --startup-project Net23Online/DelightBistroMinimalApi
```

Применение миграции — **только вручную** человеком:

```bash
dotnet ef database update --project Net23Online/DelightBistroMinimalApi --startup-project Net23Online/DelightBistroMinimalApi
```

---

## Источники в коде

- `Program.cs`
- `Properties/launchSettings.json`
- `appsettings.json` / `appsettings.Development.json`
- `DbStuff/` — `MiniDbContext`, `Tea`, `SeriLogEntry`, `TeaRepository`
- `Services/Cache/TeaCacheService.cs`
- `Middlewares/`
- ProjectReference → `DelightBistro.Sevices/DelightBistro.Services.csproj`
