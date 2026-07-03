# Auth

> Cookie-based аутентификация: вход, регистрация, выход, страница отказа в доступе.

**Контроллер:** `AuthController`  
**Layout:** `_Layout.cshtml`  
**Точка входа:** `/Auth/Login`

---

## Назначение

Управление учётными записями: регистрация новых пользователей, вход по логину/паролю, выход из системы. При отказе в доступе — редирект на `/Auth/Deny`.

---

## Маршруты и страницы

| URL | Action | View / результат | Авторизация |
|-----|--------|------------------|-------------|
| `/Auth/Login` | `Login` GET | `Views/Auth/Login.cshtml` | — |
| `/Auth/Login` | `Login` POST | Redirect → `Home/Index` или View с ошибкой | — |
| `/Auth/Registration` | `Registration` GET | `Views/Auth/Registration.cshtml` | — |
| `/Auth/Registration` | `Registration` POST | Redirect → Home или View с ошибкой | — |
| `/Auth/Logout` | `Logout` | Redirect → `Home/Index` | — |
| `/Auth/Deny` | `Deny` | `Views/Auth/Deny.cshtml` | — |

---

## Встроенное API

| Метод | Путь | Описание | Auth |
|-------|------|----------|------|
| GET/POST | `/api/Auth/IsLoginFree?login=` | Проверка уникальности логина (`bool`) | — |

> Искусственная задержка 1 сек перед проверкой. Используется `registration.js`.

---

## SignalR

Нет.

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `IUserRepository` | Поиск пользователя, регистрация, `IsNameUniq` |
| `IAuthService` | `SignIn(user)` после успешного логина |

---

## Модель данных

- `UserData` — через `IUserRepository`

---

## Frontend

- **CSS:** `wwwroot/css/auth/style.css`
- **JS:** `wwwroot/js/auth/registration.js` — AJAX к `/api/Auth/IsLoginFree`
- **Images:** `wwwroot/icons/waiter.gif`, `ok.png`, `deny.jpg`

---

## Локализация

Отдельных `.resx` для Auth нет. Тексты форм — английский. `Deny.cshtml` — русский захардкожен.

---

## Фоновые задачи

Нет.

---

## Источники в коде

- `Controllers/AuthController.cs`
- `Controllers/ApiControllers/AuthController.cs`
- `Services/AuthService.cs`
- `Views/Auth/Login.cshtml`, `Registration.cshtml`, `Deny.cshtml`
