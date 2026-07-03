# Recommendations (Steam)

> Рекомендации игр через RAWG API — поиск, популярные и новые релизы.

**Контроллер:** `RecommendationsController`  
**Родительский модуль:** [Steam](README.md)  
**Layout:** `_LayoutSteam.cshtml`

---

## Назначение

Спутник Steam: отдельный раздел с рекомендациями игр из внешнего API RAWG. Поиск, популярные игры, новые релизы и детальная страница игры по slug.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/Steam/Recommendations/IndexRecommendations` | `IndexRecommendations` | `IndexRecommendations.cshtml` | — |
| `/Steam/Recommendations/Game/{slug}` | `GameDetails` | `GameDetails.cshtml` | — |

---

## Встроенное API

Нет (данные через `RawgApi` на сервере).

---

## SignalR

Нет.

---

## Сервисы и зависимости

| Сервис | Назначение |
|--------|------------|
| `RawgApi` | `https://api.rawg.io/api/` (ключ из config `RAWG:ApiKey`) |

---

## Модель данных

Не использует локальные сущности — данные из RAWG API.

---

## Frontend

- **Views:** `Views/Recommendations/IndexRecommendations.cshtml`, `GameDetails.cshtml`
- **CSS:** `wwwroot/css/steam/recommendations.css`

---

## Локализация

Нет dedicated `.resx` — тексты inline на английском.

---

## Источники в коде

- `Controllers/RecommendationsController.cs`
- `Services/Apis/steam/RawgApi.cs`
- `Views/Recommendations/`
