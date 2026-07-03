# RockBands

> Каталог пользовательских рок-групп с фильтрацией по жанрам, лайки, загрузка изображений.

**Контроллер:** `RockBandsController`  
**Layout:** `_LayoutRockBands.cshtml`  
**Точка входа:** `/RockBands/Index`

---

## Назначение

Пользователи создают карточки рок-групп с изображениями, фильтруют по жанрам, ставят лайки. Владелец группы (`RockBandOwner`) может редактировать жанры. Real-time уведомления о новых группах через SignalR (hub не зарегистрирован в `Program.cs`).

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/RockBands/Index?genreIds=&editBandId=` | `Index` GET | `Index.cshtml` | — (auth влияет на UI) |
| `/RockBands/Index` | POST (create) | Redirect | `[Authorize]` |
| `/RockBands/UpdateGenres` | POST | Redirect | `[Authorize]` + `[IsRockBandOwner]` |

---

## Встроенное API

| Метод | Путь | Описание | Auth |
|-------|------|----------|------|
| POST | `/api/RockBands/AddLike?bandId=` | Лайк группы | `[Authorize]` |
| GET | `/api/RockBands/IsBandNameFree?name=` | Проверка имени (delay 1s) | — |

---

## SignalR

| Параметр | Значение |
|----------|----------|
| Hub | `RockBandHub` |
| Ожидаемый маршрут | `/my-hub/rock-band` |
| Server → Client | `NewRockBandWasCreated(name, url)` |

**Проблема:** `MapHub<RockBandHub>` **не зарегистрирован** в `Program.cs`. JS `notification.js` указывает на `https://localhost:7284/my-hub/rock-band` — не работает до регистрации.

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `IRockBandsService` / `RockBandsService` | CRUD, likes, genres, image upload |
| `IRockBandsRepository`, `IRockBandLikeRepository`, `IGenreOfRockBandsRepository` | Data access |

---

## Модель данных

| Сущность | Описание |
|----------|----------|
| `RockBandsData` | Группы |
| `RockBandLikeData` | Лайки |
| `GenreOfRockBandsData` | Жанры |
| `RockBandGenreData` | M:M join |

---

## Frontend

- **CSS:** `style-band.css`, `notification.css`
- **JS:** `index.js`, `create-rockband.js`, `notification.js`

---

## Локализация

| Файл | Языки |
|------|-------|
| `Localizations/RockBand.resx` | EN |
| `RockBand.Ru.resx` | RU |

---

## Фоновые задачи

Нет.

---

## Источники в коде

- `Controllers/RockBandsController.cs`
- `Controllers/ApiControllers/RockBandsController.cs`
- `Hubs/RockBandHub.cs` (не mapped)
- `Services/RockBandsService.cs`
- `Views/RockBands/Index.cshtml`
