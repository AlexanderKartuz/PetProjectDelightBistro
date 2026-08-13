# Документация DelightBistro

Центральное хранилище документации solution [DelightBistro](../DelightBistro.sln).

**Язык:** русский. Идентификаторы, URL и имена файлов — как в коде (английский).

## Категории

| Категория | Папка | Описание |
|-----------|-------|----------|
| Web (MVC) | [delight-bistro-mvc/](delight-bistro-mvc/) | Основное приложение DelightBistroMvc и его модули |
| Minimal API | [minimal-apis/](minimal-apis/) | Отдельные API-проекты |
| Libraries | [libraries/](libraries/) | Общие библиотеки (Data, MazeCore, DelightBistro.Services) |
| Console / examples | [other/](other/) | Учебные и консольные проекты (опционально) |

**Карта связей:** [integration-map.md](integration-map.md)

**Шаблоны:** [_templates/](_templates/)

---

## DelightBistroMvc

ASP.NET Core MVC-приложение с модулями — отдельными областями сайта. Маршрут по умолчанию: `DelightBistro/Index` (точка входа `/`).

→ [delight-bistro-mvc/README.md](delight-bistro-mvc/README.md) — полный список модулей

| Модуль | Папка | Точка входа |
|--------|-------|-------------|
| Platform (Home, Auth, User) | [modules/platform/](delight-bistro-mvc/modules/platform/) | `/Home/Index` |
| Notification | [modules/notification/](delight-bistro-mvc/modules/notification/) | `/Notification/Index` |
| DelightBistro | [modules/delight-bistro/](delight-bistro-mvc/modules/delight-bistro/) | `/` |

---

## Minimal APIs

| API | Папка | Порт | БД | Потребитель |
|-----|-------|------|-----|-------------|
| DelightBistroMinimalApi | [minimal-apis/delight-bistro/](minimal-apis/delight-bistro/) | 7090 | WebNet23Tea | DelightBistro MVC, [react-delight-bistro-app](../../react-delight-bistro-app/) |

→ [minimal-apis/README.md](minimal-apis/README.md)

---

## Libraries

| Библиотека | Папка | Используют |
|------------|-------|------------|
| DelightBistroMvc.Data | [libraries/delight-bistro-mvc-data/](libraries/delight-bistro-mvc-data/) | DelightBistroMvc |
| MazeCore | [libraries/maze-core/](libraries/maze-core/) | FirstConsoleApp |
| DelightBistro.Services | [libraries/delight-bistro-services/](libraries/delight-bistro-services/) | DelightBistroMinimalApi |

→ [libraries/README.md](libraries/README.md)

---

## Как обновлять документацию

1. Определите тип проекта (web-модуль, Minimal API, библиотека).
2. Используйте шаблон из [_templates/](_templates/).
3. Следуйте правилам Cursor: `.cursor/rules/docs-structure.mdc`, `.cursor/rules/docs-generation.mdc`.
4. При изменении связей MVC ↔ API обновите [integration-map.md](integration-map.md).
