# RockLegendsPortal

> Портал рок-легенд: карточки групп, случайный хит iTunes, жанры, цитаты.

**Контроллер:** `RockLegendsPortalController`  
**Layout:** `_LayoutRockLegends.cshtml`  
**Точка входа:** `/RockLegendsPortal/Index`

---

## Назначение

Showcase портал рок-музыки: главная с карточками групп и случайным хитом из iTunes, сортировка по жанрам, управление жанрами (moderator), страница цитат через QuotesMinimalApi.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/RockLegendsPortal/Index` | `Index` | `Index.cshtml` | — |
| `/RockLegendsPortal/Quotes` | `Quotes` | `Quotes.cshtml` | — |
| `/RockLegendsPortal/SortByGenre` | `SortByGenre` | `SortByGenre.cshtml` | `[Authorize]` |
| `/RockLegendsPortal/DeleteGenre` | POST | Redirect | `[IsRockLegendsModerator]` |
| `/RockLegendsPortal/AddGenre` | GET/POST | `AddGenre.cshtml` | GET/POST: `[Authorize]` |
| `/RockLegendsPortal/LinkGroupToGenre` | POST | Redirect | `[Authorize]` |

> `Views/RockLegendsPortal/Details.cshtml` существует, но matching controller action отсутствует.

---

## Встроенное API

Route prefix: `api/rock-legends`

| Метод | Путь | Описание | Auth |
|-------|------|----------|------|
| POST | `api/rock-legends/like/{id}` | Лайк группы | `[Authorize]` + cookie `HasVotedInRockPoll` |
| GET | `api/rock-legends/validate-genre?name=` | Валидация имени жанра | — |
| — | `api/rock-legends/NotifyAboutGenre` | SignalR broadcast | — |

---

## SignalR

| Параметр | Значение |
|----------|----------|
| Hub | `RockLegendsHub` |
| Маршрут | `/my-hub/rock-legends` |
| Server → Client | `NewGenreCreated(genreName, urlCover)` |

**Frontend:** `wwwroot/js/rock-legends-portal/notification.js`

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `IRockLegendsPick` / `RockLegendsPick` | Маппинг биографий (инжектирован, не используется в actions) |
| `RockApi` | `https://itunes.apple.com` — `GetRandomRockHit` |
| `IRockLegendsRepository`, `IRockLegendsGenresRepository` | Data access |

---

## Модель данных

| Сущность | Описание |
|----------|----------|
| `RockLegendsData` | GroupNames, Likes, genre FK |
| `RockLegendsGenres` | Жанры |

---

## Frontend

- **CSS:** `style.css`, `notification.css`
- **JS:** `index.js`, `sort-by-genre.js`, `create-genre.js`, `quotes.js`, `notification.js`

---

## Локализация

| Файл | Языки |
|------|-------|
| `Localizations/RockLegendsPortal.resx` | EN |
| `RockLegendsportal.Ru.resx` | RU |

---

## Фоновые задачи

Нет.

---

## Внешние API-проекты

| API | Порт | JS-файл |
|-----|------|---------|
| [QuotesMinimalApi](../../../minimal-apis/quotes/README.md) | 7042 | `quotes.js` |

**Server-side:** RockApi (iTunes) на Index.

---

## Источники в коде

- `Controllers/RockLegendsPortalController.cs`
- `Controllers/ApiControllers/RockLegendsController.cs`
- `Hubs/RockLegendsHub.cs`
- `Services/RockLegendsPick.cs`, `Services/Apis/RockApi.cs`
- `Views/RockLegendsPortal/`
