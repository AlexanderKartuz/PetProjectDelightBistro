# AnimeGirl

> Каталог аниме-персонажей и тайтлов, виджеты внешних API, real-time чат и шаринг персонажей.

**Контроллер:** `AnimeGirlController`  
**Layout:** `_LayoutAnime.cshtml`  
**Точка входа:** `/AnimeGirl/Index`

---

## Назначение

Модуль для просмотра и управления каталогом аниме-персонажей и тайтлов. Поддерживает создание записей, связывание персонажей с аниме, сортировку в таблицах, real-time чат и интеграцию с внешними виджетами (шутки, waifu, коты, фильмы).

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/AnimeGirl/Index` | `Index` | `Index.cshtml` | — |
| `/AnimeGirl/CreateGirl` | `CreateGirl` GET/POST | `CreateGirl.cshtml` | `[Authorize]` |
| `/AnimeGirl/CreateAnime` | `CreateAnime` GET/POST | `CreateAnime.cshtml` | `[Authorize]` |
| `/AnimeGirl/LinkAnimeAndGirl` | POST | Redirect Index | — |
| `/AnimeGirl/Handmade` | `Handmade` | `Handmade.cshtml` | — |
| `/AnimeGirl/Delete?id=` | `Delete` | Redirect Index | UI: moderator only |
| `/AnimeGirl/TableData` | `TableData` | `TableData.cshtml` | — |
| `/AnimeGirl/AnimeTableData` | `AnimeTableData` | `AnimeTableData.cshtml` | — |

`CanDeleteGirl` в Index = `AtLeastModerator()` — управляет видимостью кнопок удаления.

---

## Встроенное API

| Метод | Путь | Описание | Auth |
|-------|------|----------|------|
| GET | `/api/AnimeGirl/Delete?ids=` | Массовое удаление персонажей | — |
| GET | `/api/Anime/UpdateName?id=&name=` | Переименование аниме | — |
| GET | `/api/Anime/NotifyAboutAnime?name=` | SignalR broadcast `NewAnimeCreated` | — |

---

## SignalR

| Параметр | Значение |
|----------|----------|
| Hub | `AnimeHub` |
| Маршрут | `/my-hub/anime` |
| Lifetime chat-сервиса | Singleton |

**Client → Server:**

| Метод | Описание |
|-------|----------|
| `JoinChat()` | Вход в группу `anime-girl-chat`, назначение nickname |
| `LeaveChat()` | Выход из чата |
| `SendMessage(message)` | Сообщение в группу |
| `ShareCharacters(characterIds)` | Шаринг карточек (+likes, max 10) |

**Server → Client:**

| Событие | Аргументы |
|---------|-----------|
| `NewAnimeCreated` | `animeName`, `urlCover` |
| `ReceiveMessage` | `senderName`, `message` |
| `UserJoinedChat` / `UserLeftChat` | `userName` |
| `SetUserName` | `userName` |
| `ReceiveSharedCharacters` | `senderName`, `SharedCharacterChatItem[]` |

Nickname: имя пользователя → иначе случайное adjective+noun через `AnimeGirlChatNicknameService`.

---

## Сервисы и зависимости

| Сервис | Lifetime | Назначение |
|--------|----------|------------|
| `IAnimeGirlService` / `AnimeGirlGenerator` | Scoped | Генерация списков, карта аниме |
| `IAnimeGirlChatService` / `AnimeGirlChatService` | Singleton | In-memory состояние чата |
| `IAnimeGirlChatNicknameService` | Scoped | Генерация nickname |
| `IEpicMeanlessPhraseGenerator` | Scoped | Случайные заголовки |
| `IAnimeGirlRepository`, `IAnimeRepository` | Scoped | CRUD, likes, link |

**Внешние HTTP API (server-side):**

| API | Base URL |
|-----|----------|
| `JokeApi` | `https://official-joke-api.appspot.com` |
| `WaifuApi` | `https://api.waifu.im` |
| `CatApi` | `https://cataas.com` |

---

## Модель данных

| Сущность | Связи |
|----------|-------|
| `AnimeGirlData` | M2M → `AnimeData` (join `AnimeDataAnimeGirlData`) |
| `AnimeData` | M2M → `AnimeGirlData`, optional `AnimeStudioData` |
| `AnimeStudioData` | → `AnimeData` |

DbSets: `AnimeGirls`, `Animes`, `AnimeStudios`.

---

## Frontend

- **Layout:** `_LayoutAnime.cshtml`
- **CSS:** `wwwroot/css/anime-girl/` — `style.css`, `style-nice.css`, `chat.css`, `handmade.css`
- **JS:** `anime-girl-signalr.js`, `notification.js`, `sort.js`, `index.js`, `anime-girl-chat.js`, `create-girl.js`
- **Images:** `wwwroot/images/anime-girl/` (upload `girl-{id}.jpg`)

---

## Локализация

| Файл | Языки |
|------|-------|
| `Localizations/AnimeGirl.resx` | EN |
| `AnimeGirl.Ru.resx` | RU |

Ключи: `Index_Header`, `Index_Header_TagLine`.

---

## Фоновые задачи

Нет.

---

## Внешние API-проекты

| API | Порт | JS-файл | Эндпоинты |
|-----|------|---------|-----------|
| [MovieMinimalApi](../../../minimal-apis/movie/README.md) | 7142 | `index.js` | `GET /GetMovies`, `POST /CreateMovie` |

---

## Источники в коде

- `Controllers/AnimeGirlController.cs`
- `Controllers/ApiControllers/AnimeGirlController.cs`, `AnimeController.cs`
- `Hubs/AnimeHub.cs`
- `Services/AnimeGirlGenerator.cs`, `AnimeGirlChatService.cs`, `AnimeGirlChatNicknameService.cs`
- `Views/AnimeGirl/`
- `WebNet23Online.Data/Repositories/AnimeGirlRepository.cs`, `AnimeRepository.cs`
