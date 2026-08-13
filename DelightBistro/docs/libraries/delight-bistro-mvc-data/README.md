# DelightBistroMvc.Data

> Общий слой данных: WebContext, репозитории, EF-модели и миграции для DelightBistroMvc.

**Проект:** `DelightBistroMvc.Data/DelightBistroMvc.Data.csproj`

---

## Назначение

Библиотека данных основного сайта: `WebContext`, сущности, репозитории, хэширование паролей и EF Core миграции. DelightBistroMvc подключает её как ProjectReference и не держит DbContext у себя.

Имя БД в connection string по-прежнему `WebNet23Online` (LocalDB).

---

## Публичный API

| Тип | Имя | Назначение |
|-----|-----|------------|
| Class | `WebContext` | DbContext основной БД |
| Class | `WebContextFactory` | Design-time factory для миграций (`DefaultDbConnection`) |
| Interface / Class | `IBaseRepository` / `BaseRepository` | Базовый CRUD |
| Interface | `IDelightBistroRepository<T>` | Проверка уникальности имени |
| Interface / Class | `IUserRepository` / `UserRepository` | Пользователи |
| Interface / Class | `IFoodItemRepository` / `FoodItemRepository` | Блюда |
| Interface / Class | `IMenuRepository` / `MenuRepository` | Меню |
| Interface / Class | `IIngredientsRepository` / `IngredientsRepository` | Ингредиенты |
| Interface / Class | `IOrderRepository` / `OrderRepository` | Заказы |
| Interface / Class | `INotificationRepository` / `NotificationRepository` | Уведомления |
| Interface / Class | `IPasswordHasher` / `BCryptPasswordHasher` | Хэш и проверка пароля |
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
| `BCrypt.Net-Next` | Хэширование паролей |

---

## Кто использует

| Проект | Как использует |
|--------|----------------|
| DelightBistroMvc | DI `WebContext`, репозитории, `IPasswordHasher`, модели ViewModels |

---

## Миграции

```bash
dotnet ef migrations add {MigrationName} --project DelightBistro/DelightBistroMvc.Data --startup-project DelightBistro/DelightBistroMvc
```

Применение — **только вручную**:

```bash
dotnet ef database update --project DelightBistro/DelightBistroMvc.Data --startup-project DelightBistro/DelightBistroMvc
```

---

## Источники в коде

- `DelightBistroMvc.Data/`
- `WebContext.cs`
- `Models/`, `DataModels/`
- `Repositories/`
- `Services/PasswordHasher/`
- `Migrations/`
