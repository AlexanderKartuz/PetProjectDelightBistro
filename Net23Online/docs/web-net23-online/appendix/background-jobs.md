# Фоновые задачи

Hosted services и Quartz jobs WebNet23Online.

---

## Hosted Services (`AddHostedService`)

| Сервис | Файл | Интервал | Модуль | Назначение |
|--------|------|----------|--------|------------|
| `NotificationBackgroundService` | `Services/BackgroundServices/NotificationBackgroundService.cs` | 30 сек | Notification | Опрос `INotificationRepository`, broadcast `NewMessage` |
| `DelightBistroOrderBackgroundService` | `Services/BackgroundServices/DelightBistroOrderBackgroundService.cs` | 24 ч | DelightBistro | Удаление expired orders |
| `RatingAnalyticsBackgroundService` | `Services/BackgroundServices/steam/RatingAnalyticsBackgroundService.cs` | 10 мин | Steam | Пересчёт `AverageRating`, `ReviewsCount`, `PositiveReviewsCount` |

---

## Quartz Jobs (`AddQuartz`)

| Job | Файл | Расписание | Модуль | Назначение |
|-----|------|------------|--------|------------|
| `AnimalWorldPromotionsCheckJob` | `Services/Jobs/AnimalWorldPromotionsCheckJob.cs` | Cron `0 0 9-20 ? * *` (hourly 9–20) | AnimalWorld | Истечение акций, broadcast через `AnimalWorldNotificationsHub` |

---

## Регистрация (`Program.cs`)

```csharp
builder.Services.AddHostedService<NotificationBackgroundService>();
builder.Services.AddHostedService<DelightBistroOrderBackgroundService>();
builder.Services.AddHostedService<RatingAnalyticsBackgroundService>();

// Quartz
q.AddJob<AnimalWorldPromotionsCheckJob>(...);
q.AddTrigger(... CronSchedule "0 0 9-20 ? * *" ...);
```

---

## Источники в коде

- `Services/BackgroundServices/`
- `Services/BackgroundServices/steam/`
- `Services/Jobs/`
- `Program.cs`
