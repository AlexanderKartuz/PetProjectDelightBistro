# HabitTracker

> Трекер привычек: создание, недельная сетка, статистика, дневник, PDF-отчёт.

**Контроллер:** `HabitTrackerController`  
**Layout:** `_LayoutHabitTracker.cshtml`  
**Точка входа:** `/HabitTracker/Index`  
**Авторизация:** `[Authorize]` + `[IsNotBlockedInTracker]` на классе

---

## Назначение

Персональный трекер привычек: пользователь создаёт привычки, отмечает выполнение по дням недели, просматривает статистику и дневник, экспортирует CSV-отчёт. Заблокированные пользователи не имеют доступа.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/HabitTracker/Index` | `Index` | `Index.cshtml` | Authenticated + not blocked |
| `/HabitTracker/HabitTracker` | `HabitTracker` | `HabitTracker.cshtml` | same |
| `/HabitTracker/Statistics` | `Statistics` | `Statistics.cshtml` | same |
| `/HabitTracker/Diary?month=&year=` | `Diary` | `Diary.cshtml` | same |
| `/HabitTracker/Settings` | `Settings` | `Settings.cshtml` | same |
| `/HabitTracker/CreateHabit` | GET/POST | `CreateHabit.cshtml` | same |
| `/HabitTracker/DeleteHabit` | GET/POST | `DeleteHabit.cshtml` | same |
| `/HabitTracker/EditHabit` | GET/POST | `EditHabit.cshtml` | same |
| `/HabitTracker/TogglePoint` | POST | — | same |
| `/HabitTracker/GenerateReport` | POST | CSV export | same |

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
| `IHabitService` / `HabitService` | CRUD привычек, toggle points |
| `IHabitStatisticsService` / `HabitStatisticsService` | Статистика, дневник |
| `IHabitRepository`, `IHabitDoneDatesRepository`, `IHabitDiaryRepository`, `IHabitTrackerAdminRepository` | Data access |

---

## Модель данных

| Сущность | Таблица |
|----------|---------|
| `HabitData` | `Habits` |
| `HabitDoneDatesData` | `HabitDoneDates` |
| `HabitTrackerDiaryData` | `DiaryEntries` |
| `HabitTrackerProfileData` | `HabitTrackerProfile` (блокировка) |

---

## Frontend

- **CSS:** `wwwroot/css/habit-tracker/` — `main.css`, `diary.css`, `statistics.css`, `settings.css`, `welcome.css`
- **JS:** `main.js`, `diary.js`, `statistics.js`, `editHabit.js`

---

## Локализация

| Файл | Языки |
|------|-------|
| `Localizations/HabitTracker/HabitTracker.resx` | EN |
| `HabitTracker.ru.resx` | RU |

---

## Фоновые задачи

Нет.

---

## Связанные модули

| Спутник | Документ |
|---------|----------|
| Admin | [admin.md](admin.md) |

---

## Источники в коде

- `Controllers/HabitTrackerController.cs`
- `Services/HabitService.cs`, `HabitStatisticsService.cs`
- `Views/HabitTracker/`
