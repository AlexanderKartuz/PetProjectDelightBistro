# Steam

> Клон Steam: каталог игр, фильтрация, CRUD, отзывы, community chat, профиль, рекомендации RAWG.

**Контроллер:** `SteamController`  
**Layout:** `_LayoutSteam.cshtml`  
**Точка входа:** `/Steam/Index`  
**Авторизация:** `[Authorize]` на классе, selective `[AllowAnonymous]`

---

## Назначение

Полноценный игровой магазин: каталог с пагинацией и фильтрами, добавление/редактирование/удаление игр с role-based правилами, отзывы, community chat в реальном времени, интеграция с RAWG для рекомендаций.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/Steam/Index` | `Index` | `Index.cshtml` | `[AllowAnonymous]` |
| `/Steam/Catalog` | `Catalog` | `Catalog.cshtml` | Anonymous (page, pageSize max 48) |
| `/Steam/AddGame` | GET/POST | `AddGame.cshtml` | `[IsModerator]` |
| `/Steam/GameDetails?id=` | `GameDetails` | `GameDetails.cshtml` | Anonymous |
| `/Steam/EditGame` | GET/POST | `EditGame.cshtml` | `[EditForCreatorWithRequiredRole]` |
| `/Steam/DeleteGame` | GET | Redirect | `[DeleteWithRoleAndTimeRestriction]` |
| `/Steam/CommunityChat` | `CommunityChat` | `CommunityChat.cshtml` | Authenticated |

POST `AddGame` → SignalR `NewGameAdded`.

---

## Встроенное API

| Метод | Путь | Описание | Auth |
|-------|------|----------|------|
| GET | `/api/Catalog/GetGames` | Paginated JSON catalog | — |
| GET | `/api/Catalog/GetGameDetails?id=` | Game details JSON | — |
| GET | `/api/Catalog/Delete?gameIds=` | Bulk delete | `[IsAdminApi]` |
| GET | `/api/Chat/SendChatMessage?message=` | Send chat message | `[Authorize]` |
| POST | `/api/GameReview/Add` | Body: `AddGameReviewRequest` | Cookie auth check |

---

## SignalR

| Hub | Маршрут | Событие |
|-----|---------|---------|
| `SteamChatHub` | `/steam/community-chat` | `SendChatMessage(userName, message, userId, timestamp)` |
| `SteamNotificationHub` | `/steam/notification` | `NewGameAdded(gameName, urlCover)` |

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `ICatalogService` / `CatalogService` | Каталог, CRUD игр |
| `IChatService` / `ChatService` | Community chat + SignalR push |
| `RawgApi` | `https://api.rawg.io/api/` — рекомендации |
| `IGameRepository`, `IGameReviewRepository`, `IGameGenreRepository`, `IPublisherRepository`, `ICommunityChatMessageRepository` | Data access |

**Фоновый сервис:** `RatingAnalyticsBackgroundService` — пересчёт рейтингов каждые 10 мин.

---

## Модель данных

| Сущность | DbSet |
|----------|-------|
| `GameData` | `Games` |
| `PublisherData` | `Publishers` |
| `GameGenreData` | `GameGenres` |
| `GameReviewData` | `GameReviews` |
| `CommunityChatMessageData` | `CommunityChatMessages` |

M:M: `GameData` ↔ `GameGenreData`.

---

## Frontend

- **CSS:** `wwwroot/css/steam/styles.css`, `recommendations.css`
- **JS:** `notification.js`, `community-chat.js`, `catalog-delete-game.js`, `game-reviews.js`, `main-carousel.js`

---

## Локализация

| Resource | Scope | Языки |
|----------|-------|-------|
| `Steam/SteamShared.resx` | Layout nav, footer | EN + RU |
| `Steam/Mainpage.resx` | Index | EN + RU |
| `Steam/CatalogPage.resx` | Catalog filters | EN + RU |
| `Steam/GameDetailsPage.resx` | Details, reviews | EN + RU |
| `Steam/AddGamePage.resx` | Add form | EN + RU |
| `Steam/EditGamePage.resx` | Edit form | EN + RU |
| `Steam/ProfilePage.resx` | User profile | EN + RU |

Recommendations views — без `.resx` (English inline).

---

## Фоновые задачи

| Сервис | Интервал | Назначение |
|--------|----------|------------|
| `RatingAnalyticsBackgroundService` | 10 мин | `AverageRating`, `ReviewsCount`, `PositiveReviewsCount` |

---

## Связанные модули

| Спутник | Документ |
|---------|----------|
| Recommendations | [recommendations.md](recommendations.md) |
| User (SteamProfile) | [platform/user.md](../platform/user.md) |

---

## Источники в коде

- `Controllers/SteamController.cs`
- `Controllers/ApiControllers/steam/CatalogController.cs`, `ChatController.cs`, `GameReviewController.cs`
- `Controllers/CustomAuthAttribute/Steam/`
- `Hubs/SteamChatHub.cs`, `SteamNotificationHub.cs`
- `Services/CatalogService.cs`, `ChatService.cs`
- `Views/Steam/`
