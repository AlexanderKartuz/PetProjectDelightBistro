# DelightBistroMinimalApi

> API каталога напитков с кэшированием (Memory или Redis), OutputCache, rate limiting и Serilog.

**Проект:** `DelightBistroMinimalApi/DelightBistroMinimalApi.csproj`  
**Порт (HTTPS):** 7090  
**Swagger:** `https://localhost:7090/swagger`

---

## Назначение

Отдельный Minimal API для каталога напитков. Вынесен из DelightBistroMvc, чтобы демонстрировать кэш, Redis, rate limiting и централизованное логирование (Serilog) на независимой БД `WebNet23Tea`. Потребители — JS модуля DelightBistro (`drink.js`) и React-приложение `react-delight-bistro-app`.

---

## Запуск

| Параметр | Значение |
|----------|----------|
| HTTPS | `https://localhost:7090` |
| HTTP | `http://localhost:5047` |
| Swagger | `/swagger` (корень `/` редиректит сюда) |

---

## Эндпоинты

CRUD идёт через `IDrinksCacheService`. Реализация зависит от `Caching:Provider` (`Memory` по умолчанию, `Redis` — `DrinksRedisCacheService`). Отдельных `*Redis` URL больше нет.

| Метод | Путь | Описание | Request | Response |
|-------|------|----------|---------|----------|
| GET | `/` | Redirect на Swagger | — | 302 |
| GET | `/GetDrinks` | Список напитков (кэш + OutputCache, тег `DRINKS`) | — | `Drink[]` |
| GET | `/GetDrink/{id}` | Напиток по id (vary by route, expire 1 мин) | route `id` | `Drink` / 404 |
| POST | `/CreateDrink` | Создать напиток, инвалидация тега `DRINKS` | body `Drink` | `Drink` |
| PUT | `/ChangeDrink/{id}` | Изменить напиток | route `id`, body `Drink` | `Drink` / 404 |
| DELETE | `/DeleteDrink` | Удалить напиток | body `int id` | 204 / 404 |
| GET | `/Exception` | Тест exception middleware | — | error |
| GET | `/redis-test` | Тест Redis | — | string |

`/redis-test` регистрируется **только** при `Caching:Provider = Redis`.

---

## DbContext и БД

| Параметр | Значение |
|----------|----------|
| DbContext | `MiniDbContext` |
| База данных | `WebNet23Tea` |
| Connection | LocalDB, ключ `ConnectionStrings:Drinks` |

**Сущности:**

- `Drink` — напиток (`Name`, `Price`, `Description`, `ImgUrl`)
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
- **Кэш:** `AddDelightBistroCaching` — MemoryCache всегда; Redis (`localhost:6379`, instance `DelightBistro_`) при `Caching:Provider=Redis`; OutputCache (60 сек default)
- **Rate limiting:** `AddCustomRateLimiter` — chained sliding window (IP + global), 429. Лимиты: `GlobalRateLimitingOptions`, `IpRateLimitingOptions`
- **Прочее:** `UseCustomExeptionHandling`, `UseResponseHeader`, `UseCustomRequestLogging`
- **Serilog:** `builder.ConfigureSeriLog()` в `Program.cs`

---

## Потребители

| Потребитель | Файл | URL API |
|-------------|------|---------|
| DelightBistro MVC | `wwwroot/js/delight-bistro/drink.js` | `https://localhost:7090` (`GetDrinks`, `CreateDrink`) |
| react-delight-bistro-app | `src/services/drinks-service.ts` | `https://localhost:7090` (полный CRUD) |

→ [DelightBistro module](../../delight-bistro-mvc/modules/delight-bistro/README.md)

---

## Миграции

```bash
dotnet ef migrations add {MigrationName} --project DelightBistro/DelightBistroMinimalApi --startup-project DelightBistro/DelightBistroMinimalApi
```

Применение миграции — **только вручную** человеком:

```bash
dotnet ef database update --project DelightBistro/DelightBistroMinimalApi --startup-project DelightBistro/DelightBistroMinimalApi
```

---

## Источники в коде

- `Program.cs`
- `Properties/launchSettings.json`
- `appsettings.json` / `appsettings.Development.json`
- `DbStuff/` — `MiniDbContext`, `Drink`, `SeriLogEntry`, `DrinkRepository`
- `Services/Cache/` — `IDrinksCacheService`, `DrinksMemoryCacheService`, `DrinksRedisCacheService`
- `Middlewares/`
- ProjectReference → `DelightBistro.Sevices/DelightBistro.Services.csproj`
