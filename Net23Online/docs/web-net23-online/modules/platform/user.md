# User

> Профили пользователей: редактирование, аватар, язык, список пользователей (модератор), Steam-профиль, удаление аккаунта.

**Контроллер:** `UserController`  
**Layout:** `_Layout.cshtml` / `_LayoutSteam.cshtml` (SteamProfile)  
**Точка входа:** `/User/Profile`  
**Авторизация:** `[Authorize]` на классе

---

## Назначение

Личный кабинет пользователя и административные функции: смена языка, загрузка аватара, CSV-отчёт по пользователям, отдельный Steam-профиль с возможностью удаления аккаунта.

---

## Маршруты и страницы

| URL | Action | View / результат | Авторизация |
|-----|--------|------------------|-------------|
| `/User/Index?cardId=` | `Index` | `Views/User/Index.cshtml` | `[IsModerator]` |
| `/User/Profile` | `Profile` GET | `Views/User/Profile.cshtml` | Authenticated |
| `/User/ChangeLanguage` | POST | Redirect → Profile | Authenticated |
| `/User/UpdateAvatar` | POST | Redirect → Profile | Authenticated |
| `/User/UpdateProfile` | POST | Redirect → Profile | Authenticated |
| `/User/DeleteUser?id=` | GET | Redirect → Index | `[IsAdmin]` (заглушка) |
| `/User/GenerateReport` | GET | CSV-файл | Authenticated |
| `/User/SteamProfile` | GET | `Views/User/SteamProfile.cshtml` | Authenticated |
| `/User/UpdateSteamProfile` | POST | Redirect → SteamProfile | Authenticated |
| `/User/ChangeLanguageInSteam` | POST | Redirect → SteamProfile | Authenticated |
| `/User/UpdateSteamAvatar` | POST | Redirect → SteamProfile | Authenticated |
| `/User/DeleteAccount` | POST | Redirect → `Steam/Index` | Authenticated (только свой аккаунт) |

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
| `IUserRepository` | CRUD пользователей, язык, профиль, удаление |
| `IWebHostEnvironment` | Пути к `wwwroot` для загрузки аватаров |

---

## Модель данных

- `UserData` — профиль, роль, язык, Steam-поля

---

## Frontend

| View | Upload-путь |
|------|-------------|
| `Profile.cshtml` | `/images/avatars/avatar-{userId}.jpg` |
| `SteamProfile.cshtml` | `/images/steam/avatars/avatar-{userId}.jpg` |

---

## Локализация

| Область | Файлы |
|---------|-------|
| SteamProfile | `Localizations/Steam/ProfilePage.resx`, `ProfilePage.Ru.resx` |
| Навигация | `Localizations/Home.*.resx` через layout |

Profile и Index — тексты захардкожены на английском.

---

## Фоновые задачи

Нет.

---

## Связанные модули

- [Steam](../steam/README.md) — SteamProfile, DeleteAccount → redirect на Steam/Index

---

## Источники в коде

- `Controllers/UserController.cs`
- `Views/User/Profile.cshtml`, `Index.cshtml`, `SteamProfile.cshtml`
