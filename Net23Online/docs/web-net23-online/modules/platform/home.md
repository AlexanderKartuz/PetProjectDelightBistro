# Home

> Главная страница с приветствием пользователя и ссылками на все модули сайта.

**Контроллер:** `HomeController`  
**Layout:** `_Layout.cshtml`  
**Точка входа:** `/`

---

## Назначение

Landing page solution Net23Online. Показывает имя и роль текущего пользователя (или гостя) и список ссылок на feature-модули: AnimeGirl, Maze, Steam, AnimalWorld и др.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/`, `/Home/Index` | `Index` | `Views/Home/Index.cshtml` | — |
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

| Файл | Языки | Где используется |
|------|-------|------------------|
| `Localizations/Home.resx` | EN | Навигация в `_Layout.cshtml` |
| `Home.Ru.resx` | RU | |
| `Home.De.resx` | DE | |

Ключи: `Index_Home`, `Index_Login`, `Index_Logout`, `Index_Profile`, `Index_Registration`.

> Сама страница `Index.cshtml` локализацию не использует — текст захардкожен на английском.

---

## Фоновые задачи

Нет.

---

## Внешние API-проекты

Нет.

---

## Источники в коде

- `Controllers/HomeController.cs`
- `Views/Home/Index.cshtml`, `Privacy.cshtml`
- `Views/Shared/_Layout.cshtml`
