# User

> Профили пользователей: редактирование, аватар, язык, список пользователей (модератор), удаление аккаунта.

**Контроллер:** `UserController`  
**Layout:** `_Layout.cshtml`  
**Точка входа:** `/User/Profile`  
**Авторизация:** `[Authorize]` на классе

---

## Назначение

Личный кабинет пользователя и административные функции: смена языка, загрузка аватара, CSV-отчёт по пользователям, удаление собственного аккаунта.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/User/Index?cardId=` | `Index` | `Views/User/Index.cshtml` | `[IsModerator]` |
| `/User/Profile` | `Profile` GET | `Views/User/Profile.cshtml` | Authenticated |
| `/User/ChangeLanguage` | POST | Redirect → Profile | Authenticated |
| `/User/UpdateAvatar` | POST | Redirect → Profile | Authenticated |
| `/User/UpdateProfile` | POST | Redirect → Profile | Authenticated |
| `/User/DeleteUser` | GET | Redirect → Index | `[IsAdmin]` (заглушка) |
| `/User/GenerateReport` | GET | CSV-файл | Authenticated |
| `/User/DeleteAccount` | POST | Redirect → `Home/Index` | Authenticated (только свой аккаунт) |

---

## Встроенное API

Нет.

---

## SignalR

Нет.

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `IAuthService` | Текущий пользователь, язык, роль, re-sign |
| `IUserDataService` | `UpdateLanguage`, `UpdateProfile` |
| `IUserRepository` | Список, аватар (`Update`), удаление |
| `IWebHostEnvironment` | Пути к `wwwroot` для загрузки аватаров |

**Внешние HTTP API:** нет

---

## Модель данных

- `UserData` — профиль, `PasswordHash`, роль, язык, аватар
- `UserProfileData` — дополнительные поля профиля

---

## Frontend

- **Layout:** `_Layout.cshtml`
- **CSS:** общий `site.css`
- **JS:** нет отдельной папки
- **Upload:** `/images/avatars/avatar-{userId}.jpg`

Views: `Profile.cshtml`, `Index.cshtml`.

---

## Локализация

- **Файлы:** навигация через `Localizations/Home.*.resx`
- **Языки:** Profile и Index — тексты захардкожены на английском

---

## Фоновые задачи

Нет.

---

## Внешние API-проекты

Нет.

---

## Связанные модули

- [Auth](auth.md) — вход перед доступом к профилю
- [Home](home.md) — redirect после `DeleteAccount`

---

## Источники в коде

- `Controllers/UserController.cs`
- `Views/User/Profile.cshtml`, `Index.cshtml`
