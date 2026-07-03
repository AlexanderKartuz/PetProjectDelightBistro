# HabitTracker Admin

> Админ-панель трекера: статистика, список пользователей, блокировка.

**Контроллер:** `HabitTrackerAdminController`  
**Родительский модуль:** [HabitTracker](README.md)  
**Точка входа:** `/HabitTrackerAdmin/AdminPanel`  
**Авторизация:** `[IsModerator]` на классе

---

## Назначение

Административный интерфейс для модераторов: просмотр общей статистики трекера, список пользователей с возможностью блокировки/разблокировки в HabitTracker.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/HabitTrackerAdmin/AdminPanel` | `AdminPanel` | `AdminPanel.cshtml` | Moderator+ |
| `/HabitTrackerAdmin/UserList` | `UserList` | `UserList.cshtml` | Moderator+ |
| `/HabitTrackerAdmin/ToggleBlock` | POST | Redirect | Moderator+ |

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
| `IHabitTrackerAdminRepository` | Список пользователей, блокировка |
| `IHabitStatisticsService` | Агрегированная статистика |
| `IUserRepository` | Данные пользователей |

---

## Модель данных

- `HabitTrackerProfileData` — флаг блокировки
- `UserData` — список пользователей

---

## Frontend

- **Views:** `Views/HabitTrackerAdmin/AdminPanel.cshtml`, `UserList.cshtml`

---

## Локализация

Нет отдельных `.resx`.

---

## Источники в коде

- `Controllers/HabitTrackerAdminController.cs`
- `Views/HabitTrackerAdmin/`
