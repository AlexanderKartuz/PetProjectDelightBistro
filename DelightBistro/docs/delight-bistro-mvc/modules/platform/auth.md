# Auth

> Cookie-based аутентификация: вход, регистрация, выход, страница отказа в доступе.

**Контроллер:** `AuthController`  
**Layout:** `_Layout.cshtml`  
**Точка входа:** `/Auth/Login`

---

## Назначение

Управление учётными записями: регистрация, вход по логину и паролю, выход. Пароль в БД хранится как `UserData.PasswordHash` (BCrypt, work factor 11): при регистрации `UserRepository.Registration` хэширует значение, при входе `GetByNameAndPassword` проверяет через `IPasswordHasher.VerifyPassword`. Cookie-схема `AuthDelightBistro` — в [Platform](README.md). После Login / Registration / Logout — редирект на `DelightBistro/Index`; отказ в доступе — `/Auth/Deny`.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/Auth/Login` | `Login` GET | `Views/Auth/Login.cshtml` | — |
| `/Auth/Login` | `Login` POST (`LoginAsync`) | Redirect → `DelightBistro/Index` или View с ошибкой | — |
| `/Auth/Registration` | `Registration` GET | `Views/Auth/Registration.cshtml` | — |
| `/Auth/Registration` | `Registration` POST | Redirect → DelightBistro или View с ошибкой | — |
| `/Auth/Logout` | `LogoutAsync` | Redirect → `DelightBistro/Index` | — |
| `/Auth/Deny` | `Deny` | `Views/Auth/Deny.cshtml` | — |

---

## Встроенное API

| Метод | Путь | Описание |
|-------|------|----------|
| GET/POST | `/api/Auth/IsLoginFree?login=` | Проверка уникальности логина (`bool`) |

> Искусственная задержка 1 сек перед проверкой. Используется `registration.js`.

---

## SignalR

Нет.

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `IUserRepository` | `GetByNameAndPassword`, `Registration`, `IsNameUniq` |
| `IAuthService` | Claims cookie, `SignIn(user)` |
| `IPasswordHasher` / `BCryptPasswordHasher` | Хэш и проверка пароля (DI в `Program.cs`) |

**Claims в cookie:** `Id`, `Role`, `UserName`, `Language`, `ClaimTypes.AuthenticationMethod`.

**Внешние HTTP API:** нет

---

## Модель данных

- `UserData` — логин (`Name`), `PasswordHash`, роль, язык, профиль; поле пароля в открытом виде нет (миграция `renameUserEntityFieldPasswordHash`)

---

## Frontend

- **Layout:** `_Layout.cshtml`
- **CSS:** `wwwroot/css/auth/style.css`
- **JS:** `wwwroot/js/auth/registration.js` — AJAX к `/api/Auth/IsLoginFree`

---

## Локализация

- **Файлы:** отдельных `.resx` для Auth нет
- **Языки:** тексты форм — английский; `Deny.cshtml` — русский захардкожен

---

## Фоновые задачи

Нет.

---

## Внешние API-проекты

Нет.

---

## Связанные модули

- [User](user.md) — профиль после входа
- [DelightBistro](../delight-bistro/README.md) — редирект после Login / Registration / Logout

---

## Источники в коде

- `Controllers/AuthController.cs`
- `Controllers/ApiControllers/AuthController.cs`
- `Services/AuthService.cs`
- `DelightBistroMvc.Data/Repositories/UserRepository.cs`
- `DelightBistroMvc.Data/Services/PasswordHasher/`
- `Views/Auth/Login.cshtml`, `Registration.cshtml`, `Deny.cshtml`
