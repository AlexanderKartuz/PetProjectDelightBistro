# MaksKorz

> Демо-страница: форма пользователя (имя, возраст, страна), приветствие по времени суток.

**Контроллер:** `MaksKorzController`  
**Layout:** default  
**Точка входа:** `/MaksKorz/Index`

---

## Назначение

Standalone demo: форма с валидацией возраста, приветствие по времени суток. Состояние хранится in-memory в полях контроллера — без DI, сервисов и БД.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/MaksKorz/Index` | GET/POST | `Index.cshtml` | — |
| `/MaksKorz/FormUser` | POST | `FormUser.cshtml` | — |

---

## Встроенное API

Нет.

---

## SignalR

Нет.

---

## Сервисы и зависимости

Нет. In-controller state: `DataUser`, `Authorization` из `Models/Maks Korz/`.

---

## Модель данных

In-memory models (не EF):
- `DataUser`, `StatusUser`, `Authorization`

---

## Frontend

- **Views:** `Index.cshtml`, `FormUser.cshtml`
- **CSS:** `wwwroot/css/korz/site.css`

---

## Локализация

Нет.

---

## Фоновые задачи

Нет.

---

## Источники в коде

- `Controllers/MaksKorzController.cs`
- `Models/Maks Korz/`
- `Views/MaksKorz/`
