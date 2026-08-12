# WebNet23Online.Data

> Общий слой данных: WebContext, репозитории, EF-модели и миграции для WebNet23Online.

**Проект:** `WebNet23Online.Data/WebNet23Online.Data.csproj`

---

## Назначение

Библиотека данных основного сайта: `WebContext`, сущности, репозитории и EF Core миграции. WebNet23Online подключает её как ProjectReference и не держит DbContext у себя.

---

## Публичный API

| Тип | Имя | Назначение |
|-----|-----|------------|
| Class | `WebContext` | DbContext основной БД |
| Class | `WebContextFactory` | Design-time factory для миграций |
| Interface / Class | `IBaseRepository` / `BaseRepository` | Базовый CRUD |
| Interface / Class | `IUserRepository` / `UserRepository` | Пользователи |
| Interface / Class | `IFoodItemRepository` / `FoodItemRepository` | Блюда |
| Interface / Class | `IMenuRepository` / `MenuRepository` | Меню |
| Interface / Class | `IIngredientsRepository` / `IngredientsRepository` | Ингредиенты |
| Interface / Class | `IOrderRepository` / `OrderRepository` | Заказы |
| Interface / Class | `INotificationRepository` / `NotificationRepository` | Уведомления |
| Enum | `UserRole` | `User`, `Employee`, `Moderator`, `Admin` |

**DbSet в `WebContext`:**

- `Users` → `UserData` (+ `UserProfileData`)
- `FoodItems` → `FoodItemData`
- `Ingredients` → `IngredientData`
- `Menus` → `MenuData`
- `Orders` → `OrderData`
- `Notifications` → `NotificationData`
- Join: `FoodItemIngredientData` (M:M блюдо ↔ ингредиент с quantity)

---

## Зависимости

| Пакет / проект | Назначение |
|----------------|------------|
| `Microsoft.EntityFrameworkCore.SqlServer` | SQL Server / LocalDB |
| `Microsoft.EntityFrameworkCore.Tools` | Миграции |

---

## Кто использует

| Проект | Как использует |
|--------|----------------|
| WebNet23Online | DI `WebContext`, репозитории, модели ViewModels |

---

## Миграции

```bash
dotnet ef migrations add {MigrationName} --project Net23Online/WebNet23Online.Data --startup-project Net23Online/WebNet23Online
```

Применение — **только вручную**:

```bash
dotnet ef database update --project Net23Online/WebNet23Online.Data --startup-project Net23Online/WebNet23Online
```

---

## Источники в коде

- `WebNet23Online.Data/`
- `WebContext.cs`
- `Models/`, `DataModels/`
- `Repositories/`
- `Migrations/`
