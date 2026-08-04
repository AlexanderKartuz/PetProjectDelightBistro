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
| DelightBistro | [modules/delight-bistro/](modules/delight-bistro/) | DelightBistroController | `/DelightBistro/Index` | да |

---

## Справочники

| Документ | Описание |
|----------|----------|
| [appendix/signalr-hubs.md](appendix/signalr-hubs.md) | SignalR-хабы и маршруты |
| [appendix/custom-auth-attributes.md](appendix/custom-auth-attributes.md) | Кастомные атрибуты авторизации |
| [appendix/external-apis.md](appendix/external-apis.md) | Внешние HTTP API (CatFact, Dog и др.) |
| [appendix/background-jobs.md](appendix/background-jobs.md) | Фоновые сервисы |

---

## Связанные проекты

- [WebNet23Online.Data](../libraries/web-net23-online-data/README.md) — основная БД
- [DelightBistroMinimalApi](../minimal-apis/delight-bistro/README.md) — каталог чая
- [integration-map.md](../integration-map.md) — карта связей
