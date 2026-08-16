# JWT-аутентификация (DelightBistroMinimalApi)

> Bearer JWT для Minimal API: login по пользователям MVC-БД, защита мутаций (Delete — только Admin). Cookie-auth сайта не используется.

**Проект:** `DelightBistroMinimalApi`  
**Связанный MVC Auth:** [cookie-аутентификация](../../delight-bistro-mvc/modules/platform/auth.md)  
**Обзор API:** [README.md](README.md)

---

## Назначение

API выдаёт JWT после проверки логина/пароля через `IUserDataService` / BCrypt из [DelightBistroMvc.Data](../../libraries/delight-bistro-mvc-data/README.md). Регистрации в API нет — пользователь создаётся в MVC (`/Auth/Registration`). Каталог напитков живёт в отдельной БД `WebNet23Tea`; пользователи — в `WebNet23Online`.

```text
MVC Registration → UserData (WebNet23Online)
POST /login      → IEndpointValidator → ValidateCredetials + JwtTokenService → accessToken
Authorization: Bearer … → JwtBearer → HttpContext.User
DELETE /DeleteDrink/{id} → RequireRole(Admin)
```

---

## Конфигурация

Секции в `appsettings.json` / `appsettings.Development.json`:

| Ключ | Назначение |
|------|------------|
| `ConnectionStrings:Users` | LocalDB `WebNet23Online` — чтение пользователей при login |
| `Jwt:Issuer` | Кто выдал токен (`DelightBistro.Api`) — должен совпасть при проверке |
| `Jwt:Audience` | Для кого токен (`DelightBistro.Client`) |
| `Jwt:Key` | Секрет HS256 (≥ 32 символа); один и тот же при выдаче и проверке |
| `Jwt:ExpireMinutes` | Срок жизни access token (по умолчанию 60) |

Модель настроек: `Services/Auth/Options/JwtOptions` (`SectionName = "Jwt"`), DI через `IOptions<JwtOptions>`.

---

## Регистрация DI и pipeline

Расширение `AddDelightBistroJwtAuth` (`Services/Auth/AuthServiceCollectionExtensions.cs`):

1. `Configure<JwtOptions>` + валидация длины `Key`
2. `AddDbContext<WebContext>` на `ConnectionStrings:Users`
3. `IUserRepository` / `UserRepository`, `IPasswordHasher` / `BCryptPasswordHasher`, `IUserDataService` / `UserDataService`
4. `IJwtTokenService` / `JwtTokenService`
5. `AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(...)` — `TokenValidationParameters` (Issuer, Audience, Lifetime, SigningKey, `ClockSkew` 1 мин)
6. `AddAuthorization()` — обязательно для `UseAuthorization` / `RequireAuthorization`

Swagger: `AddDelightBistroSwaggerWithJwt` — схема Bearer и кнопка Authorize.

В `Program.cs`:

```text
AddDelightBistroJwtAuth → AddDelightBistroSwaggerWithJwt
…
UseCors → UseAuthentication → UseAuthorization → …
```

Пакет: `Microsoft.AspNetCore.Authentication.JwtBearer`.  
ProjectReference: `DelightBistroMvc.Data`.

---

## Выдача токена (`JwtTokenService`)

Метод `CreateToken(UserData user)`:

| Claim | Источник | Зачем |
|-------|----------|--------|
| `ClaimTypes.NameIdentifier` | `user.Id` | Id пользователя |
| `ClaimTypes.Name` | `user.Name` | Логин |
| `ClaimTypes.Role` | `user.Role.ToString()` | `RequireRole` (например `Admin`) |

Подпись: `SymmetricSecurityKey` + `HmacSha256`, issuer/audience/expires из `JwtOptions`.

> Роль обязательно в `ClaimTypes.Role`. Иначе `RequireRole("Admin")` не сработает.

---

## Эндпоинты и доступ

| Метод | Путь | Auth |
|-------|------|------|
| POST | `/login` | Анонимно. Body: `LoginRequest` (`login`, `password`). Пустой/невалидный body: **400**. Успех: `LoginResponse` (`accessToken`, `expiresInMinutes`). Неверные учётные данные: **401** |
| GET | `/GetDrinks`, `/GetDrink/{id}` | Анонимно |
| POST | `/CreateDrink` | Сейчас без `RequireAuthorization` (body `DrinkRequest`, **400** при ошибках валидации) |
| PUT | `/ChangeDrink/{id}` | Сейчас без `RequireAuthorization` (route `id` + body `DrinkRequest`) |
| DELETE | `/DeleteDrink/{id}` | Route `id`. `.RequireAuthorization` + `RequireRole(Admin)` → без токена **401**, не Admin **403** |

Auth DTO: `ModelsDto/LoginRequest.cs`, `ModelsDto/LoginResponse.cs`. Напитки: `ModelsDto/EntityDto/DrinkRequest.cs`, `DrinkResponse.cs`.

---

## Проверка через Swagger

1. Создать пользователя в MVC (`/Auth/Registration`). Для Delete — в БД `WebNet23Online` выставить `Role = 99` (`UserRole.Admin`).
2. `https://localhost:7090/swagger` → `POST /login` с `{ "login": "...", "password": "..." }`.
3. Скопировать `accessToken` → **Authorize** (Bearer).
4. Вызвать `DELETE /DeleteDrink/{id}` (id в path, без body).

| Ответ | Смысл |
|-------|--------|
| Login **400** | Не прошла валидация `LoginRequest` |
| Login **401** | Нет пользователя / неверный пароль / неверная строка `Users` |
| Мутация **401** | Нет или невалидный JWT |
| Мутация **403** | Токен валиден, роли недостаточно |
| **200** / **204** | Успех |

---

## Две БД у API

| Connection | DbContext | Назначение |
|------------|-----------|------------|
| `Drinks` | `MiniDbContext` | Каталог напитков (`WebNet23Tea`) |
| `Users` | `WebContext` | Login / роли (`WebNet23Online`) |

JWT после выдачи в SQL не хранится: на каждом запросе проверяется подпись и claims.

---

## Типичные ошибки настройки

- В JSON appsettings нет запятой перед секцией `Jwt` → API не стартует.
- `UseAuthorization` без `AddAuthorization` → исключение при старте.
- Короткий `Jwt:Key` → ошибка при регистрации auth.
- Роль не в `ClaimTypes.Role` → всегда 403 на Admin-only.
- `ConnectionStrings:Users` указывает не на ту БД, что MVC → login всегда 401.

---

## Источники в коде

- `Program.cs` — pipeline, `/login`, `DELETE /DeleteDrink/{id}` + `RequireAuthorization`
- `Services/Auth/Options/JwtOptions.cs`
- `Services/Auth/Interfaces/IJwtTokenService.cs`
- `Services/Auth/JwtTokenService.cs`
- `Services/Auth/AuthServiceCollectionExtensions.cs`
- `ModelsDto/LoginRequest.cs`, `ModelsDto/LoginResponse.cs`
- `appsettings.json` / `appsettings.Development.json` — `Jwt`, `Users`
- `DelightBistroMvc.Data` — `WebContext`, `UserDataService`, `BCryptPasswordHasher`, `UserRole`
