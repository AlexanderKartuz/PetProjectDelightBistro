# JDM (Japanese Domestic Market)

> Каталог JDM-автомобилей, конструктор, журнал с комментариями, мерч через Minimal API.

**Контроллер:** `JdmController`  
**Layout:** `_LayoutJdm.cshtml`  
**Точка входа:** `/Jdm/Home`

---

## Назначение

Фан-сайт японского автопрома: каталог машин по производителю, добавление объявлений, конструктор, блог-журнал с комментариями и real-time уведомления о новых авто. Мерч вынесен в отдельный Minimal API.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/Jdm/Home` | `Home` | `Home.cshtml` | — |
| `/Jdm/Catalog?manufacturerType=` | `Catalog` | `Catalog.cshtml` | — |
| `/Jdm/CreateCars` | GET/POST | `CreateCars.cshtml` | `[Authorize]` |
| `/Jdm/Builder` | GET/POST | `Builder.cshtml` | `[Authorize]` |
| `/Jdm/Journal` | `Journal` | `Journal.cshtml` | — |
| `/Jdm/AddComment` | POST | Redirect | `[Authorize]` |
| `/Jdm/DeleteComments` | GET | Redirect (stub) | `[IsJdmOwner]` |
| `/Jdm/DeleteOldPosts` | POST | — | `[IsAdmin]` + `[IsJdmOwner]` |
| `/Jdm/GetJdmCarsContact?id=` | GET | JSON | — |

POST `CreateCars` → SignalR `NewJdmCarsCreated`.

---

## Встроенное API

| Метод | Путь | Описание | Auth |
|-------|------|----------|------|
| GET | `/api/Jdm/NotifyAboutJdmCars?model=&price=&url=` | SignalR broadcast | — |

---

## SignalR

| Параметр | Значение |
|----------|----------|
| Hub | `JdmHub` |
| Маршрут | `/my-hub/jdm` |
| Server → Client | `NewJdmCarsCreated(model, price, url)` |

**Frontend:** `wwwroot/js/japanese-domestic-market/notice.js`

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `IJdmGenerator` / `JdmGenerator` | Seed/demo car items, select lists |
| `IJdmCatalogGenerator` / `JDMCatalogGenerator` | Фильтрация каталога |
| `IJdmRepository`, `IJdmManufacturerRepository`, `IJdmPostsRepository`, `IJdmJournalCommentRepository` | Data access |

---

## Модель данных

| Сущность | Описание |
|----------|----------|
| `JdmCarsData` | Объявления авто |
| `JdmManufacturerData` | Производители |
| `JdmPostsData` | Посты журнала |
| `JdmCarsBlogCommentsData` | Комментарии к журналу |

---

## Frontend

- **CSS:** `wwwroot/css/japanese-domestic-market/jdm-style.css`
- **JS:** `notice.js`, `createJdmMerch.js`
- **Partial:** `_JournalJdmCommentPartial.cshtml`

---

## Локализация

| Файл | Языки |
|------|-------|
| `Localizations/JapaneseDomesticMarket.resx` | EN |
| `JapaneseDomesticMarket.Ru.resx` | RU |

---

## Фоновые задачи

Нет.

---

## Внешние API-проекты

| API | Порт | JS-файл |
|-----|------|---------|
| [JdmMerchMinimalApi](../../../minimal-apis/jdm-merch/README.md) | 7001 | `createJdmMerch.js` |

---

## Источники в коде

- `Controllers/JdmController.cs`
- `Controllers/ApiControllers/JdmController.cs`
- `Hubs/JdmHub.cs`
- `Services/JdmGenerator.cs`, `JDMCatalogGenerator.cs`
- `Views/Jdm/`
