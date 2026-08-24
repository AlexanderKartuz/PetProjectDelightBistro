# DelightBistroMvc.Data

> Общий слой данных: WebContext, репозитории, Unit of Work, EF-модели и миграции для DelightBistroMvc.

**Проект:** `DelightBistroMvc.Data/DelightBistroMvc.Data.csproj`

---

## Назначение

Библиотека данных основного сайта: `WebContext`, сущности ресторана и пользователей, async-репозитории, тонкий `IUnitOfWork`, `IUserDataService` / BCrypt и EF Core миграции. DelightBistroMvc подключает её как ProjectReference и не держит DbContext у себя.

**DelightBistroMinimalApi** тоже ссылается на эту библиотеку: для JWT login регистрирует `WebContext` на `ConnectionStrings:Users` (каталог `WebNet23Online`) и использует `IUserDataService` / `IPasswordHasher` / `IUnitOfWork` / `UserRole`. Каталог напитков API остаётся на своём `MiniDbContext` / `WebNet23Tea`. → [jwt-auth.md](../../minimal-apis/delight-bistro/jwt-auth.md)

**БД (LocalDB):** каталог `WebNet23Online`. Runtime MVC задаёт строку в `Program.cs` (hardcoded). Design-time (`WebContextFactory`) читает `ConnectionStrings:DefaultDbConnection` из `DelightBistroMvc.Data/appsettings.json` — тот же каталог. Актуальная схема — только сущности текущего сайта (лишние таблицы старых модулей сняты миграцией вместе с переименованием `Password` → `PasswordHash`).

---

## Unit of Work

Тонкий UoW — только коммит Change Tracker:

| Тип | Назначение |
|-----|------------|
| `IUnitOfWork` / `UnitOfWork` | `SaveChangesAsync(CancellationToken)` → `WebContext.SaveChangesAsync` |

**Правила:**

1. Репозитории **не вызывают** `SaveChanges` — только query и трекинг (`AddAsync` / `UpdateAsync` / `DeleteAsync` / …).
2. После мутаций в одном use-case вызывающий код делает `await _unitOfWork.SaveChangesAsync()`.
3. **Исключение:** `IOrderRepository.DeleteExpiredOrderDatasAsync` — `ExecuteDeleteAsync` коммитит сам, UoW не нужен.
4. **Особый случай:** создание/правка блюда с картинкой — два `SaveChangesAsync` (получить `Id` → файл → обновить `ImgURL`).

`IUserDataService` (`RegisterAsync`, `UpdateLanguageAsync`, `UpdateProfileAsync`) сам вызывает UoW после репозитория.

---

## Публичный API

| Тип | Имя | Назначение |
|-----|-----|------------|
| Class | `WebContext` | DbContext основной БД |
| Class | `WebContextFactory` | Design-time factory для миграций (`DefaultDbConnection`) |
| Interface / Class | `IUnitOfWork` / `UnitOfWork` | Коммит изменений |
| Interface / Class | `IBaseRepository` / `BaseRepository` | Async CRUD без SaveChanges |
| Interface | `IDelightBistroRepository<T>` | Проверка уникальности имени |
| Interface / Class | `IUserRepository` / `UserRepository` | `GetByNameAsync`, `IsNameUniq`, CRUD |
| Interface / Class | `IUserDataService` / `UserDataService` | Регистрация, проверка пароля, язык/профиль (+ UoW) |
| Interface / Class | `IFoodItemRepository` / `FoodItemRepository` | Блюда; Include через `FoodItemIngredientDatas` |
| Interface / Class | `IMenuRepository` / `MenuRepository` | Меню + граф блюд/ингредиентов (`AsNoTracking` на чтении) |
| Interface / Class | `IIngredientsRepository` / `IngredientsRepository` | Ингредиенты |
| Interface / Class | `IOrderRepository` / `OrderRepository` | Заказы; `DeleteExpiredOrderDatasAsync` (`ExecuteDeleteAsync`) |
| Interface / Class | `INotificationRepository` / `NotificationRepository` | Уведомления; индекс `(IsActive, TimeToPublish)` |
| Interface / Class | `IChatMessageRepository` / `ChatMessageRepository` | NewChat; `GetRecentAsync(count)` |
| Interface / Class | `IPasswordHasher` / `BCryptPasswordHasher` | BCrypt, work factor 11 |
| Enum | `UserRole` | `User`, `Employee`, `Moderator`, `Admin` |

**DbSet в `WebContext`:**

- `Users` → `UserData` (`PasswordHash`, роль, язык, аватар; + `UserProfileData`; коллекция `ChatMessages`)
- `FoodItems` → `FoodItemData`
- `Ingredients` → `IngredientData`
- `Menus` → `MenuData`
- `Orders` → `OrderData`
- `Notifications` → `NotificationData`
- `Messages` → `ChatMessageData` (`SenderName`, `Text`, `CreatedAtUtc`, nullable `UserId`)
- Join: `FoodItemIngredientData` (M:M блюдо ↔ ингредиент с quantity; `HasPrecision` на quantity)
- Индексы: `Orders.CreatedDateTime`; `Notifications (IsActive, TimeToPublish)`; `ChatMessageData.CreatedAtUtc` (неуникальный)

---

## DI (DelightBistroMvc)

Явная регистрация Scoped в `Program.cs` (рефлексия `ResolveRepositories` / `[AutoRegister]` не используются):

```csharp
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFoodItemRepository, FoodItemRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IIngredientsRepository, IngredientsRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
```

Hosted services берут репозитории и UoW **из `CreateScope()`**, не из конструктора Singleton.

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
| DelightBistroMvc | DI `WebContext`, репозитории, `IUnitOfWork`, `IUserDataService`, `IPasswordHasher` |
| DelightBistroMinimalApi | JWT login: `WebContext` (`Users`), `IUnitOfWork`, `IUserDataService`, `IPasswordHasher`, `UserRole` — [jwt-auth.md](../../minimal-apis/delight-bistro/jwt-auth.md) |

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
- `Repositories/`, `Repositories/Interfaces/`, `Repositories/UnitOfWork.cs`
- `Services/UserService/`
- `Services/PasswordHasher/`
- `Migrations/`
