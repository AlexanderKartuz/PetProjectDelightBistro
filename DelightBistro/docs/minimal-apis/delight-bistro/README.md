# DelightBistroMinimalApi

> API каталога напитков с кэшированием (Memory или Redis), OutputCache, rate limiting, Serilog, JWT Bearer и DTO/валидацией на границе HTTP.

**Проект:** `DelightBistroMinimalApi/DelightBistroMinimalApi.csproj`  
**Порт (HTTPS):** 7090  
**Swagger:** `https://localhost:7090/swagger`

**JWT:** [jwt-auth.md](jwt-auth.md) — настройка, login, роли, проверка в Swagger.

---

## Назначение

Отдельный Minimal API для каталога напитков. Вынесен из DelightBistroMvc, чтобы кэш, Redis, rate limiting и Serilog жили на независимой БД `WebNet23Tea`. Потребители — JS модуля DelightBistro (`drink.js`) и `react-delight-bistro-app`.

CRUD всегда идёт через `IDrinksCacheService` (внутри — entity `Drink`). На HTTP-границе — DTO (`DrinkRequest` / `DrinkResponse`), ручной маппинг `IDrinkMapper` и DataAnnotations через `IEndpointValidator` (невалидный body → **400** ValidationProblem). Провайдер кэша — `Caching:Provider` (`Memory` | `Redis`) в `AddDelightBistroCaching`.

Аутентификация API — JWT Bearer (`AddDelightBistroJwtAuth`). Пользователи и пароли — из `WebNet23Online` через ProjectReference на DelightBistroMvc.Data; регистрация только в MVC.

---

## Запуск

| Параметр | Значение |
|----------|----------|
| HTTPS | `https://localhost:7090` |
| HTTP | `http://localhost:5047` |
| Swagger | `/swagger` |

Корень `/` редиректит на `/swagger`.

---

## Эндпоинты

| Метод | Путь | Описание | Auth | Request | Response |
|-------|------|----------|------|---------|----------|
| GET | `/` | Redirect на Swagger | — | — | 302 |
| POST | `/login` | Выдача JWT | анонимно | `LoginRequest` | `LoginResponse` / **400** / 401 |
| GET | `/GetDrinks` | Список (кэш + OutputCache, тег `drinks`) | анонимно | — | `DrinkResponse[]` |
| GET | `/GetDrink/{id}` | По id (vary by route, OutputCache 1 мин, тег `drink`) | анонимно | route `id` | `DrinkResponse` / 404 |
| POST | `/CreateDrink` | Создать, сброс тега `drinks` | пока открыт | body `DrinkRequest` | `DrinkResponse` / **400** |
| PUT | `/ChangeDrink/{id}` | Изменить | пока открыт | route `id`, body `DrinkRequest` | `DrinkResponse` / **400** / 404 |
| DELETE | `/DeleteDrink/{id}` | Удалить | **JWT + роль Admin** | route `id` | 204 / 404 / 401 / 403 |
| GET | `/Exception` | Тест exception middleware | — | — | error |
| GET | `/redis-test` | Проверка Redis | — | — | string |

`/redis-test` регистрируется **только** при `Caching:Provider = Redis`.

**Контракт id:** у `GetDrink` / `ChangeDrink` / `DeleteDrink` идентификатор только в **route**. Create — без id (выдаёт БД). Delete — без body.

**Валидация:** `login`, `CreateDrink`, `ChangeDrink` вызывают `IEndpointValidator` (DataAnnotations на DTO). Ошибки → **400** `ValidationProblem` (`errors` по полям). Неверный логин/пароль после успешной валидации → **401**.

Подробности JWT, claims и проверки в Swagger: [jwt-auth.md](jwt-auth.md).

### DTO напитков

| Класс | Файл | Назначение |
|-------|------|------------|
| `DrinkRequest` | `ModelsDto/EntityDto/DrinkRequest.cs` | Body Create/Change: `Name` (Required, MaxLength 50), `Price` (Range 0.1–500), `Description?`, `ImgUrl?` — **без `Id`** |
| `DrinkResponse` | `ModelsDto/EntityDto/DrinkResponse.cs` | Ответ GET/POST/PUT: `Id`, `Name`, `Price`, `Description?`, `ImgUrl?` |

Маппинг: `IDrinkMapper` / `DrinkMapper` (`Mappings/`). Entity `Drink` наружу из эндпоинтов не отдаётся.

---

## DbContext и БД

| Параметр | Напитки | Пользователи (JWT login) |
|----------|---------|--------------------------|
| DbContext | `MiniDbContext` | `WebContext` (DelightBistroMvc.Data) |
| База данных | `WebNet23Tea` | `WebNet23Online` |
| Connection | `ConnectionStrings:Drinks` | `ConnectionStrings:Users` |

**Сущности (напитки):**

- `Drink` — таблица `Drinks` (`Name`, `Price`, `Description`, `ImgUrl`); раньше была `Tea` / `Teas`
- `SeriLogEntry` — чтение `Logging.SeriLogs` (запись через Serilog sink, не через EF)

**Пользователи:** `UserData` и роли — в MVC-БД; см. [jwt-auth.md](jwt-auth.md).

---

## Middleware и инфраструктура

- **CORS:** default policy — any header/method, credentials, any origin
- **JWT:** `AddDelightBistroJwtAuth` + `UseAuthentication` / `UseAuthorization`; Swagger Bearer — [jwt-auth.md](jwt-auth.md)
- **Кэш / Redis:** см. ниже
- **Rate limiting:** `AddCustomRateLimiter` — chained sliding window (IP + global), 429. Лимиты: `GlobalRateLimitingOptions`, `IpRateLimitingOptions`
- **Прочее:** `UseCustomExeptionHandling`, `UseResponseHeader` (`Cache-Control: public, max-age=10` на успешные GET), `UseCustomRequestLogging`
- **Serilog:** `builder.ConfigureSeriLog()` + DI `IAppLogging<>` ([DelightBistro.Services](../../libraries/delight-bistro-services/README.md)); SQL — `ConnectionStrings:Logging`, таблица `Logging.SeriLogs`

### Кэш приложения (`IDrinksCacheService`)

Регистрация: `AddDelightBistroCaching` (`Caching:Provider`, `Caching:InstanceName`).

| `Caching:Provider` | Реализация | Хранилище |
|--------------------|------------|-----------|
| `Memory` (по умолчанию) | `DrinksMemoryCacheService` | `IMemoryCache` |
| `Redis` | `DrinksRedisCacheService` | StackExchange Redis (`ConnectionStrings:Redis`, prefix `DelightBistro_`) |

Ключи (`CacheKeys`): список `drink:all`, карточка `drink:{id}`. TTL: absolute 5 мин, sliding 2 мин (у Redis GetDrink sliding 3 мин). Пустой GetDrink в кэш не кладётся. Create/Change/Delete снимают соответствующие ключи.

### OutputCache

Default expiration 60 сек. GET-список — тег `CacheTags.DRINKS` (`drinks`); GET по id — тег `drink`, `SetVaryByRouteValue("id")`, expire 1 мин. Create сбрасывает тег `drinks`. Change/Delete сейчас вызывают `EvictByTagAsync` со строкой-ключом, а не с тегом `CacheTags.DRINK` — кэш `GetDrink/{id}` после мутации может остаться до TTL.

---

## Потребители

| Потребитель | Файл | URL API |
|-------------|------|---------|
| DelightBistro MVC | `wwwroot/js/delight-bistro/drink.js` | `https://localhost:7090` (`GetDrinks`, `CreateDrink`) |
| react-delight-bistro-app | `src/services/drinks-service.ts` | `https://localhost:7090` (полный CRUD; Delete — `DELETE /DeleteDrink/{id}`) |

→ [DelightBistro module](../../delight-bistro-mvc/modules/delight-bistro/README.md)

---

## Миграции

```bash
dotnet ef migrations add {MigrationName} --project DelightBistro/DelightBistroMinimalApi --startup-project DelightBistro/DelightBistroMinimalApi
```

Применение миграции — **только вручную** человеком.

---

## Источники в коде

- `Program.cs` — эндпоинты, DI `IDrinkMapper` / `IEndpointValidator`
- `Properties/launchSettings.json`
- `appsettings.json` / `appsettings.Development.json` — `Drinks`, `Users`, `Jwt`, `Caching`
- `DbStuff/` — `MiniDbContext`, `Drink`, `SeriLogEntry`, `DrinkRepository`
- `Services/Cache/` — `CachingServiceCollectionExtensions`, `IDrinksCacheService`, Memory/Redis реализации
- `Services/Auth/` — JWT options, `JwtTokenService`, `AddDelightBistroJwtAuth` / Swagger JWT
- `ModelsDto/` — `LoginRequest`, `LoginResponse`, `ApiErrorResponse`
- `ModelsDto/EntityDto/` — `DrinkRequest`, `DrinkResponse`
- `Mappings/` — `IDrinkMapper`, `DrinkMapper`
- `Validation/` — `IEndpointValidator`, `EndpointValidator`
- `Constans/CacheKeys.cs`, `Constans/CacheTags.cs`
- `Middlewares/`
- ProjectReference → `DelightBistro.Sevices/DelightBistro.Services.csproj`
- ProjectReference → `DelightBistroMvc.Data/DelightBistroMvc.Data.csproj`
