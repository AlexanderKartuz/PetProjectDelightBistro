# SignalR-хабы WebNet23Online

Все SignalR-хабы, зарегистрированные в `Program.cs`.

---

## Карта хабов

| Hub | Маршрут | Модуль | Server → Client |
|-----|---------|--------|-----------------|
| `AnimeHub` | `/my-hub/anime` | AnimeGirl | `NewAnimeCreated`, `ReceiveMessage`, chat events |
| `DeligtBistroHub` | `/my-hub/delightbistro` | DelightBistro | `NewFoodWasCreated`, chat events |
| `RockLegendsHub` | `/my-hub/rock-legends` | RockLegendsPortal | `NewGenreCreated` |
| `AnimalWorldHub` | `/my-hub/animal-world` | AnimalWorld | `NewAnimalInZooAppeared` |
| `AnimalWorldNotificationsHub` | `/my-hub/animal-world-promotions` | AnimalWorld | `ZoosPromotions` |
| `JdmHub` | `/my-hub/jdm` | JDM | `NewJdmCarsCreated` |
| `SteamChatHub` | `/steam/community-chat` | Steam | `SendChatMessage` |
| `SteamNotificationHub` | `/steam/notification` | Steam | `NewGameAdded` |
| `LittleLemonHub` | `/my-hub/little-lemon` | LittleLemon | `NewReservationCreated`, `ReceivePrivateMessage` |
| `NotificationHub` | `/my-hub/notification` | Notification (global) | `NewMessage` |

---

## Не зарегистрированные

| Hub | Ожидаемый маршрут | Модуль | Проблема |
|-----|-------------------|--------|----------|
| `RockBandHub` | `/my-hub/rock-band` | RockBands | `MapHub` отсутствует в `Program.cs` |

---

## Регистрация (`Program.cs`)

```csharp
app.MapHub<AnimeHub>("/my-hub/anime");
app.MapHub<DeligtBistroHub>("/my-hub/delightbistro");
app.MapHub<RockLegendsHub>("/my-hub/rock-legends");
app.MapHub<AnimalWorldHub>("/my-hub/animal-world");
app.MapHub<AnimalWorldNotificationsHub>("/my-hub/animal-world-promotions");
app.MapHub<JdmHub>("/my-hub/jdm");
app.MapHub<SteamChatHub>("/steam/community-chat");
app.MapHub<SteamNotificationHub>("/steam/notification");
app.MapHub<LittleLemonHub>("/my-hub/little-lemon");
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
