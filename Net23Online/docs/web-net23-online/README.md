# WebNet23Online

ASP.NET Core MVC-приложение — основной веб-сайт solution Net23Online.

**Проект:** [WebNet23Online/WebNet23Online.csproj](../WebNet23Online/WebNet23Online.csproj)  
**Точка входа:** `/` (Home/Index)  
**Layout по умолчанию:** `_Layout.cshtml`

---

## Модули

| Модуль | Папка | Контроллер | Точка входа | SignalR |
|--------|-------|------------|-------------|---------|
| Platform | [modules/platform/](modules/platform/) | Home, Auth, User | `/` | — |
| Notification | [modules/notification/](modules/notification/) | NotificationController | `/Notification/Index` | да |
| AnimeGirl | [modules/anime-girl/](modules/anime-girl/) | AnimeGirlController | `/AnimeGirl/Index` | да |
| AnimalWorld | [modules/animal-world/](modules/animal-world/) | AnimalWorldController | `/AnimalWorld/Index` | да |
| DelightBistro | [modules/delight-bistro/](modules/delight-bistro/) | DelightBistroController | `/DelightBistro/Index` | да |
| LittleLemon | [modules/little-lemon/](modules/little-lemon/) | LittleLemonController | `/LittleLemon/Index` | да |
| Steam | [modules/steam/](modules/steam/) | SteamController | `/Steam/Index` | да |
| HabitTracker | [modules/habit-tracker/](modules/habit-tracker/) | HabitTrackerController | `/HabitTracker/Index` | — |
| JDM | [modules/jdm/](modules/jdm/) | JdmController | `/Jdm/Home` | да |
| RockLegendsPortal | [modules/rock-legends-portal/](modules/rock-legends-portal/) | RockLegendsPortalController | `/RockLegendsPortal/Index` | да |
| RockBands | [modules/rock-bands/](modules/rock-bands/) | RockBandsController | `/RockBands/Index` | — |
| SlayTheSpire2 | [modules/slay-the-spire-2/](modules/slay-the-spire-2/) | SlayTheSpire2Controller | `/SlayTheSpire2/Index` | — |
| Maze | [modules/maze/](modules/maze/) | MazeController | `/Maze/Index` | — |
| EventWorkshop | [modules/event-workshop/](modules/event-workshop/) | EventWorkshopController | `/EventWorkshop/Index` | — |
| MaksKorz | [modules/maks-korz/](modules/maks-korz/) | MaksKorzController | `/MaksKorz/Index` | — |

---

## Справочники

| Документ | Описание |
|----------|----------|
| [appendix/signalr-hubs.md](appendix/signalr-hubs.md) | SignalR-хабы и маршруты |
| [appendix/custom-auth-attributes.md](appendix/custom-auth-attributes.md) | Кастомные атрибуты авторизации |
| [appendix/external-apis.md](appendix/external-apis.md) | Внешние HTTP API (JokeApi, RawgApi и др.) |
| [appendix/background-jobs.md](appendix/background-jobs.md) | Фоновые сервисы и Quartz jobs |

---

## Связанные проекты

- [WebNet23Online.Data](../libraries/web-net23-online-data/README.md) — основная БД
- [Minimal APIs](../minimal-apis/README.md) — внешние API, вызываемые из JS
- [integration-map.md](../integration-map.md) — карта связей
