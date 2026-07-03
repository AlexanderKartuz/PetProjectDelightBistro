# Внешние HTTP API

HttpClient-сервисы, зарегистрированные в `Program.cs`, и Minimal API, вызываемые из frontend.

---

## Server-side HttpClient (`Program.cs`)

| Client | Base URL | Модуль | Эндпоинты |
|--------|----------|--------|-----------|
| `JokeApi` | `https://official-joke-api.appspot.com` | AnimeGirl | `GET /jokes/random` |
| `WaifuApi` | `https://api.waifu.im` | AnimeGirl | `GET /images` |
| `CatApi` | `https://cataas.com` | AnimeGirl | `GET /api/cats` |
| `AnimalWorldRandomAnimalApi` | `https://api.some-random-api.com` | AnimalWorld | `GET /animal`, `GET {type}` |
| `CatFactApi` | `https://catfact.ninja` | — | Не привязан к модулю |
| `DogApi` | `https://dog.ceo` | — | Не привязан к модулю |
| `RawgApi` | `https://api.rawg.io/api/` | Steam (Recommendations) | Search, popular, game details |
| `RockApi` | `https://itunes.apple.com` | RockLegendsPortal | `GetRandomRockHit` |
| `FakeRestaurantApi` | `https://fakerestaurantapi.runasp.net` | LittleLemon | `/api/Restaurant/5/menu` |

---

## Minimal API (frontend fetch)

| API | Порт | JS-файл | Модуль |
|-----|------|---------|--------|
| MovieMinimalApi | 7142 | `anime-girl/index.js` | AnimeGirl |
| AnimalWorldMinimalApi | 7264 | `animal-world/animal-species-facts.js` | AnimalWorld |
| DelightBistroMinimalApi | 7090 | `delight-bistro/tea.js` | DelightBistro |
| JdmMerchMinimalApi | 7001 | `japanese-domestic-market/createJdmMerch.js` | JDM |
| QuotesMinimalApi | 7042 | `rock-legends-portal/quotes.js` | RockLegendsPortal |
| SlayTheSpire2RelicsMinimalApi | 7050 | `SlayTheSpire2/Relics.js` | SlayTheSpire2 |
| LittleLemonMinimalApi | 7100 | — | Standalone (нет интеграции) |

→ Подробнее: [integration-map.md](../../integration-map.md), [minimal-apis/README.md](../../minimal-apis/README.md)

---

## Источники в коде

- `Program.cs` — `AddHttpClient<T>()`
- `Services/Apis/` — client classes
- `wwwroot/js/` — frontend consumers
