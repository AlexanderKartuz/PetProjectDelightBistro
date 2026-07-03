# AnimalWorld

> Виртуальный зоопарк: зоопарки, семейства/виды животных, акции, галерея, интересные факты.

**Контроллер:** `AnimalWorldController`  
**Layout:** `_LayoutAnimalWorld.cshtml`  
**Точка входа:** `/AnimalWorld/Index`

---

## Назначение

Экосистема виртуального зоопарка: CRUD зоопарков, семейств и видов, привязка видов к зоопаркам, акции с real-time уведомлениями, галерея случайных животных, поиск по видам и интересные факты через отдельный Minimal API.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/AnimalWorld/Index` | `Index` | `Index.cshtml` | — |
| `/AnimalWorld/Add` | `Add` | `Add.cshtml` | `[Authorize]` + `[IsModerator]` |
| `/AnimalWorld/AddZoo` | GET/POST | `AddZoo.cshtml` | Moderator |
| `/AnimalWorld/AddFamily` | GET/POST | `AddFamily.cshtml` | Moderator |
| `/AnimalWorld/AddSpecies` | GET/POST | `AddSpecies.cshtml` | Moderator |
| `/AnimalWorld/BindZooAndAnimalSpecies` | GET/POST | `BindZooAndAnimalSpecies.cshtml` | Moderator |
| `/AnimalWorld/Zoos` | `Zoos` | `Zoos.cshtml` | `[Authorize]` |
| `/AnimalWorld/Promotions` | `Promotions` | `Promotions.cshtml` | — |
| `/AnimalWorld/AddPromotion` | GET/POST | `AddPromotion.cshtml` | Moderator |
| `/AnimalWorld/Gallery` | `Gallery` | `Gallery.cshtml` | — |
| `/AnimalWorld/AnimalSpeciesInfo` | `AnimalSpeciesInfo` | `AnimalSpeciesInfo.cshtml` | — |
| `/AnimalWorld/InterestingFacts` | `InterestingFacts` | `InterestingFacts.cshtml` | — |

POST `BindZooAndAnimalSpecies` → SignalR `NewAnimalInZooAppeared`.

---

## Встроенное API

| Метод | Путь | Описание | Auth |
|-------|------|----------|------|
| GET | `/api/AnimalWorld/IsZooNameFree?zooName=` | Проверка уникальности имени зоопарка | — |
| GET | `/api/AnimalWorld/GetAnimalSpeciesNames` | Список имён видов | — |

---

## SignalR

| Hub | Маршрут | Событие | Триггер |
|-----|---------|---------|---------|
| `AnimalWorldHub` | `/my-hub/animal-world` | `NewAnimalInZooAppeared(zooName, speciesName)` | Bind zoo ↔ species |
| `AnimalWorldNotificationsHub` | `/my-hub/animal-world-promotions` | `ZoosPromotions(message)` | Quartz job акций |

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `IAnimalWorldService` / `AnimalWorldService` | CRUD, upload изображений, gallery |
| `IAnimalWorldMapper` / `AnimalWorldMapper` | Маппинг view models |
| `IZooRepository`, `IAnimalFamilyRepository`, `IAnimalSpeciesRepository`, `IPromotionRepository` | Data access |
| `AnimalWorldRandomAnimalApi` | `https://api.some-random-api.com` — gallery |

**Фоновая задача:** `AnimalWorldPromotionsCheckJob` — cron `0 0 9-20 ? * *` (ежечасно 9–20).

---

## Модель данных

| Сущность | Связи |
|----------|-------|
| `ZooData` | M2M `AnimalSpecies`, `Tickets`, `Comments`, `Promotions`, `Creator` |
| `AnimalFamilyData` | `Species`, `Creator` |
| `AnimalSpeciesData` | `AnimalFamily`, M2M `ZooData`, `Creator` |
| `PromotionData` | `Venue` (Zoo), `Creator` |

DbSets: `Zoos`, `AnimalFamilies`, `AnimalSpecies`, `Promotions`.

---

## Frontend

- **Layout:** `_LayoutAnimalWorld.cshtml`
- **CSS:** `animal-world-style.css`, `random-animals.css`, `species-table.css`, `notifications.css`
- **JS:** `new-animal-species-appeared.js`, `promotions.js`, `add-zoo.js`, `add-species.js`, `animal-species-facts.js`, `index.js`
- **Images:** `wwwroot/images/animal-world/`

---

## Локализация

| Файл | Языки |
|------|-------|
| `Localizations/AnimalWorld.resx` | EN |
| `AnimalWorld.Ru.resx` | RU |

Ключи: `AboutPlanet`, `Add`, `AddZoo`, `AnimalFamilies`, `AnimalSpecies`, `Reservation`, `Zoos` и др.

---

## Фоновые задачи

| Job | Расписание | Назначение |
|-----|------------|------------|
| `AnimalWorldPromotionsCheckJob` | Hourly 9–20 | Истечение акций, broadcast через `AnimalWorldNotificationsHub` |

---

## Внешние API-проекты

| API | Порт | JS-файл |
|-----|------|---------|
| [AnimalWorldMinimalApi](../../../minimal-apis/animal-world/README.md) | 7264 | `animal-species-facts.js` |

---

## Связанные модули

| Спутник | Документ |
|---------|----------|
| Comments | [comments.md](comments.md) |
| Tickets | [tickets.md](tickets.md) |

---

## Источники в коде

- `Controllers/AnimalWorldController.cs`
- `Controllers/ApiControllers/AnimalWorldController.cs`
- `Hubs/AnimalWorldHub.cs`, `AnimalWorldNotificationsHub.cs`
- `Services/AnimalWorldService.cs`, `Services/Jobs/AnimalWorldPromotionsCheckJob.cs`
- `Views/AnimalWorld/`
- `WebNet23Online.Data/Repositories/AnimalWorld/`
