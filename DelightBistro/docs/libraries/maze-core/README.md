# MazeCore

> Библиотека игрового движка лабиринта: клетки, герой, туман войны, звуки, магазин.

**Проект:** `MazeCore/MazeCore.csproj`

---

## Назначение

Переиспользуемый движок консольного лабиринта: генерация карты, персонажи, клетки (стены, ловушки, магазин и т.д.), отрисовка и звук. Используется учебным проектом FirstConsoleApp. MVC-модуль Maze удалён; DelightBistroMvc на MazeCore не ссылается.

---

## Публичный API

| Тип | Имя | Назначение |
|-----|-----|------------|
| Interface / Class | `IMaze` / `Maze` | Модель лабиринта |
| Interface / Class | `IMazeBuilder` / `MazeBuilder` | Построение карты |
| Interface / Class | `IMazeController` / `MazeController` | Игровой цикл (`Play`) |
| Class | `MazeDrawer` | Отрисовка в консоль |
| Class | `MazeFogOfWar` | Туман войны |
| Interface / Class | `IMazeSoundPlayer` / `MazeSoundPlayer` | Звуки (NAudio) |
| Class | `Hero` / `BaseCharacter` | Персонажи |
| Interface / Class | `IBaseCell` / `BaseCell` | Клетки карты |
| Class | `Shopkeeper`, `ShopMenuController` | Магазин на карте |

Примеры клеток: `Wall`, `Ground`, `Trap`, `Coin`, `Key`, `Doors`, `Portal`, `Lava`, `Ice`, `Ghost`, `Mimic`, `Rest`, `SecretRoom`.

---

## Зависимости

| Пакет / проект | Назначение |
|----------------|------------|
| `NAudio` | Воспроизведение звуков из `Sounds/` |

---

## Кто использует

| Проект | Как использует |
|--------|----------------|
| FirstConsoleApp | Консольная игра «лабиринт» (`MazeStuff/`, `Program.cs`) |

---

## Источники в коде

- `MazeCore/`
- `Cells/`, `Characters/`, `Interfaces/`
