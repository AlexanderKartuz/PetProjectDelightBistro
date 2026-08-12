# {ApiProjectName}

> {Краткое описание API в 1–2 предложениях.}

**Проект:** `{ProjectFolder}/{ProjectName}.csproj`  
**Порт (HTTPS):** {port}  
**Swagger:** `https://localhost:{port}/swagger`

---

## Назначение

{Что делает API, зачем вынесен отдельно от WebNet23Online.}

---

## Запуск

| Параметр | Значение |
|----------|----------|
| HTTPS | `https://localhost:{port}` |
| HTTP | `http://localhost:{httpPort}` |
| Swagger | `/swagger` |

---

## Эндпоинты

| Метод | Путь | Описание | Request | Response |
|-------|------|----------|---------|----------|
| | | | | |

---

## DbContext и БД

| Параметр | Значение |
|----------|----------|
| DbContext | `{DbContextName}` |
| База данных | `{DatabaseName}` |
| Connection | LocalDB |

**Сущности:**

- 

---

## Middleware и инфраструктура

- **CORS:** {описание}
- **Кэш / Redis / Rate limiting:** {если есть, иначе «Нет»}

---

## Потребители

| Потребитель | Файл | URL API |
|-------------|------|---------|
| | | |

{Или: «Standalone — потребителей в WebNet23Online нет.»}

---

## Миграции

```bash
dotnet ef migrations add {MigrationName} --project Net23Online/{ProjectFolder} --startup-project Net23Online/{ProjectFolder}
```

Применение миграции — **только вручную** человеком.

---

## Источники в коде

- `Program.cs`
- `Properties/launchSettings.json`
- Models / DbContext
