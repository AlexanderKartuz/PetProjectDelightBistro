# Внешние HTTP API

HttpClient-сервисы, зарегистрированные в `Program.cs`, и Minimal API, вызываемые из frontend.

---

## Server-side HttpClient (`Program.cs`)

| Client | Base URL | Модуль | Использование |
|--------|----------|--------|---------------|
| `CatFactApi` | `https://catfact.ninja` | DelightBistro | Факт о котах на Index |
| `DogApi` | `https://dog.ceo` | DelightBistro | Случайное фото собаки на Index |
| `JokeApi` | `https://official-joke-api.appspot.com` | — | Зарегистрирован, **не используется** |

---

## Minimal API (frontend fetch)

| API | Порт | JS-файл | Модуль |
|-----|------|---------|--------|
| DelightBistroMinimalApi | 7090 | `delight-bistro/drink.js` | DelightBistro |

Вне solution: `react-delight-bistro-app/src/services/drinks-service.ts` — тот же порт и эндпоинты `GetDrinks` / `CreateDrink` / `ChangeDrink` / `DeleteDrink` / `GetDrink/{id}`.

→ Подробнее: [integration-map.md](../../integration-map.md), [minimal-apis/README.md](../../minimal-apis/README.md)

---

## Источники в коде

- `Program.cs` — `AddHttpClient<T>()`
- `Services/Apis/` — client classes
- `wwwroot/js/delight-bistro/drink.js` — frontend consumer
