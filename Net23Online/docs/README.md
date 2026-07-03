# Документация Net23Online

Центральное хранилище документации solution [Net23Online](../Net23Online.sln).

**Язык:** русский. Идентификаторы, URL и имена файлов — как в коде (английский).

## Категории

| Категория | Папка | Описание |
|-----------|-------|----------|
| Web (MVC) | [web-net23-online/](web-net23-online/) | Основное приложение WebNet23Online и его модули |
| Minimal API | [minimal-apis/](minimal-apis/) | Отдельные API-проекты |
| Libraries | [libraries/](libraries/) | Общие библиотеки (Data, MazeCore) |
| Console / examples | [other/](other/) | Учебные и консольные проекты (опционально) |

**Карта связей:** [integration-map.md](integration-map.md)

**Шаблоны:** [_templates/](_templates/)

---

## WebNet23Online

ASP.NET Core MVC-приложение с модулями — отдельными областями сайта.

→ [web-net23-online/README.md](web-net23-online/README.md) — полный список модулей

| Модуль | Папка | Точка входа |
|--------|-------|-------------|
| Platform (Home, Auth, User) | [modules/platform/](web-net23-online/modules/platform/) | `/` |
| Notification | [modules/notification/](web-net23-online/modules/notification/) | `/Notification/Index` |
| AnimeGirl | [modules/anime-girl/](web-net23-online/modules/anime-girl/) | `/AnimeGirl/Index` |
| AnimalWorld | [modules/animal-world/](web-net23-online/modules/animal-world/) | `/AnimalWorld/Index` |
| DelightBistro | [modules/delight-bistro/](web-net23-online/modules/delight-bistro/) | `/DelightBistro/Index` |
| LittleLemon | [modules/little-lemon/](web-net23-online/modules/little-lemon/) | `/LittleLemon/Index` |
| Steam | [modules/steam/](web-net23-online/modules/steam/) | `/Steam/Index` |
| HabitTracker | [modules/habit-tracker/](web-net23-online/modules/habit-tracker/) | `/HabitTracker/Index` |
| JDM | [modules/jdm/](web-net23-online/modules/jdm/) | `/Jdm/Home` |
| RockLegendsPortal | [modules/rock-legends-portal/](web-net23-online/modules/rock-legends-portal/) | `/RockLegendsPortal/Index` |
| RockBands | [modules/rock-bands/](web-net23-online/modules/rock-bands/) | `/RockBands/Index` |
| SlayTheSpire2 | [modules/slay-the-spire-2/](web-net23-online/modules/slay-the-spire-2/) | `/SlayTheSpire2/Index` |
| Maze | [modules/maze/](web-net23-online/modules/maze/) | `/Maze/Index` |
| EventWorkshop | [modules/event-workshop/](web-net23-online/modules/event-workshop/) | `/EventWorkshop/Index` |
| MaksKorz | [modules/maks-korz/](web-net23-online/modules/maks-korz/) | `/MaksKorz/Index` |

---

## Minimal APIs

| API | Папка | Порт | БД | Потребитель |
|-----|-------|------|-----|-------------|
| MovieMinimalApi | [minimal-apis/movie/](minimal-apis/movie/) | 7142 | WebNet23Movie | AnimeGirl |
| AnimalWorldMinimalApi | [minimal-apis/animal-world/](minimal-apis/animal-world/) | 7264 | WebNet23AnimalFacts | AnimalWorld |
| DelightBistroMinimalApi | [minimal-apis/delight-bistro/](minimal-apis/delight-bistro/) | 7090 | WebNet23Tea | DelightBistro |
| JdmMerchMinimalApi | [minimal-apis/jdm-merch/](minimal-apis/jdm-merch/) | 7001 | WebNet23JdmMerch | JDM |
| QuotesMinimalApi | [minimal-apis/quotes/](minimal-apis/quotes/) | 7042 | WebNet23Quote | RockLegendsPortal |
| SlayTheSpire2RelicsMinimalApi | [minimal-apis/slay-the-spire-2-relics/](minimal-apis/slay-the-spire-2-relics/) | 7050 | WebNet23SlayTheSpire2Relic | SlayTheSpire2 |
| LittleLemonMinimalApi | [minimal-apis/little-lemon/](minimal-apis/little-lemon/) | 7100 | WebNet23LittleLemon | — (standalone) |

→ [minimal-apis/README.md](minimal-apis/README.md)

---

## Libraries

| Библиотека | Папка |
|------------|-------|
| WebNet23Online.Data | [libraries/web-net23-online-data/](libraries/web-net23-online-data/) |
| MazeCore | [libraries/maze-core/](libraries/maze-core/) |

→ [libraries/README.md](libraries/README.md)

---

## Как обновлять документацию

1. Определите тип проекта (web-модуль, Minimal API, библиотека).
2. Используйте шаблон из [_templates/](_templates/).
3. Следуйте правилам Cursor: `.cursor/rules/docs-structure.mdc`, `.cursor/rules/docs-generation.mdc`.
4. При изменении связей MVC ↔ API обновите [integration-map.md](integration-map.md).
