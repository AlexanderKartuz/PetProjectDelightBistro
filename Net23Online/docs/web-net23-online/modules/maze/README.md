# Maze

> Интерактивная игра-лабиринт: просмотр, построение, движение. Использует библиотеку MazeCore.

**Контроллер:** `MazeController`  
**Layout:** default (`_Layout.cshtml`)  
**Точка входа:** `/Maze/Index`

---

## Назначение

Мини-игра: пользователь играет в лабиринт, строит свой (width/height/seed), двигается вправо и перестраивает карту. Вся логика — in-memory через MazeCore, без персистентности в БД.

---

## Маршруты и страницы

| URL | Action | View | Авторизация |
|-----|--------|------|-------------|
| `/Maze/Index` | `Index` | `Index.cshtml` | — |
| `/Maze/Builder` | GET/POST | `Builder.cshtml` | — |
| `/Maze/MoveRight` | GET | Redirect Index | — |
| `/Maze/RebuildMaze` | GET | Redirect (30×10, seed 0) | — |

---

## Встроенное API

Нет.

---

## SignalR

Нет.

---

## Сервисы и зависимости

| Сервис | Lifetime | Назначение |
|--------|----------|------------|
| `IMazeService` / `MazeService` | Singleton | In-memory maze state, movement, view mapping |
| `IMazeBuilder` / `MazeBuilder` | Singleton | Генерация через [MazeCore](../../../libraries/maze-core/README.md) |

> `MazeData` и `IMazeRepository` существуют в Data layer, но **не используются** контроллером.

---

## Модель данных

Не используется at runtime (in-memory only).

---

## Frontend

- **CSS:** `wwwroot/css/maze/maze.css`
- **Views:** `Index.cshtml`, `Builder.cshtml`

---

## Локализация

Нет.

---

## Фоновые задачи

Нет.

---

## Источники в коде

- `Controllers/MazeController.cs`
- `Services/MazeService.cs`
- `MazeCore/` (external library)
- `Views/Maze/`
