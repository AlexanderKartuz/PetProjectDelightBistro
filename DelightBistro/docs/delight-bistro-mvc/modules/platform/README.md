# Platform

> Сквозная инфраструктура сайта: главная страница, аутентификация, профили пользователей.

**Контроллеры:** `HomeController`, `AuthController`, `UserController`  
**Layout:** `_Layout.cshtml` (по умолчанию)

---

## Назначение

Platform объединяет базовые функции, доступные на всём сайте: лендинг с навигацией к модулю DelightBistro, cookie-based аутентификацию и управление профилем пользователя.

---

## Подмодули

| Подмодуль | Документ | Точка входа |
|-----------|----------|-------------|
| Home | [home.md](home.md) | `/Home/Index` |
| Auth | [auth.md](auth.md) | `/Auth/Login` |
| User | [user.md](user.md) | `/User/Profile` |

---

## Общие настройки auth

Cookie-схема `AuthService.AUTH_KEY` = `AuthDelightBistro` (`Program.cs`):

| Параметр | Значение |
|----------|----------|
| LoginPath | `/Auth/Login` |
| AccessDeniedPath | `/Auth/Deny` |
| ExpireTimeSpan | 2 часа |
| Cookie.SecurePolicy | Always |
| Cookie.HttpOnly | true |
| SlidingExpiration | true |

Claims: `Id`, `Role`, `UserName`, `Language`. Пароли не кладутся в cookie — только `PasswordHash` в БД (BCrypt). Подробности входа — [auth.md](auth.md).

---

## Связанные модули

- [Notification](../notification/README.md) — глобальные уведомления через `_Layout`
- [DelightBistro](../delight-bistro/README.md) — основной feature-модуль, ссылка с Home; редирект после Login/Logout/Registration

---

## Источники в коде

- `Controllers/HomeController.cs`
- `Controllers/AuthController.cs`
- `Controllers/UserController.cs`
- `Controllers/ApiControllers/AuthController.cs`
- `Services/AuthService.cs`
- `Program.cs` — cookie-схема, `IPasswordHasher`
- `Views/Shared/_Layout.cshtml`
