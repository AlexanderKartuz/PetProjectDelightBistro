# Platform

> Сквозная инфраструктура сайта: главная страница, аутентификация, профили пользователей.

**Контроллеры:** `HomeController`, `AuthController`, `UserController`  
**Layout:** `_Layout.cshtml` (по умолчанию)

---

## Назначение

Platform объединяет базовые функции, доступные на всём сайте: точку входа с навигацией по модулям, cookie-based аутентификацию и управление профилем пользователя (общий и Steam-вариант).

---

## Подмодули

| Подмодуль | Документ | Точка входа |
|-----------|----------|-------------|
| Home | [home.md](home.md) | `/` |
| Auth | [auth.md](auth.md) | `/Auth/Login` |
| User | [user.md](user.md) | `/User/Profile` |

---

## Общие настройки auth

Cookie-схема `AuthService.AUTH_KEY` (`Program.cs`):

| Параметр | Значение |
|----------|----------|
| LoginPath | `/Auth/Login` |
| AccessDeniedPath | `/Auth/Deny` |
| ExpireTimeSpan | 13 минут |

---

## Источники в коде

- `Controllers/HomeController.cs`
- `Controllers/AuthController.cs`
- `Controllers/UserController.cs`
- `Controllers/ApiControllers/AuthController.cs`
- `Services/AuthService.cs`
- `Views/Shared/_Layout.cshtml`
