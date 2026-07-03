# DelightBistro

> Ресторан: меню по типам, конструктор блюд, заказы, статистика, real-time чат и каталог чая.

**Контроллер:** `DelightBistroController`  
**Layout:** `_LayoutDelightBistro.cshtml`  
**Точка входа:** `/DelightBistro/Index`

---

## Назначение

Модуль ресторана с полным циклом: создание меню, ингредиентов и блюд, оформление заказов, чат персонала, статистика и CSV-экспорт. Часть данных (чай) вынесена в отдельный Minimal API.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/DelightBistro/Index?menuType=` | `Index` | `Index.cshtml` | — |
| `/DelightBistro/CreateMenu` | GET/POST | `CreateMenu.cshtml` | `[Authorize]` + `[IsModerator]` |
| `/DelightBistro/CreateIngredient` | GET/POST | `CreateIngredient.cshtml` | Moderator |
| `/DelightBistro/FoodBuilderData?id=` | GET/POST | `FoodBuilderData.cshtml` | Moderator |
| `/DelightBistro/AllFoodItems` | `AllFoodItems` | `AllFoodItems.cshtml` | `[IsEmployee]` |
| `/DelightBistro/DeleteFoodItem?id=` | POST | Redirect | Employee |
| `/DelightBistro/GenerateTable` | GET | CSV export | — |
| `/DelightBistro/Stats` | `Stats` | `Stats.cshtml` | — |
| `/DelightBistro/Chat` | `Chat` | `Chat.cshtml` | — |

POST `FoodBuilderData` → SignalR `NewFoodWasCreated`.

---

## Встроенное API

| Метод | Путь | Описание | Auth |
|-------|------|----------|------|
| GET | `/api/DelightBistro/Delete?ids=` | Удаление блюд | — |
| POST | `/api/DelightBistro/CreateOrder` | Body: `CreateOrderDto` | `[Authorize]` |
| GET | `/api/DelightBistro/NotifyAboutFood?name=&price=` | SignalR broadcast | — |

---

## SignalR

| Параметр | Значение |
|----------|----------|
| Hub | `DeligtBistroHub` *(typo в коде)* |
| Маршрут | `/my-hub/delightbistro` |

**Client → Server:** `SendMessage`, `JoinChat`, `GetUserName`

**Server → Client:** `NewFoodWasCreated`, `ReceiveMessage`, `UserConnected`, `UserDisconnected`, `SetUserName`, `ConnectedUsers`

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `IFoodItemGenerator` | Генерация блюд |
| `IMenuTypeGenerator` | Типы меню |
| `IIngredientGenerator` | Ингредиенты |
| `IDelightBistroMainIndexGenerator` | Главная страница |
| `IFoodItemRepository`, `IMenuRepository`, `IIngredientsRepository`, `IOrderRepository` | Data access |

**Фоновый сервис:** `DelightBistroOrderBackgroundService` — удаление просроченных заказов (интервал 24ч).

---

## Модель данных

| Сущность | Описание |
|----------|----------|
| `FoodItemData` | Блюда |
| `MenuData` | Меню |
| `IngredientData` | Ингредиенты |
| `FoodItemIngredientData` | M:M join с quantity |
| `OrderData` | Заказы, M:M с FoodItem, FK UserId |

---

## Frontend

- **Layouts:** `_LayoutDelightBistro.cshtml`, `_LayoutimagesDelightBistro.cshtml`
- **CSS:** `style.css`, `chat.css`, `delight-bistro-hub.css`
- **JS:** `delight-bistro-signalr.js`, `delight-bistro-hub.js`, `delight-bistro-chat-hub.js`, `buy-button.js`, `all-foods.js`, `tea.js`, `preview-food-item.js`
- **Images:** `wwwroot/images/delight-bistro/`

---

## Локализация

| Файл | Языки |
|------|-------|
| `Localizations/DelightBistro.resx` | EN |
| `DelightBistro.Ru.resx` | RU |
| `DelightBistro.De.resx` | DE |

Ключи: `Index_Create_menu`, `Index_Create_ingredient`, `Index_Create_dish`, `Index_All_menu`, `Index_All_dish`, `Index_Cuisine`.

---

## Фоновые задачи

| Сервис | Интервал | Назначение |
|--------|----------|------------|
| `DelightBistroOrderBackgroundService` | 24ч | Удаление expired orders |

---

## Внешние API-проекты

| API | Порт | JS-файл |
|-----|------|---------|
| [DelightBistroMinimalApi](../../../minimal-apis/delight-bistro/README.md) | 7090 | `tea.js` |

---

## Источники в коде

- `Controllers/DelightBistroController.cs`
- `Controllers/ApiControllers/DelightBistroController.cs`
- `Hubs/DeligtBistroHub.cs`
- `Services/DelightBistro/`
- `Services/BackgroundServices/DelightBistroOrderBackgroundService.cs`
- `Views/DelightBistro/`
