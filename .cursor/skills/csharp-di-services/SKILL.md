---
name: csharp-di-services
description: >-
  C# and .NET coding conventions: prefer instance services over static methods,
  register in DI, inject at call sites. Use when writing, editing, or reviewing
  C# code (.cs files), ASP.NET Core APIs, Minimal APIs, controllers, services,
  or when refactoring static helpers into injectable services.
---

# C# — сервисы вместо static

## Главное правило

Нельзя использовать статичные методы, если они могут быть заменены обычным сервисами.
Их нужно переписать в обычные инстанс методы и в местах использования просто запрашивай этот сервис из DI.

## Когда применять

При создании или изменении C# кода:

1. **Новая логика** — сразу в класс-сервис с инстанс-методами, не в `static` helper.
2. **Существующий `static`** — если метод содержит бизнес-логику, работу с БД, конфигом или другими зависимостями, перенести в сервис.
3. **Ревью** — отметить `static`-методы, которые можно заменить сервисом.

## Когда static допустим

Не трогать без явной необходимости:

- Точка входа (`Program.cs`, `Main`)
- Автогенерированный код (`.Designer.cs`, EF migrations)
- Чистые extension-методы без состояния и зависимостей (например, `Shuffle` для `IList<T>`)
- Константы и `readonly static` поля

## Паттерн реализации

### 1. Класс-сервис

```csharp
public class TagService
{
    private readonly MiniDbContext _dbContext;

    public TagService(MiniDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string Normalize(string tagName) => tagName.Trim();

    public MovieDto ToDto(Movie movie) => new(/* ... */);
}
```

### 2. Регистрация в DI

```csharp
builder.Services.AddScoped<TagService>();
// или AddSingleton / AddTransient — по смыслу lifetime
```

### 3. Использование — запрос из DI

**Minimal API** — параметр в делегате:

```csharp
app.MapGet("GetMovies", async (MiniDbContext db, TagService tagService) =>
{
    return movies.Select(tagService.ToDto).ToList();
});
```

**Controller** — через конструктор:

```csharp
public class MoviesController : ControllerBase
{
    private readonly TagService _tagService;

    public MoviesController(TagService tagService) => _tagService = tagService;
}
```

**Другой сервис** — инжект в конструктор, не `TagService.Method()` статически.

## Рефакторинг static → сервис

1. Создать класс (или расширить существующий сервис).
2. Перенести логику в инстанс-методы; зависимости — через конструктор.
3. Зарегистрировать в `Program.cs` / `Startup`.
4. Заменить все вызовы `SomeHelper.Do()` на `_someService.Do()` или параметр из DI.
5. Удалить `static` класс, если он больше не нужен.

## Антипаттерны

```csharp
// ❌ бизнес-логика в static
public static class MovieMapper
{
    public static MovieDto ToDto(Movie movie) => /* ... */;
}

// ❌ прямой вызов без DI
var dto = MovieMapper.ToDto(movie);

// ✅ сервис + DI
var dto = tagService.ToDto(movie);
```

```csharp
// ❌ static с зависимостями через ServiceLocator / new
public static class TagHelper
{
    public static async Task<Tag> FindOrCreate(string name)
    {
        using var db = new MiniDbContext(/* ... */);
        // ...
    }
}

// ✅ зависимость через конструктор
public class TagService
{
    private readonly MiniDbContext _dbContext;
    public TagService(MiniDbContext dbContext) => _dbContext = dbContext;
}
```

## Lifetime

| Тип | Когда |
|-----|-------|
| `Scoped` | Работа с `DbContext`, HTTP-запрос |
| `Singleton` | Кэш, конфиг, stateless |
| `Transient` | Лёгкие stateless операции на каждый resolve |

При сомнении для ASP.NET Core с EF — `Scoped`.
