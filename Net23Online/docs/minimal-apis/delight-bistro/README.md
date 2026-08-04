# DelightBistroMinimalApi

> API каталога чая/напитков с демо кэширования (Memory + Redis), OutputCache и rate limiting.

**Проект:** `DelightBistroMinimalApi/DelightBistroMinimalApi.csproj`  
**Порт (HTTPS):** 7090  
**Swagger:** `https://localhost:7090/swagger`

---

## Назначение

Отдельный Minimal API для каталога чая. Вынесен из WebNet23Online, чтобы демонстрировать кэш, Redis и rate limiting на независимой БД `WebNet23Tea`. Потребитель — JS модуля DelightBistro (`tea.js`).

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
| PUT | `/ChangeDrink/{id}` | Изменить чай | route `id`, body `Tea` | — |
| DELETE | `/DeleteDrink` | Удалить чай | query | — |
| GET | `/Exception` | Тест exception middleware | — | error |
| GET | `/redis-test` | Тест Redis | — | — |
| GET | `/GetTeasRedis` | Список через Redis (`TeaCacheService`) | — | `Tea[]` |
| GET | `/GetTeaRedis/{id}` | Чай через Redis | route `id` | `Tea` |
| POST | `/CreateTeaRedis` | Создать + инвалидация Redis/OutputCache | body `Tea` | — |
| PUT | `/ChangeDrinkRedis/{id}` | Изменить через Redis | route + body | — |
| DELETE | `/DeleteDrinkRedis` | Удалить через Redis | query | — |

---

## DbContext и БД

| Параметр | Значение |
|----------|----------|
| DbContext | `MiniDbContext` |
| База данных | `WebNet23Tea` |
| Connection | LocalDB |

**Сущности:**

- `Tea` — напиток (имя, цена и др.)

---

## Middleware и инфраструктура

- **CORS:** default policy — any header/method, credentials, any origin
- **Кэш:** MemoryCache, OutputCache (60 сек default), StackExchange Redis (`localhost:6379`, instance `DelightBistro_`)
- **Rate limiting:** `AddCustomRateLimiter` / `UseRateLimiter`
- **Прочее:** `UseCustomExeptionHandling`, `UseResponseHeader`, `UseCustomRequestLogging`

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

Применение миграции — **только вручную** человеком.

---

## Источники в коде

- `Program.cs`
- `Properties/launchSettings.json`
- `DbStuff/` — `MiniDbContext`, `TeaRepository`
- `Services/TeaCacheService.cs`
- `Middlewares/`
