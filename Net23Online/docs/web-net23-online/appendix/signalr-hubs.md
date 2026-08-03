# SignalR-хабы WebNet23Online

Все SignalR-хабы, зарегистрированные в `Program.cs`.

---

## Карта хабов

| Hub | Маршрут | Модуль | Server → Client |
|-----|---------|--------|-----------------|
| `DeligtBistroHub` | `/my-hub/delightbistro` | DelightBistro | `NewFoodWasCreated`, chat events |
| `NotificationHub` | `/my-hub/notification` | Notification (global) | `NewMessage` |

---

## Регистрация (`Program.cs`)

```csharp
app.MapHub<DeligtBistroHub>("/my-hub/delightbistro");
app.MapHub<NotificationHub>("/my-hub/notification");
```

---

## Глобальный клиент уведомлений

- **JS:** `wwwroot/js/commonNotification.js`
- **Hub:** `/my-hub/notification`
- **Подключение:** `_Layout.cshtml` (все страницы с default layout)
- **URL:** `https://localhost:7284/my-hub/notification` (hardcoded)

---

## Источники в коде

- `Hubs/` — все hub-классы и интерфейсы
- `Program.cs` — `MapHub` registrations
- `wwwroot/js/*/ *-signalr.js`, `notification.js` — клиенты
