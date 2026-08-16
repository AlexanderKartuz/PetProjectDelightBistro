# DelightBistro

> Ресторан: меню по типам, конструктор блюд, заказы, статистика, два SignalR-чата (legacy и NewChat с БД) и каталог напитков.

**Контроллер:** `DelightBistroController`  
**Layout:** `_LayoutDelightBistro.cshtml`  
**Точка входа:** `/` и `/DelightBistro/Index`

---

## Назначение

Модуль ресторана с полным циклом: создание меню, ингредиентов и блюд, оформление заказов, чат персонала, статистика и CSV-экспорт. Каталог напитков вынесен в отдельный Minimal API. На Index асинхронно подгружаются факты о котах и фото собак (`GetMainIndexViewModelAsync`); при недоступности API меню всё равно отдаётся. Сид меню/ингредиентов/блюд — `IDelightBistroSeedService.EnsureSeed()` при старте приложения (`Program.cs`), не на каждый Index.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/`, `/DelightBistro/Index?menuType=` | `Index` (async) | `Index.cshtml` | — |
| `/DelightBistro/CreateMenu` | GET/POST | `CreateMenu.cshtml` | `[Authorize]` + `[IsModerator]` |
| `/DelightBistro/CreateIngredient` | GET/POST | `CreateIngredient.cshtml` | Moderator |
| `/DelightBistro/FoodBuilderData?id=` | GET/POST | `FoodBuilderData.cshtml` | Moderator |
| `/DelightBistro/AllFoodItems` | `AllFoodItems` | `AllFoodItems.cshtml` | `[IsEmployee]` |
| `/DelightBistro/DeleteFoodItem?id=` | POST | Redirect | Employee |
| `/DelightBistro/GenerateTable` | GET | CSV export | — |
| `/DelightBistro/Stats` | `Stats` | `Stats.cshtml` | — |
| `/DelightBistro/Chat` | `Chat` | `Chat.cshtml` | — (анонимы ок) |
| `/DelightBistro/NewChat` | `NewChat` | `NewChat.cshtml` | — (анонимы ок) |

POST `FoodBuilderData` → SignalR `NewFoodWasCreated` (хаб `DeligtBistroHub`).

---

## Встроенное API

| Метод | Путь | Описание | Auth |
|-------|------|----------|------|
| GET | `/api/DelightBistro/Delete?ids=` | Удаление блюд | — |
| POST | `/api/DelightBistro/CreateOrder` | Body: `CreateOrderDto` | `[Authorize]` |
| GET | `/api/DelightBistro/NotifyAboutFood?name=&price=` | SignalR broadcast | — |

---

## SignalR

Два независимых хаба. Подробная карта: [appendix/signalr-hubs.md](../../appendix/signalr-hubs.md).

### Legacy: `DeligtBistroHub`

| Параметр | Значение |
|----------|----------|
| Hub | `DeligtBistroHub` *(typo в имени класса)* |
| Маршрут | `/my-hub/delightbistro` |
| Presence | `static ConcurrentDictionary` в хабе |
| История | нет (только live) |

**Client → Server:** `SendMessage` (имя берётся на сервере), `JoinChat`  
**Server → Client:** `NewFoodWasCreated`, `ReceiveMessage`, `UserConnected`, `UserDisconnected`, `SetUserName`, `ConnectedUsers`

Клиент: `delight-bistro-signalr.js` (layout) + `delight-bistro-hub.js` (Index) + `delight-bistro-chat-hub.js` (`Chat`).

### Новый чат: `NewChatHub`

| Параметр | Значение |
|----------|----------|
| Hub | `NewChatHub` : `Hub<INewChatHub>` |
| Маршрут | `/my-hub/new-chat` |
| Группа | `"new-chat"` |
| Presence | `ChatPresenceService` (Singleton, `ConcurrentDictionary`) |
| Сообщения | БД через `INewChatService` / `IChatMessageRepository` |
| Auth на хабе | нет — анонимы с именем `Anonimus-{suffix}` или cookie `UserName` |

**Client → Server:** `JoinChat()`, `SendMessage(string text)`  
**Server → Client:** `SetUserName`, `ReceiveHistory`, `ReceiveMessage`, `ConnectedUsers`, `UserConnected`, `UserDisconnected`

Клиент: `wwwroot/js/delight-bistro/new-chat-hub.js` (`NewChat.cshtml`). Своё соединение, не шарит `delightBistroSignalR`.

---

## Сервисы и зависимости

| Сервис | Lifetime | Назначение |
|--------|----------|------------|
| `IFoodItemGenerator` | Scoped | CRUD блюд, CSV/stats |
| `IMenuTypeGenerator` | Scoped | Типы меню |
| `IIngredientGenerator` | Scoped | Ингредиенты для форм/карточек |
| `IDelightBistroMainIndexGenerator` | Scoped | `GetMainIndexViewModelAsync` (+ CatFact/Dog) |
| `IDelightBistroSeedService` | Scoped | Сид при старте |
| `INewChatService` / `NewChatService` | Scoped | Имя отправителя, сохранение/история сообщений чата |
| `ChatPresenceService` | **Singleton** | Онлайн в NewChat (`connectionId` → displayName) |
| `IFoodItemRepository`, `IMenuRepository`, `IIngredientsRepository`, `IOrderRepository`, `IChatMessageRepository` | Scoped | Data access |

**Внешние HTTP API:** `CatFactApi`, `DogApi` (Index)

---

## Модель данных

| Сущность | Описание |
|----------|----------|
| `FoodItemData` | Блюда |
| `MenuData` | Меню |
| `IngredientData` | Ингредиенты |
| `FoodItemIngredientData` | M:M join с quantity |
| `OrderData` | Заказы, M:M с FoodItem, FK UserId |
| `ChatMessageData` | Сообщения NewChat: `SenderName`, `Text`, `CreatedAtUtc`, nullable `UserId` → `UserData.ChatMessages` |

DbSet в `WebContext`: `Messages` → `ChatMessageData`. Индекс по `CreatedAtUtc` (неуникальный).

---

## Frontend

- **Layout:** `_LayoutDelightBistro.cshtml` (CDN SignalR 6.0.1 + `delight-bistro-signalr.js`)
- **CSS:** `wwwroot/css/delight-bistro/` — `style.css`, `chat.css`, `delight-bistro-hub.css`
- **JS:** `wwwroot/js/delight-bistro/` — `delight-bistro-signalr.js`, `delight-bistro-hub.js`, `delight-bistro-chat-hub.js`, `new-chat-hub.js`, `buy-button.js`, `all-foods.js`, `drink.js`, `preview-food-item.js`

> `Index.cshtml` всё ещё подключает `/js/delight-bistro/tea.js`. Файл переименован в `drink.js` — скрипт в view нужно поправить.

---

## Локализация

- **Файлы:** `Localizations/DelightBistro.resx`, `DelightBistro.Ru.resx`, `DelightBistro.De.resx`
- **Языки:** EN, Ru, De

Ключи: `Index_Create_menu`, `Index_Create_ingredient`, `Index_Create_dish`, `Index_All_menu`, `Index_All_dish`, `Index_Cuisine`.

---

## Фоновые задачи

| Сервис | Интервал | Назначение |
|--------|----------|------------|
| `DelightBistroOrderBackgroundService` | 24ч | `ExecuteDelete` просроченных заказов |

---

## Внешние API-проекты

| API | Порт | JS-файл |
|-----|------|---------|
| [DelightBistroMinimalApi](../../../minimal-apis/delight-bistro/README.md) | 7090 | `drink.js` — кэш Memory/Redis на стороне API |

Также каталог пьёт [react-delight-bistro-app](../../../../../react-delight-bistro-app/) (`src/services/drinks-service.ts`).

Презентация модуля: [presentation.html](presentation.html).

---

## Связанные модули

- [Platform / Auth](../platform/auth.md) — заказ требует авторизации; чат доступен и анонимам
- [Notification](../notification/README.md) — глобальные уведомления на layout сайта

---

## Источники в коде

- `Controllers/DelightBistroController.cs`
- `Controllers/ApiControllers/DelightBistroController.cs`
- `Hubs/DeligtBistroHub.cs`, `Hubs/NewChatHub.cs`
- `Hubs/Interfaces/IDeligtBistroHub.cs`, `Hubs/Interfaces/INewChatHub.cs`
- `Services/DelightBistro/`
- `Services/Chat/` — `NewChatService`, `ChatPresenceService`
- `Models/DTOs/Chat/`
- `Services/BackgroundServices/DelightBistroOrderBackgroundService.cs`
- `Views/DelightBistro/` — в т.ч. `Chat.cshtml`, `NewChat.cshtml`
- `wwwroot/js/delight-bistro/new-chat-hub.js`
