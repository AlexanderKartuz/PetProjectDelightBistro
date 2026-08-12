# Документация Net23Online

Центральное хранилище документации solution [Net23Online](../Net23Online.sln).

**Язык:** русский. Идентификаторы, URL и имена файлов — как в коде (английский).

## Категории

| Категория | Папка | Описание |
|-----------|-------|----------|
| Web (MVC) | [web-net23-online/](web-net23-online/) | Основное приложение WebNet23Online и его модули |
| Minimal API | [minimal-apis/](minimal-apis/) | Отдельные API-проекты |
| Libraries | [libraries/](libraries/) | Общие библиотеки (Data, MazeCore, DelightBistro.Services) |
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
| DelightBistro | [modules/delight-bistro/](web-net23-online/modules/delight-bistro/) | `/DelightBistro/Index` |

---

## Minimal APIs

| API | Папка | Порт | БД | Потребитель |
|-----|-------|------|-----|-------------|
| DelightBistroMinimalApi | [minimal-apis/delight-bistro/](minimal-apis/delight-bistro/) | 7090 | WebNet23Tea | DelightBistro |

→ [minimal-apis/README.md](minimal-apis/README.md)

---

## Libraries

| Библиотека | Папка | Используют |
|------------|-------|------------|
| WebNet23Online.Data | [libraries/web-net23-online-data/](libraries/web-net23-online-data/) | WebNet23Online |
| MazeCore | [libraries/maze-core/](libraries/maze-core/) | FirstConsoleApp |
| DelightBistro.Services | [libraries/delight-bistro-services/](libraries/delight-bistro-services/) | DelightBistroMinimalApi |

→ [libraries/README.md](libraries/README.md)

---

## Как обновлять документацию

1. Определите тип проекта (web-модуль, Minimal API, библиотека).
2. Используйте шаблон из [_templates/](_templates/).
3. Следуйте правилам Cursor: `.cursor/rules/docs-structure.mdc`, `.cursor/rules/docs-generation.mdc`.
4. При изменении связей MVC ↔ API обновите [integration-map.md](integration-map.md).
