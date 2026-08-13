# Home

> Главная страница с приветствием пользователя и ссылкой на Delight Bistro.

**Контроллер:** `HomeController`  
**Layout:** `_Layout.cshtml`  
**Точка входа:** `/Home/Index`

---

## Назначение

Лендинг DelightBistroMvc. Показывает имя и роль текущего пользователя (или гостя) и ссылку на модуль DelightBistro. Корневой URL `/` открывает не Home, а DelightBistro (маршрут по умолчанию).

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/Home/Index` | `Index` | `Views/Home/Index.cshtml` | — |
| `/Home/Privacy` | `Privacy` | `Views/Home/Privacy.cshtml` | — |
| `/Home/Error` | `Error` | `Views/Shared/Error.cshtml` | — |

---

## Встроенное API

Нет.

---

## SignalR

Нет (клиент глобальных уведомлений подключён через `_Layout.cshtml` → `commonNotification.js`).

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `IAuthService` | Имя и роль текущего пользователя для Index |
| `IUserRepository` | Инжектирован, в actions не используется |
| `ILogger<HomeController>` | Логирование |

**Внешние HTTP API:** нет

---

## Модель данных

Не использует сущности напрямую.

---

## Frontend

- **Layout:** `_Layout.cshtml`
- **CSS:** `wwwroot/css/site.css` (в т.ч. стили уведомлений)
- **JS:** `wwwroot/js/site.js`, `wwwroot/js/commonNotification.js`

---

## Локализация

- **Файлы:** `Localizations/Home.resx`, `Home.Ru.resx`, `Home.De.resx`
- **Языки:** EN, Ru, De

Ключи: `Index_Home`, `Index_Login`, `Index_Logout`, `Index_Profile`, `Index_Registration`.

> Сама страница `Index.cshtml` локализацию не использует — текст захардкожен на английском.

---

## Фоновые задачи

Нет.

---

## Внешние API-проекты

Нет.

---

## Связанные модули

- [DelightBistro](../delight-bistro/README.md) — ссылка с Index (`/DelightBistro/Index`)

---

## Источники в коде

- `Controllers/HomeController.cs`
- `Views/Home/Index.cshtml`, `Privacy.cshtml`
- `Views/Shared/_Layout.cshtml`
