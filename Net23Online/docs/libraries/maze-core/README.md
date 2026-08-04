# MazeCore

> Библиотека игрового движка лабиринта: клетки, герой, туман войны, звуки.

> TODO: наполнить по шаблону [_templates/shared-library.md](../../_templates/shared-library.md).

---

## Назначение

Переиспользуемый движок консольного лабиринта: генерация карты, персонажи, клетки (стены, ловушки, магазин и т.д.), отрисовка и звук. Используется учебным проектом FirstConsoleApp. MVC-модуль Maze из WebNet23Online удалён.

---

## Публичный API

| Тип | Имя | Назначение |
|-----|-----|------------|
| Class | `Maze` / `IMaze` | Модель лабиринта |
| Class | `MazeBuilder` / `IMazeBuilder` | Построение карты |
| Class | `MazeController` / `IMazeController` | Игровой цикл |
| Class | `MazeDrawer` | Отрисовка в консоль |
| Class | `MazeFogOfWar` | Туман войны |
| Class | `MazeSoundPlayer` / `IMazeSoundPlayer` | Звуки |
| Class | `Hero` / `BaseCharacter` | Персонажи |
| Class / Interface | `BaseCell` / `IBaseCell` | Клетки карты |

---

## Зависимости

| Пакет / проект | Назначение |
|----------------|------------|
| — | Стандартная библиотека .NET, без внешних пакетов данных |

---

## Кто использует

| Проект | Как использует |
|--------|----------------|
| FirstConsoleApp | Консольная игра «лабиринт» (`MazeStuff/`) |

> В `WebNet23Online.csproj` может оставаться ProjectReference на MazeCore без использования в коде.

---

## Источники в коде

- `MazeCore/`
- `Cells/`, `Characters/`, `Interfaces/`
