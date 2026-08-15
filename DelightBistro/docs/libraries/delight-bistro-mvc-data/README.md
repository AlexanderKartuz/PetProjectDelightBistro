# DelightBistroMvc.Data

> Общий слой данных: WebContext, репозитории, EF-модели и миграции для DelightBistroMvc.

**Проект:** `DelightBistroMvc.Data/DelightBistroMvc.Data.csproj`

---

## Назначение

Библиотека данных основного сайта: `WebContext`, сущности ресторана и пользователей, репозитории, `IUserDataService` / BCrypt и EF Core миграции. DelightBistroMvc подключает её как ProjectReference и не держит DbContext у себя.

**DelightBistroMinimalApi** тоже ссылается на эту библиотеку: для JWT login регистрирует `WebContext` на `ConnectionStrings:Users` (каталог `WebNet23Online`) и использует `IUserDataService` / `IPasswordHasher` / `UserRole`. Каталог напитков API остаётся на своём `MiniDbContext` / `WebNet23Tea`. → [jwt-auth.md](../../minimal-apis/delight-bistro/jwt-auth.md)

**БД (LocalDB):** каталог `WebNet23Online`. Runtime MVC задаёт строку в `Program.cs` (hardcoded). Design-time (`WebContextFactory`) читает `ConnectionStrings:DefaultDbConnection` из `DelightBistroMvc.Data/appsettings.json` — тот же каталог. Актуальная схема — только сущности текущего сайта (лишние таблицы старых модулей сняты миграцией вместе с переименованием `Password` → `PasswordHash`).

---

## Публичный API

| Тип | Имя | Назначение |
|-----|-----|------------|
| Class | `WebContext` | DbContext основной БД |
| Class | `WebContextFactory` | Design-time factory для миграций (`DefaultDbConnection`) |
| Interface / Class | `IBaseRepository` / `BaseRepository` | Базовый CRUD |
| Interface | `IDelightBistroRepository<T>` | Проверка уникальности имени |
| Interface / Class | `IUserRepository` / `UserRepository` | `GetByName`, `IsNameUniq`, CRUD |
| Interface / Class | `IUserDataService` / `UserDataService` | Регистрация, проверка пароля, язык/профиль |
| Interface / Class | `IFoodItemRepository` / `FoodItemRepository` | Блюда; Include через `FoodItemIngredientDatas` |
| Interface / Class | `IMenuRepository` / `MenuRepository` | Меню + граф блюд/ингредиентов (`AsNoTracking` на чтении) |
| Interface / Class | `IIngredientsRepository` / `IngredientsRepository` | Ингредиенты |
| Interface / Class | `IOrderRepository` / `OrderRepository` | Заказы; `DeleteExpiredOrders` (`ExecuteDelete`) |
| Interface / Class | `INotificationRepository` / `NotificationRepository` | Уведомления; индекс `(IsActive, TimeToPublish)` |
| Interface / Class | `IPasswordHasher` / `BCryptPasswordHasher` | BCrypt, work factor 11 |
| Enum | `UserRole` | `User`, `Employee`, `Moderator`, `Admin` |

**DbSet в `WebContext`:**

- `Users` → `UserData` (`PasswordHash`, роль, язык, аватар; + `UserProfileData`)
- `FoodItems` → `FoodItemData`
- `Ingredients` → `IngredientData`
- `Menus` → `MenuData`
- `Orders` → `OrderData`
- `Notifications` → `NotificationData`
- Join: `FoodItemIngredientData` (M:M блюдо ↔ ингредиент с quantity; `HasPrecision` на quantity)
- Индексы: `Orders.CreatedDateTime`; `Notifications (IsActive, TimeToPublish)`

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
| DelightBistroMvc | DI `WebContext`, репозитории, `IUserDataService`, `IPasswordHasher` |
| DelightBistroMinimalApi | JWT login: `WebContext` (`Users`), `IUserDataService`, `IPasswordHasher`, `UserRole` — [jwt-auth.md](../../minimal-apis/delight-bistro/jwt-auth.md) |

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
- `Services/UserService/`
- `Services/PasswordHasher/`
- `Migrations/`
