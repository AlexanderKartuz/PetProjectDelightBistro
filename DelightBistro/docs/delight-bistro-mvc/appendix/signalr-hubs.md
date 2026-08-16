# SignalR-хабы DelightBistroMvc

Все SignalR-хабы, зарегистрированные в `Program.cs`.

---

## Карта хабов

| Hub | Маршрут | Модуль | Client → Server | Server → Client |
|-----|---------|--------|-----------------|-----------------|
| `DeligtBistroHub` | `/my-hub/delightbistro` | DelightBistro (legacy chat + еда) | `JoinChat`, `SendMessage` | `NewFoodWasCreated`, `ReceiveMessage`, `UserConnected`, `UserDisconnected`, `SetUserName`, `ConnectedUsers` |
| `NewChatHub` | `/my-hub/new-chat` | DelightBistro (NewChat) | `JoinChat`, `SendMessage` | `SetUserName`, `ReceiveHistory`, `ReceiveMessage`, `ConnectedUsers`, `UserConnected`, `UserDisconnected` |
| `NotificationHub` | `/my-hub/notification` | Notification (global) | — | `NewMessage` |

---

## Регистрация (`Program.cs`)

```csharp
builder.Services.AddSignalR();
builder.Services.AddSingleton<ChatPresenceService>();
builder.Services.AddScoped<INewChatService, NewChatService>();

// …

app.MapHub<DeligtBistroHub>("/my-hub/delightbistro");
app.MapHub<NotificationHub>("/my-hub/notification");
app.MapHub<NewChatHub>("/my-hub/new-chat");
```

---

## DeligtBistroHub

- **Страницы:** Index (тост о блюде), `Chat`
- **JS:** `delight-bistro-signalr.js` (общее соединение в layout), `delight-bistro-hub.js`, `delight-bistro-chat-hub.js`
- **Presence:** `static ConcurrentDictionary` внутри хаба
- **История сообщений:** нет
- **Пуш из MVC:** `IHubContext<DeligtBistroHub, IDeligtBistroHub>` → `NewFoodWasCreated` после `FoodBuilderData`

---

## NewChatHub

- **Страница:** `/DelightBistro/NewChat`
- **JS:** `wwwroot/js/delight-bistro/new-chat-hub.js` (отдельное соединение на `/my-hub/new-chat`)
- **Группа:** `"new-chat"`
- **Presence:** `ChatPresenceService` (Singleton)
- **Сообщения:** `ChatMessageData` через `INewChatService` + `IChatMessageRepository`; при `JoinChat` — `ReceiveHistory`
- **Анонимы:** разрешены; имя из cookie `UserName` или `Anonimus-{4 символа ConnectionId}`
- **Имя отправителя:** только с сервера (клиент передаёт лишь `text`)

Поток `JoinChat`: presence → `Groups.AddToGroupAsync` → Caller: `SetUserName` + `ReceiveHistory` + `ConnectedUsers` → OthersInGroup: `UserConnected`.

---

## NotificationHub

- **JS:** `wwwroot/js/commonNotification.js`
- **Подключение:** `_Layout.cshtml`
- **URL в JS:** `https://localhost:7284/my-hub/notification` (hardcoded)
- Hub-класс пустой; рассылка из `NotificationController` и `NotificationBackgroundService` через `IHubContext`

---

## Глобальный клиент уведомлений

- **JS:** `wwwroot/js/commonNotification.js`
- **Hub:** `/my-hub/notification`
- **Layout:** `_Layout.cshtml` (не `_LayoutDelightBistro`)

Клиент layout бистро: `wwwroot/js/delight-bistro/delight-bistro-signalr.js` → только `/my-hub/delightbistro`.

---

## Источники в коде

- `Hubs/` — hub-классы и `Hubs/Interfaces/`
- `Services/Chat/` — NewChat
- `Program.cs` — `AddSignalR`, `MapHub`, DI presence/chat
- `wwwroot/js/commonNotification.js`
- `wwwroot/js/delight-bistro/delight-bistro-signalr.js`
- `wwwroot/js/delight-bistro/new-chat-hub.js`
