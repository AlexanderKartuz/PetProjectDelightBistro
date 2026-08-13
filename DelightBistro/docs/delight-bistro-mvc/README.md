# DelightBistroMvc

ASP.NET Core MVC-приложение — основной веб-сайт solution DelightBistro.

**Проект:** [DelightBistroMvc/DelightBistroMvc.csproj](../../DelightBistroMvc/DelightBistroMvc.csproj)  
**HTTPS:** `https://localhost:7284`  
**Точка входа:** `/` (`DelightBistro/Index`, маршрут по умолчанию в `Program.cs`)  
**Layout по умолчанию:** `_Layout.cshtml` (модуль DelightBistro использует `_LayoutDelightBistro.cshtml`)

---

## Модули

| Модуль | Папка | Контроллер | Точка входа | SignalR |
|--------|-------|------------|-------------|---------|
| Platform | [modules/platform/](modules/platform/) | Home, Auth, User | `/Home/Index` | — |
| Notification | [modules/notification/](modules/notification/) | NotificationController | `/Notification/Index` | да |
| DelightBistro | [modules/delight-bistro/](modules/delight-bistro/) | DelightBistroController | `/`, `/DelightBistro/Index` | да |

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

- [DelightBistroMvc.Data](../libraries/delight-bistro-mvc-data/README.md) — основная БД
- [DelightBistroMinimalApi](../minimal-apis/delight-bistro/README.md) — каталог напитков
- [integration-map.md](../integration-map.md) — карта связей
