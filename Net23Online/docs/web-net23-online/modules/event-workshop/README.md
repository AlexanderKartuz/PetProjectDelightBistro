# EventWorkshop

> Статический каталог событий (Creation / Sport / Games) — без БД и сервисов.

**Контроллер:** `EventWorkshopController`  
**Layout:** `_LayoutEventWorkshop.cshtml`  
**Точка входа:** `/EventWorkshop/Index`

---

## Назначение

Демо-страница каталога событий с тремя категориями. Все данные захардкожены в контроллере — нет БД, сервисов и API.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/EventWorkshop/Index?typeEvent=` | `Index` | `Index.cshtml` | — |

**Query `typeEvent`:** `Creation`, `Sport`, `Games`, или default (все категории).

---

## Встроенное API

Нет.

---

## SignalR

Нет.

---

## Сервисы и зависимости

Нет. Данные из private methods контроллера: `GetCreationViewModels`, `GetSportViewModels`, `GetGameViewModels`.

---

## Модель данных

Нет.

---

## Frontend

- **CSS:** `wwwroot/css/event-workshop/event-workshop.css`
- **Images:** `wwwroot/images/event-workshop/{creation,sport,games}/`

---

## Локализация

Нет.

---

## Фоновые задачи

Нет.

---

## Источники в коде

- `Controllers/EventWorkshopController.cs`
- `Views/EventWorkshop/Index.cshtml`
- `Views/Shared/_LayoutEventWorkshop.cshtml`
