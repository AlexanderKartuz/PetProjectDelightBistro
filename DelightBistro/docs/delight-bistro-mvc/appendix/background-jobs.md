# Фоновые задачи

Hosted services DelightBistroMvc.

---

## Hosted Services (`AddHostedService`)

| Сервис | Файл | Интервал | Модуль | Назначение |
|--------|------|----------|--------|------------|
| `NotificationBackgroundService` | `Services/BackgroundServices/NotificationBackgroundService.cs` | 30 сек | Notification | Scope на итерацию; опрос активных уведомлений (`UtcNow`), broadcast `NewMessage` |
| `DelightBistroOrderBackgroundService` | `Services/BackgroundServices/DelightBistroOrderBackgroundService.cs` | 24 ч | DelightBistro | Scope на итерацию; `IOrderRepository.DeleteExpiredOrders` (`ExecuteDelete`) |

---

## Quartz

В `Program.cs` зарегистрирован `AddQuartzHostedService`, но **jobs не добавлены**.

---

## Регистрация (`Program.cs`)

```csharp
builder.Services.AddHostedService<NotificationBackgroundService>();
builder.Services.AddHostedService<DelightBistroOrderBackgroundService>();

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});
```

---

## Источники в коде

- `Services/BackgroundServices/`
- `Program.cs`
