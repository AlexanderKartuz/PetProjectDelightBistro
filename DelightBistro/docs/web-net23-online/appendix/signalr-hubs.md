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
- **Подключение:** `_Layout.cshtml` (страницы с default layout)
- **URL:** `https://localhost:7284/my-hub/notification` (hardcoded)

Клиент DelightBistro: `wwwroot/js/delight-bistro/delight-bistro-signalr.js`.

---

## Источники в коде

- `Hubs/` — hub-классы и интерфейсы
- `Program.cs` — `MapHub` registrations
- `wwwroot/js/commonNotification.js`
- `wwwroot/js/delight-bistro/delight-bistro-signalr.js`
