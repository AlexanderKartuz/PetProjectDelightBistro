# DelightBistro

> Ресторан: меню по типам, конструктор блюд, заказы, статистика, real-time чат и каталог напитков.

**Контроллер:** `DelightBistroController`  
**Layout:** `_LayoutDelightBistro.cshtml`  
**Точка входа:** `/` и `/DelightBistro/Index`

---

## Назначение

Модуль ресторана с полным циклом: создание меню, ингредиентов и блюд, оформление заказов, чат персонала, статистика и CSV-экспорт. Каталог напитков вынесен в отдельный Minimal API. На Index подгружаются факты о котах и фото собак через внешние HTTP API.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/`, `/DelightBistro/Index?menuType=` | `Index` | `Index.cshtml` | — |
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
| `IFoodItemGenerator` | Генерация / CRUD блюд |
| `IMenuTypeGenerator` | Типы меню |
| `IIngredientGenerator` | Ингредиенты |
| `IDelightBistroMainIndexGenerator` | Главная страница (+ CatFact/Dog) |
| `IFoodItemRepository`, `IMenuRepository`, `IIngredientsRepository`, `IOrderRepository` | Data access |

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

---

## Frontend

- **Layout:** `_LayoutDelightBistro.cshtml`
- **CSS:** `wwwroot/css/delight-bistro/` — `style.css`, `chat.css`, `delight-bistro-hub.css`
- **JS:** `wwwroot/js/delight-bistro/` — `delight-bistro-signalr.js`, `delight-bistro-hub.js`, `delight-bistro-chat-hub.js`, `buy-button.js`, `all-foods.js`, `drink.js`, `preview-food-item.js`

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
| `DelightBistroOrderBackgroundService` | 24ч | Удаление expired orders |

---

## Внешние API-проекты

| API | Порт | JS-файл |
|-----|------|---------|
| [DelightBistroMinimalApi](../../../minimal-apis/delight-bistro/README.md) | 7090 | `drink.js` — кэш Memory/Redis на стороне API |

Также каталог пьёт [react-delight-bistro-app](../../../../../react-delight-bistro-app/) (`src/services/drinks-service.ts`).

Презентация модуля: [presentation.html](presentation.html).

---

## Связанные модули

- [Platform / Auth](../platform/auth.md) — заказ требует авторизации
- [Notification](../notification/README.md) — глобальные уведомления на layout сайта

---

## Источники в коде

- `Controllers/DelightBistroController.cs`
- `Controllers/ApiControllers/DelightBistroController.cs`
- `Hubs/DeligtBistroHub.cs`
- `Services/DelightBistro/`
- `Services/BackgroundServices/DelightBistroOrderBackgroundService.cs`
- `Views/DelightBistro/`
