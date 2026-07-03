# SlayTheSpire2

> Фан-сайт Slay the Spire 2: лендинг, галерея реликвий, герои с CRUD карт, Kickstarter.

**Контроллер:** `SlayTheSpire2Controller`  
**Layout:** `_LayoutSLayTheSpire.cshtml`  
**Точка входа:** `/SlayTheSpire2/Index`

---

## Назначение

Фан-сайт STS2: главная страница, галерея реликвий (через Minimal API), страницы героев с картами (CRUD для creator/admin), форма Kickstarter с reward image по tier.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/SlayTheSpire2/Index` | `Index` | `Index.cshtml` | — |
| `/SlayTheSpire2/Relics` | `Relics` | `Relics.cshtml` | — |
| `/SlayTheSpire2/Heroes?id=` | `Heroes` | `Heroes.cshtml` | — |
| `/SlayTheSpire2/AddCard?heroId=` | GET/POST | — | `[Authorize]` |
| `/SlayTheSpire2/EditCard?id=` | GET/POST | `EditCard.cshtml` | `[Authorize]` + `[IsSlayTheSpire2CreatorOrAdmin]` |
| `/SlayTheSpire2/KickStarter` | GET/POST | `KickStarter.cshtml` | — |

---

## Встроенное API

Нет.

---

## SignalR

Нет.

---

## Сервисы и зависимости

| Сервис | Lifetime | Назначение |
|--------|----------|------------|
| `ISlayTheSpire2RewardImageService` | Singleton | Kickstarter reward image by tier |
| `ISlayTheSpire2CardOptionsService` | Singleton | Rarity/type select lists |
| `ISlayTheSpire2HeroesRepository`, `ISlayTheSpire2HeroesCardsRepository` | Scoped | Data access |

---

## Модель данных

| Сущность | Поля |
|----------|------|
| `SlayTheSpire2HeroesData` | Name, Color |
| `SlayTheSpire2HeroesCards` | Card fields, `CreatedByUserId`, `ModifiedByUserId`, `ModifiedAt` |

---

## Frontend

- **CSS:** `wwwroot/css/slay-the-spire-2/style.css`
- **JS:** `Hero.js`, `card-form.js`, `Relics.js`

---

## Локализация

| Файл | Языки |
|------|-------|
| `Localizations/SlayTheSpire2Index.resx` | EN |
| `SlayTheSpire2Index.Ru.resx` | RU |

---

## Фоновые задачи

Нет.

---

## Внешние API-проекты

| API | Порт | JS-файл |
|-----|------|---------|
| [SlayTheSpire2RelicsMinimalApi](../../../minimal-apis/slay-the-spire-2-relics/README.md) | 7050 | `Relics.js` |

---

## Источники в коде

- `Controllers/SlayTheSpire2Controller.cs`
- `Services/SlayTheSpire2Services&Interfaces/`
- `Views/SlayTheSpire2/`
