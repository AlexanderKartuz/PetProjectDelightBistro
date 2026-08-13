# DelightBistroMvc.Data

> Общий слой данных: WebContext, репозитории, EF-модели и миграции для DelightBistroMvc.

**Проект:** `DelightBistroMvc.Data/DelightBistroMvc.Data.csproj`

---

## Назначение

Библиотека данных основного сайта: `WebContext`, сущности ресторана и пользователей, репозитории, BCrypt-хэширование паролей и EF Core миграции. DelightBistroMvc подключает её как ProjectReference и не держит DbContext у себя.

**БД (LocalDB):** каталог `WebNet23Online`. Runtime MVC задаёт строку в `Program.cs` (hardcoded). Design-time (`WebContextFactory`) читает `ConnectionStrings:DefaultDbConnection` из `DelightBistroMvc.Data/appsettings.json` — тот же каталог. Актуальная схема — только сущности текущего сайта (лишние таблицы старых модулей сняты миграцией вместе с переименованием `Password` → `PasswordHash`).

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
| Interface / Class | `IPasswordHasher` / `BCryptPasswordHasher` | BCrypt, work factor 11; хэш при `Registration`, проверка при логине |
| Enum | `UserRole` | `User`, `Employee`, `Moderator`, `Admin` |

**DbSet в `WebContext`:**

- `Users` → `UserData` (`PasswordHash`, роль, язык, аватар; + `UserProfileData`)
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
